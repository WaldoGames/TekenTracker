using Core.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekenTracker.UnitTests
{
    public class EncryptionUnitTests
    {
        [Fact]
        public void encryption_directTest_should_be_true()
        {
            string pass = "helloWorldPassWord";

            Encryption enc = new Encryption();

            string encPass = enc.EncryptNewString(pass);

            Assert.True(enc.CompareEncryptedString(pass, encPass));
        }
        [Fact]
        public void encryption_PreGeneratedTest_should_be_true()
        {
            string pass = "helloWorldPassWord";
            string encPass = "$2a$11$2yuIcgA7BCvQyvMjl0FvpeCyfFV0d9EIaA7yPebJEfz13SL1eEvL.";

            Encryption enc = new Encryption();



            Assert.True(enc.CompareEncryptedString(pass, encPass));
        }

        [Fact]
        public void encryption_WrongPasswordTest_should_be_false()
        {
            string pass = "helloWorldNewPassword123";
            string encPass = "$2a$11$2yuIcgA7BCvQyvMjl0FvpeCyfFV0d9EIaA7yPebJEfz13SL1eEvL.";

            Encryption enc = new Encryption();
            Assert.False(enc.CompareEncryptedString(pass, encPass));
        }
    }
}
