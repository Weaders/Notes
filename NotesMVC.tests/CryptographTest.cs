using NotesMVC.Services.Encrypter;
using System;
using Xunit;
using System.Reflection;
using Xunit.Abstractions;

namespace NotesMVC.tests {
    public class CryptographTest {

        private readonly ITestOutputHelper output;

        public CryptographTest(ITestOutputHelper _output) {
            output = _output;
        }

        [Fact]
        public void GetBadCrypto() {

            var cryptoMng = new CryptographManager();

            Assert.Throws<Exception>(() => { CryptographType.Get("test"); });

        }

        [Fact]
        public void GetAllCryptoTypes() {

            var fields = typeof(CryptographType).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields) {

                var cryptoType = (CryptographType)field.GetValue(null);

                output.WriteLine("Try find for type - " + cryptoType.Type);

                Assert.Equal(CryptographType.Get(cryptoType.Type), cryptoType);

            }

        }

    }
}
