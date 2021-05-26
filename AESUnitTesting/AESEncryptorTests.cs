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

        [Test]
        public void Test_encryptSmallFinalBlock()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] toTransform = { 0, 1, 2, 3, 4, 5, 6, 7 };
            byte[] after = encryptor.TransformFinalBlock(toTransform, 0, toTransform.Length);

            byte[] expectedAfter = { 140, 58, 160, 75, 146, 104, 11, 200, 169, 67, 22, 127, 121, 91, 25, 17 };
            CheckArraysEqual(after, expectedAfter);
        }

//=========================================================== common
        private void CheckArraysEqual(byte[] arr1, byte[] arr2)
        {
            Assert.AreEqual(arr1.Length, arr2.Length);
            for (int i = 0; i < arr1.Length; i++)
            {
                Assert.AreEqual(arr1[i], arr2[i]);
            }
        }

        
    }
}
