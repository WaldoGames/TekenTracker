using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace Core.Classes
{
    public class Encryption//put back on internal after test(12-10-2023)
    {
        public string EncryptNewString(string Password)
        {
            string pass = BC.HashPassword(Password);

            return pass;
        }

        public bool CompareEncryptedString(string Password, string EncyptedPassword)
        {
            bool tmp = BC.Verify(Password, EncyptedPassword);

            return tmp;
        }
    }
}
