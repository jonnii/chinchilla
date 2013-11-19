using RestSharp;

namespace Chinchilla.Api
{
    public class RabbitJsonSerializerStrategy : PocoJsonSerializerStrategy
    {
        protected override string MapClrMemberNameToJsonFieldName(string clrPropertyName)
        {
            return base.MapClrMemberNameToJsonFieldName(clrPropertyName.ToLowerInvariant());
        }
    }
}