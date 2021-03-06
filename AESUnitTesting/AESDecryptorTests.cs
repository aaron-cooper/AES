using NUnit.Framework;
using System.Security.Cryptography;
using Cryptography;
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
            AES aes = new AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();
        }

        [Test]
        public void Test_decryptSmallFinalBlock()
        {
            AES aes = new AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] toTransform = { 140, 58, 160, 75, 146, 104, 11, 200, 169, 67, 22, 127, 121, 91, 25, 17 };
            byte[] expected = { 0, 1, 2, 3, 4, 5, 6, 7 };

            byte[] after = decryptor.TransformFinalBlock(toTransform, 0, toTransform.Length);

            TestUtilities.CheckArraysEqual(after, expected);
        }
        [Test]
        public void Test_decryptSmallFinalBlockAfterAnotherBlock()
        {
            AES aes = new AES(Key, IV);
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
            AES aes = new AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();

            byte[] toTransform = { 242, 144, 0, 182, 42, 73, 159, 208, 169, 243, 154, 106, 221, 46, 119, 128, 149, 67, 184, 111, 192, 70, 250, 136, 58, 148, 70, 184, 46, 71, 209, 45, 245, 6, 211, 246, 165, 133, 51, 129, 86, 215, 23, 211, 134, 144, 25, 77, };
            byte[] expected = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, };

            byte[] output = decryptor.TransformFinalBlock(toTransform, 0, toTransform.Length);

            TestUtilities.CheckArraysEqual(expected, output);
        }
        [Test]
        public void Test_decryptBlock()
        {
            AES aes = new AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] toTransform = { 242, 144, 0, 182, 42, 73, 159, 208, 169, 243, 154, 106, 221, 46, 119, 128, };
            byte[] expected = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            byte[] output = new byte[16];
            decryptor.TransformBlock(toTransform, 0, 16, output, 0);
            TestUtilities.CheckArraysEqual(output, expected);
        }
        [Test]
        public void Test_decryptMultipleBlocks()
        {
            AES aes = new AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] toTransform = { 242, 144, 0, 182, 42, 73, 159, 208, 169, 243, 154, 106, 221, 46, 119, 128, 149, 67, 184, 111, 192, 70, 250, 136, 58, 148, 70, 184, 46, 71, 209, 45, 161, 68, 252, 37, 90, 173, 69, 191, 104, 29, 58, 55, 115, 163, 37, 194, };
            byte[] output = new byte[48];
            decryptor.TransformBlock(toTransform, 0, 16, output, 0);
            decryptor.TransformBlock(toTransform, 16, 32, output, 16);
            byte[] expected = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, };
            TestUtilities.CheckArraysEqual(output, expected);
        }
        [Test]
        public void Test_decryptLargeBlock()
        {
            AES aes = new AES(Key, IV);
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
            AES aes = new AES(Key, IV);
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
            AES aes = new AES(Key, IV);
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
            AES aes = new AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();
            Assert.AreEqual(decryptor.InputBlockSize, blocksize);
        }
        [Test]
        public void Test_decryptorOutputBlockSize()
        {
            AES aes = new AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();
            Assert.AreEqual(decryptor.OutputBlockSize, blocksize);
        }
        [Test]
        public void Test_decryptorCanReuseTransform()
        {
            AES aes = new AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();
            Assert.IsTrue(decryptor.CanReuseTransform);
        }
        [Test]
        public void Test_decryptorCanTransformMultipleBlocks()
        {
            AES aes = new AES(Key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();
            Assert.IsTrue(decryptor.CanTransformMultipleBlocks);
        }
        [Test]
        public void Test_decryptWith24ByteKey()
        {
            byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
            AES aes = new AES(key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] toTransform = { 145, 98, 81, 130, 28, 115, 165, 34, 195, 150, 214, 39, 56, 1, 150, 7, };
            byte[] expected = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            byte[] output = new byte[16];
            decryptor.TransformBlock(toTransform, 0, 16, output, 0);
            TestUtilities.CheckArraysEqual(output, expected);
        }
        [Test]
        public void Test_decryptWith16ByteKey()
        {
            byte[] key = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            AES aes = new AES(key, IV);
            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] toTransform = { 198, 161, 59, 55, 135, 143, 91, 130, 111, 79, 129, 98, 161, 200, 216, 121, };
            byte[] expected = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            byte[] output = new byte[16];
            decryptor.TransformBlock(toTransform, 0, 16, output, 0);
            TestUtilities.CheckArraysEqual(output, expected);
        }
    }
}