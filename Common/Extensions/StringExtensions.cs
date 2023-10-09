using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        public static string Capitalize(this string text) =>
            (string.IsNullOrEmpty(text)) ? string.Empty : char.ToUpper(text[0]) + text[1..];
    }
}
