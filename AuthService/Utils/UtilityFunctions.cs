using System;
using System.Linq;
using System.Text;

namespace AuthService.Utils
{
    public class UtilityFunctions
    {
        public static string RemoveControlCharacters(string inputString)
        {
            return new string(inputString.Where(c => !char.IsControl(c)).ToArray());
        }

        public static string DecodeBase64String(string encodedString)
        {
            var data = Convert.FromBase64String(encodedString);
            var decodedString = Encoding.UTF8.GetString(data);

            return decodedString;
        }
    }
}