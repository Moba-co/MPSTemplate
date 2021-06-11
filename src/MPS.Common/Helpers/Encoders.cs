using System;
using System.Text;

namespace Moba.Common.Helpers
{
    public static class Encoders
    {
        public static string Base64Encode(string text)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}