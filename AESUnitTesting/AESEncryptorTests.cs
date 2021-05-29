using NUnit.Framework;
using System.Security.Cryptography;
using AES;
using System;

namespace AESUnitTesting
{
    class AESEncryptorTests
    {
        const int blocksize = 16;
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
        [Test]
        public void Test_encryptSmallFinalBlockAfterOtherBlock()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform encryptor = aes.CreateEncryptor();

            byte[] firstBlock = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            byte[] finalBlock = { 0, 1, 2, 3, 4, 5, 6, 7 };

            byte[] firstOutput = new byte[16];
            byte[] finalOutput;

            encryptor.TransformBlock(firstBlock, 0, 16, firstOutput, 0);
            finalOutput = encryptor.TransformFinalBlock(finalBlock, 0, finalBlock.Length);

            byte[] firstExpected = { 242, 144, 0, 182, 42, 73, 159, 208, 169, 243, 154, 106, 221, 46, 119, 128, };
            byte[] finalExpected = { 25, 5, 33, 39, 94, 197, 236, 127, 71, 122, 213, 35, 220, 108, 81, 8, };
            CheckArraysEqual(firstOutput, firstExpected);
            CheckArraysEqual(finalExpected, finalOutput);
        }
        [Test]
        public void Test_encryptLargeFinalBlock()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform encryptor = aes.CreateEncryptor();

            byte[] toTransform = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, };
            byte[] output = encryptor.TransformFinalBlock(toTransform, 0, toTransform.Length);
            byte[] expected = { 242, 144, 0, 182, 42, 73, 159, 208, 169, 243, 154, 106, 221, 46, 119, 128, 149, 67, 184, 111, 192, 70, 250, 136, 58, 148, 70, 184, 46, 71, 209, 45, 245, 6, 211, 246, 165, 133, 51, 129, 86, 215, 23, 211, 134, 144, 25, 77, };
            CheckArraysEqual(output, expected);
        }

        [Test]
        public void Test_encryptBlock()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] toTransform = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            byte[] output = new byte[16];
            encryptor.TransformBlock(toTransform, 0, 16, output, 0);
            byte[] expected = { 242, 144, 0, 182, 42, 73, 159, 208, 169, 243, 154, 106, 221, 46, 119, 128, };
            CheckArraysEqual(output, expected);
        }
        [Test]
        public void Test_encryptLargeBlock()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] toTransform = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, };
            byte[] output = new byte[48];
            encryptor.TransformBlock(toTransform, 0, 48, output, 0);
            byte[] expected = { 242, 144, 0, 182, 42, 73, 159, 208, 169, 243, 154, 106, 221, 46, 119, 128, 149, 67, 184, 111, 192, 70, 250, 136, 58, 148, 70, 184, 46, 71, 209, 45, 161, 68, 252, 37, 90, 173, 69, 191, 104, 29, 58, 55, 115, 163, 37, 194, };
            CheckArraysEqual(output, expected);
        }
        [Test]
        public void Test_encryptInvalidSmallSize()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] toTransform = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            try
            {
                encryptor.TransformBlock(toTransform, 0, toTransform.Length, toTransform, 0);
            }
            catch(ArgumentException)
            {
                // exception expected, this is correct behavior
                Assert.Pass();
            }
            Assert.Fail();
        }
        [Test]
        public void Test_encryptInvalidLargeSize()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] toTransform = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19};
            try
            {
                encryptor.TransformBlock(toTransform, 0, toTransform.Length, toTransform, 0);
            }
            catch(ArgumentException)
            {
                // exception expected, this is correct behavior
                Assert.Pass();
            }
            Assert.Fail();
        }
        [Test]
        public void Test_encryptorInputBlockSize()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform encryptor = aes.CreateEncryptor();
            Assert.AreEqual(encryptor.InputBlockSize, blocksize);
        }
        [Test]
        public void Test_encryptorOutputBlockSize()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform encryptor = aes.CreateEncryptor();
            Assert.AreEqual(encryptor.OutputBlockSize, blocksize);
        }
        [Test]
        public void Test_encryptorCanReuseTransform()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform encryptor = aes.CreateEncryptor();
            Assert.IsTrue(encryptor.CanReuseTransform);
        }
        [Test]
        public void Test_encryptorCanTransformMultipleBlocks()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform encryptor = aes.CreateEncryptor();
            Assert.IsTrue(encryptor.CanTransformMultipleBlocks);
        }
        [Test]
        public void Test_encryptWith24ByteKey()
        {
            byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
            AES.AES aes = new AES.AES(key, IV);
            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] toTransform = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            byte[] output = new byte[16];
            encryptor.TransformBlock(toTransform, 0, 16, output, 0);
            byte[] expected = { 145, 98, 81, 130, 28, 115, 165, 34, 195, 150, 214, 39, 56, 1, 150, 7, };
            CheckArraysEqual(output, expected);
        }
        [Test]
        public void Test_encryptWith16ByteKey()
        {
            byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15};
            AES.AES aes = new AES.AES(key, IV);
            ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] toTransform = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            byte[] output = new byte[16];
            encryptor.TransformBlock(toTransform, 0, 16, output, 0);
            byte[] expected = { 198, 161, 59, 55, 135, 143, 91, 130, 111, 79, 129, 98, 161, 200, 216, 121, };
            CheckArraysEqual(output, expected);
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
