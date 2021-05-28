using System;
using System.Security.Cryptography;
using System.Text;


namespace Autentification
{
    public static class Hasher
    {
        public static string GetHashedPassword(string pass)
        {
            SHA256 sha256Hash = SHA256.Create();
            string hash = GetHash(sha256Hash, pass);
            return hash;
        }


        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
        
            var sBuilder = new StringBuilder();
        
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
        
            return sBuilder.ToString();
        }

        public static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            var hashOfInput = GetHash(hashAlgorithm, input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}