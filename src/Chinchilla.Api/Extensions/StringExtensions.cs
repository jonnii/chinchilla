using System;
using System.Collections.Generic;
using System.Linq;

namespace Chinchilla.Api.Extensions
{
    public static class StringExtensions
    {
        private static readonly Dictionary<string, Func<string>> replacements
           = new Dictionary<string, Func<string>>();

        static StringExtensions()
        {
            ResetReplacements();
        }

        public static void ResetReplacements()
        {
            replacements.Clear();
        }

        public static void AddReplacement(string replacement, Func<string> replacementFunction)
        {
            replacements.Add(replacement, replacementFunction);
        }

        public static bool HasReplacement(string replacement)
        {
            return replacements.ContainsKey(replacement);
        }

        public static string FormatWithReplacements(this string original)
        {
            return replacements.Aggregate(
                original,
                (current, replacement) => current.Replace(replacement.Key, replacement.Value()));
        }
    }
}
