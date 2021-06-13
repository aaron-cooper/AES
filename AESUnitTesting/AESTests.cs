using NUnit.Framework;
using System;
using Cryptography;

namespace AESUnitTesting
{
    class AESTests
    {
        [Test]
        public void Test_AESConstructor()
        {
            AES aes = new AES();
        }
        [Test]
        public void Test_AESConstructorWithArgs()
        {
            byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

            AES aes = new AES(key, iv);
        }
        [Test]
        public void Test_AESConstructorNullKey()
        {
            byte[] key = null;
            byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            try
            {
                AES aes = new AES(key, iv);
            }
            catch (ArgumentException e)
            {
                Assert.Pass();
            }
        }
        [Test]
        public void Test_AESConstructorInvalidKey()
        {
            byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
            byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            try
            {
                AES aes = new AES(key, iv);
            }
            catch (ArgumentException e)
            {
                Assert.Pass();
            }
        }
        [Test]
        public void Test_AESConstructorWithValidArgs()
        {
            byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            AES aes = new AES(key, iv);
        }
        [Test]
        public void Test_AESConstructorNullIV()
        {
            byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            byte[] iv = null;
            try
            {
                AES aes = new AES(key, iv);
            }
            catch (ArgumentException e)
            {
                Assert.Pass();
            }
        }
        [Test]
        public void Test_AESConstructorInvalidIV()
        {
            byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
            try
            {
                AES aes = new AES(key, iv);
            }
            catch (ArgumentException e)
            {
                Assert.Pass();
            }
        }
        [Test]
        public void Test_AESConstructorWithValidArgs24ByteKey()
        {
            byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23};
            byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            AES aes = new AES(key, iv);
        }
        [Test]
        public void Test_AESConstructorWithValidArgs32ByteKey()
        {
            byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23};
            byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            AES aes = new AES(key, iv);
        }
        [Test]
        public void Test_AESValidKeySizes()
        {
            var keySizes = AES.ValidKeySizes;
            if (keySizes.Contains(16) && keySizes.Contains(24) && keySizes.Contains(32))
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
        [Test]
        public void Test_AESChangeValidKeySizes()
        {
            AES.ValidKeySizes[0] = 2435662;
            Test_AESValidKeySizes();
        }
        [Test]
        public void Test_AESMaximumKeySize()
        {
            Assert.AreEqual(32, AES.MaximumKeySize);
        }
        [Test]
        public void Test_AESMinimumKeySize()
        {
            Assert.AreEqual(16, AES.MinimumKeySize);
        }
        [Test]
        public void Test_AESValidIVSize()
        {
            Assert.AreEqual(16, AES.ValidIVSize);
        }
        [Test]
        public void Test_AESValidBlockSize()
        {
            Assert.AreEqual(16, AES.ValidBlockSize);
            AES aes = new AES();
        }
        [Test]
        public void Test_AESBlockSize()
        {
            AES aes = new AES();
            Assert.AreEqual(16, aes.BlockSize);
        }
        [Test]
        public void Test_AESGenerateKey()
        {
            AES aes = new AES();
            byte[] key = aes.Key;
            aes.GenerateKey();
            Assert.IsFalse(TestUtilities.AreArraysEqual(key, aes.Key), "Keys before and after GenerateKey call are identical");
        }
        [Test]
        public void Test_AESGenerateIV()
        {
            AES aes = new AES();
            byte[] iv = aes.IV;
            aes.GenerateIV();
            Assert.IsFalse(TestUtilities.AreArraysEqual(iv, aes.IV), "IVs before and after GenerateIV call are identical");
        }
        [Test]
        public void Test_AESDispose()
        {
            AES aes = new AES();
            aes.Dispose();
            aes.Dispose();

            byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            aes = new AES(key, iv);
            aes.Dispose();
            aes.Dispose();
        }
    }
}
