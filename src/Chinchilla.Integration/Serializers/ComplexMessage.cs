using System;

namespace Chinchilla.Integration.Serializers
{
    public class ComplexMessage
    {
        public enum AnEnum
        {
            value1,
            value2,
            value3
        }

        public static ComplexMessage Default
        {
            get
            {
                return new ComplexMessage
                {
                    SimpleString = "simple-string",
                    SimpleEnum = AnEnum.value2,
                    SimpleInteger = 3,
                    SimpleDate = new DateTime(2013, 10, 5, 10, 30, 40, DateTimeKind.Utc),
                    NullableEnum = null,
                    NullableEnumWithValue = AnEnum.value3
                };
            }
        }

        public string SimpleString { get; set; }

        public AnEnum SimpleEnum { get; set; }

        public int SimpleInteger { get; set; }

        public DateTime SimpleDate { get; set; }

        public AnEnum? NullableEnum { get; set; }

        public AnEnum? NullableEnumWithValue { get; set; }
    }
}