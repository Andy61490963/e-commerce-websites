using System.Security.Cryptography;
using System.Text;

namespace Shopping.lib.Helpers
{

    public static class HashHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Sha512(string str)
        {
            using var sha = SHA512.Create();
            var source = Encoding.Default.GetBytes(str);
            var computeHash = sha.ComputeHash(source);
            return Convert.ToBase64String(computeHash);
        }

        private static byte[] ToSha256(this string text)
        {
            return SHA256.HashData(Encoding.UTF8.GetBytes(text));
        }

        public static string ToSHA256String(this string text)
        {
            var shA256 = text.ToSha256();
            var stringBuilder = new StringBuilder();

            foreach (var num in shA256)
            {
                stringBuilder.Append(num.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}