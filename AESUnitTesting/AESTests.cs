using NUnit.Framework;
using System;

namespace AESUnitTesting
{
    class AESTests
    {
        [Test]
        public void Test_AESConstructor()
        {
            AES.AES aes = new AES.AES();
        }
        [Test]
        public void Test_AESConstructorWithArgs()
        {
            byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

            AES.AES aes = new AES.AES(key, iv);
        }
        [Test]
        public void Test_AESConstructorNullKey()
        {
            byte[] key = null;
            byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            try
            {
                AES.AES aes = new AES.AES(key, iv);
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
                AES.AES aes = new AES.AES(key, iv);
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
            AES.AES aes = new AES.AES(key, iv);
        }
        [Test]
        public void Test_AESConstructorNullIV()
        {
            byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            byte[] iv = null;
            try
            {
                AES.AES aes = new AES.AES(key, iv);
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
                AES.AES aes = new AES.AES(key, iv);
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
            AES.AES aes = new AES.AES(key, iv);
        }
        [Test]
        public void Test_AESConstructorWithValidArgs32ByteKey()
        {
            byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23};
            byte[] iv = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            AES.AES aes = new AES.AES(key, iv);
        }
    }
}
