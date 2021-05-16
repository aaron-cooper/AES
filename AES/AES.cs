using System;
using System.Security.Cryptography;

namespace AES
{
    public class AES : IDisposable
    {
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }

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
            throw new NotImplementedException();
        }

        public void GenerateKey()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
