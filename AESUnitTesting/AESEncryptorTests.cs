﻿using NUnit.Framework;
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
