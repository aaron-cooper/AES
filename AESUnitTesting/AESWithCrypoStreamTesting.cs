using System;
using System.IO;
using System.Security.Cryptography;
using NUnit.Framework;
using Cryptography;

namespace AESUnitTesting
{
    class AESWithCrypoStreamTesting
    {
        [Test]
        public void Test_cryptoStream()
        {
            MemoryStream memStream = new MemoryStream();
            AES aes = new AES();
            byte[] bytes = new byte[1024];

            Random rand = new Random();
            
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)rand.Next(0, 255);
            }

            ICryptoTransform encryptor = aes.CreateEncryptor();
            CryptoStream encryptStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write);
            encryptStream.Write(bytes, 0, bytes.Length);

            memStream.Seek(0, SeekOrigin.Begin);

            ICryptoTransform decryptor = aes.CreateDecryptor();
            CryptoStream decryptStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read);

            byte[] outBuffer = new byte[1024];
            int numRead = decryptStream.Read(outBuffer, 0, 1024);
            if (numRead != 1024)
            {
                throw new Exception("Failed to read all bytes from memory stream");
            }

            TestUtilities.CheckArraysEqual(bytes, outBuffer);
        }
        [Test]
        public void Test_cryptoStreamWithTransformFinal()
        {
            MemoryStream memStream = new MemoryStream();
            AES aes = new AES();
            byte[] bytes = new byte[1020];

            Random rand = new Random();

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)rand.Next(0, 255);
            }

            ICryptoTransform encryptor = aes.CreateEncryptor();
            CryptoStream encryptStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write);
            encryptStream.Write(bytes, 0, bytes.Length);
            encryptStream.FlushFinalBlock();

            memStream.Seek(0, SeekOrigin.Begin);

            ICryptoTransform decryptor = aes.CreateDecryptor();
            CryptoStream decryptStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read);

            byte[] outBuffer = new byte[1020];
            int numRead = decryptStream.Read(outBuffer, 0, 1020);
            if (numRead != 1020)
            {
                throw new Exception("Failed to read all bytes from memory stream");
            }

            TestUtilities.CheckArraysEqual(bytes, outBuffer);
        }
    }
}
