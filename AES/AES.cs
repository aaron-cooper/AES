using System;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace AES
{
    public class AES : IDisposable
    {
        public const int KeySize = 32;
        public byte[] Key { get; set; }
        public const int IVSize = 16;
        public byte[] IV { get; set; }
        public const int BlockSize = 16;

        private RandomNumberGenerator rng;

        private AES()
        {
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            this.GenerateKey();
            this.GenerateIV();
        }

        public ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new NotImplementedException();
        }

        public ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new NotImplementedException();
        }

        public void GenerateIV()
        {
            IV = new byte[IVSize];
            rng.GetBytes(IV);
        }

        public void GenerateKey()
        {
            Key = new byte[KeySize];
            rng.GetBytes(Key);
        }

        public void Dispose()
        {
            rng.Dispose();
        }
    }
}
