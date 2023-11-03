using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Classes
{
    public class TokenGenerator
    {
        public string GenerateToken(int Length = 40)
        {
            byte[] TokenBytes = new byte[Length];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(TokenBytes);
            }

            // Convert the random bytes to a hex string for display.
            string hexString = BitConverter.ToString(TokenBytes);

            return hexString;
        }
    }
}
