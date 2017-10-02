using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Chinchilla.Serializers
{
    public class DefaultMessageTypeFactory : IMessageTypeFactory
    {
        private const MethodAttributes DuckMethodAttributes =
            MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig;

        private readonly ConcurrentDictionary<Type, Func<object>> factories = new ConcurrentDictionary<Type, Func<object>>();

        private readonly Lazy<ModuleBuilder> moduleBuilder;

        public DefaultMessageTypeFactory()
        {
            moduleBuilder = new Lazy<ModuleBuilder>(GetModuleBuilder);
        }

        public bool HasCachedFactory(Type type)
        {
            return factories.ContainsKey(type);
        }

        public Func<object> GetTypeFactory(Type interfaceType)
        {
            return factories.GetOrAdd(interfaceType, CreateFactory);
        }

        private ModuleBuilder GetModuleBuilder()
        {
            var assemblyName = new AssemblyName { Name = "Chinchilla_DynamicTypes" };
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            return assemblyBuilder.DefineDynamicModule("Chinchilla_DynamicTypes");
        }

        private Func<object> CreateFactory(Type interfaceType)
        {
            var typeBuilder = moduleBuilder.Value.DefineType(
                interfaceType.Name + "_backing",
                TypeAttributes.Public | TypeAttributes.Class);

            typeBuilder.AddInterfaceImplementation(interfaceType);

            foreach (var interfaceProperty in interfaceType.GetProperties())
            {
                CreateProperty(typeBuilder, interfaceProperty);
            }

            var generatedType = typeBuilder.CreateTypeInfo();

            var newExpression = Expression.New(generatedType.AsType());
            var lambda = Expression.Lambda(newExpression);
            return (Func<object>)lambda.Compile();
        }

        private void CreateProperty(TypeBuilder typeBuilder, PropertyInfo interfaceProperty)
        {
            var propertyName = interfaceProperty.Name;

            var field = typeBuilder.DefineField(
                propertyName,
                interfaceProperty.PropertyType,
                FieldAttributes.Private);

            var property = typeBuilder.DefineProperty(
                propertyName,
                PropertyAttributes.None,
                interfaceProperty.PropertyType,
                new[] { interfaceProperty.PropertyType });

            var getMethodBuilder = typeBuilder.DefineMethod(
                "get_value",
                DuckMethodAttributes,
                interfaceProperty.PropertyType,
                Type.EmptyTypes);

            var getGenerator = getMethodBuilder.GetILGenerator();
            getGenerator.Emit(OpCodes.Ldarg_0);
            getGenerator.Emit(OpCodes.Ldfld, field);
            getGenerator.Emit(OpCodes.Ret);

            var setMethodBuilder = typeBuilder.DefineMethod(
                "set_value",
                DuckMethodAttributes,
                null,
                new[] { interfaceProperty.PropertyType });

            var setGenerator = setMethodBuilder.GetILGenerator();
            setGenerator.Emit(OpCodes.Ldarg_0);
            setGenerator.Emit(OpCodes.Ldarg_1);
            setGenerator.Emit(OpCodes.Stfld, field);
            setGenerator.Emit(OpCodes.Ret);

            property.SetGetMethod(getMethodBuilder);
            property.SetSetMethod(setMethodBuilder);

            typeBuilder.DefineMethodOverride(getMethodBuilder, interfaceProperty.GetGetMethod());
            typeBuilder.DefineMethodOverride(setMethodBuilder, interfaceProperty.GetSetMethod());
        }
    }
}
