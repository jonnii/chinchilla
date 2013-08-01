using System.Text.RegularExpressions;
using SpeakEasy.Serializers;

namespace Chinchilla.Api
{
    public class RabbitJsonSerializerStrategy : DefaultJsonSerializer.DefaultJsonSerializerStrategy
    {
        protected override string MapClrMemberNameToJsonFieldName(string clrPropertyName)
        {
            return Regex.Replace(clrPropertyName, "([a-z])([A-Z])", "$1_$2").ToLower();
        }
    }
}