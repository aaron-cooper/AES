using NUnit.Framework;
using System.Security.Cryptography;
using AES;

namespace AESUnitTesting
{
    class AESEncryptorTests
    {
        private byte[] Key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        private byte[] IV = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

        [Test]
        public void Test_CreateEncryptor()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform encryptor = aes.CreateEncryptor();
        }
    }
}
