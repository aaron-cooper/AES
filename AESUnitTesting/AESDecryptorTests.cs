using NUnit.Framework;
using System.Security.Cryptography;
using AES;
using System;

namespace AESUnitTesting
{
    class AESDecryptorTests
    {
        const int blocksize = 16;
        private byte[] Key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        private byte[] IV = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

        [Test]
        public void Test_CreateDecryptor()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();
        }

        [Test]
        public void Test_decryptSmallFinalBlock()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] toTransform = { 140, 58, 160, 75, 146, 104, 11, 200, 169, 67, 22, 127, 121, 91, 25, 17 };
            byte[] expected = { 0, 1, 2, 3, 4, 5, 6, 7 };

            byte[] after = decryptor.TransformFinalBlock(toTransform, 0, toTransform.Length);

            TestUtilities.CheckArraysEqual(after, expected);
        }
        [Test]
        public void Test_decryptSmallFinalBlockAfterAnotherBlock()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();

            byte[] first = { 242, 144, 0, 182, 42, 73, 159, 208, 169, 243, 154, 106, 221, 46, 119, 128, };
            byte[] final = { 25, 5, 33, 39, 94, 197, 236, 127, 71, 122, 213, 35, 220, 108, 81, 8, };

            byte[] firstOut = new byte[16];
            byte[] finalout;
            decryptor.TransformBlock(first, 0, 16, firstOut, 0);
            finalout = decryptor.TransformFinalBlock(final, 0, 16);

            byte[] firstExpected = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            byte[] finalExpected = { 0, 1, 2, 3, 4, 5, 6, 7 };

            TestUtilities.CheckArraysEqual(firstExpected, firstOut);
            TestUtilities.CheckArraysEqual(finalExpected, finalout);
        }
        [Test]
        public void Test_decryptLargeFinalBlock()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();

            byte[] toTransform = { 242, 144, 0, 182, 42, 73, 159, 208, 169, 243, 154, 106, 221, 46, 119, 128, 149, 67, 184, 111, 192, 70, 250, 136, 58, 148, 70, 184, 46, 71, 209, 45, 245, 6, 211, 246, 165, 133, 51, 129, 86, 215, 23, 211, 134, 144, 25, 77, };
            byte[] expected = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, };

            byte[] output = decryptor.TransformFinalBlock(toTransform, 0, toTransform.Length);

            TestUtilities.CheckArraysEqual(expected, output);
        }
        [Test]
        public void Test_decryptBlock()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] toTransform = { 242, 144, 0, 182, 42, 73, 159, 208, 169, 243, 154, 106, 221, 46, 119, 128, };
            byte[] expected = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            byte[] output = new byte[16];
            decryptor.TransformBlock(toTransform, 0, 16, output, 0);
            TestUtilities.CheckArraysEqual(output, expected);
        }
        [Test]
        public void Test_decryptLargeBlock()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] toTransform = { 242, 144, 0, 182, 42, 73, 159, 208, 169, 243, 154, 106, 221, 46, 119, 128, 149, 67, 184, 111, 192, 70, 250, 136, 58, 148, 70, 184, 46, 71, 209, 45, 161, 68, 252, 37, 90, 173, 69, 191, 104, 29, 58, 55, 115, 163, 37, 194, };
            byte[] expected = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, };
            byte[] output = new byte[48];
            decryptor.TransformBlock(toTransform, 0, 48, output, 0);
            TestUtilities.CheckArraysEqual(output, expected);
        }
        [Test]
        public void Test_decryptInvalidSmallSize()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] toTransform = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            try
            {
                decryptor.TransformBlock(toTransform, 0, toTransform.Length, toTransform, 0);
            }
            catch (ArgumentException)
            {
                // exception expected, this is correct behavior
                Assert.Pass();
            }
            Assert.Fail();
        }
        [Test]
        public void Test_decryptInvalidLargeSize()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] toTransform = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
            try
            {
                decryptor.TransformBlock(toTransform, 0, toTransform.Length, toTransform, 0);
            }
            catch (ArgumentException)
            {
                // exception expected, this is correct behavior
                Assert.Pass();
            }
            Assert.Fail();
        }
        [Test]
        public void Test_decryptorInputBlockSize()
        {
            AES.AES aes = new AES.AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();
            Assert.AreEqual(decryptor.InputBlockSize, blocksize);
        }
    }
}