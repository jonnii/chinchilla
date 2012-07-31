using System;

namespace Chinchilla
{
    public static class Guard
    {
        public static T NotNull<T>(T t, string paramName)
            where T : class
        {
            if (t == null)
            {
                throw new ArgumentNullException(paramName);
            }

            return t;
        }
    }
}
