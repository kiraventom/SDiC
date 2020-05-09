using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDiC.Authorization.Other
{
    public static class Hasher
    {
        public static string GetHash(string input)
        {
            byte[] data;
            using (System.Security.Cryptography.SHA256 sha = System.Security.Cryptography.SHA256.Create())
            {
                data = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            }

            var sb = new StringBuilder();
            foreach (byte b in data)
            {
                sb.Append(b.ToString("x2")); // HEX
            }

            return sb.ToString();
        }

        public static bool VerifyHash(string input, string hash)
        {
            var hashOfInput = GetHash(input);
            return hashOfInput.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
