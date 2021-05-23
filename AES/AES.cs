using System;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace AES
{
    public class AES : IDisposable
    {
        private static SortedSet<int> _validKeySizes = new SortedSet<int>
        {
            16,
            24,
            32
        };
        public static int ValidBlockSize
        {
            get => 16;
        }
        public static int ValidIVSize
        {
            get => 16;
        }
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

        public static List<int> ValidKeySizes { 
            get
            {
                return new List<int>(_validKeySizes);
            }
        }
        public static int MaximumKeySize
        {
            get
            {
                return _validKeySizes.Max;
            }
        }
        public static int MinimumKeySize
        {
            get
            {
                return _validKeySizes.Min;
            }
        }
        public static void ThrowIfKeyInvalid(byte[] key)
        {
            if (!IsKeyValid(key))
            {
                throw new ArgumentException("Key must be 16, 24, or 32 bytes in length");
            }
        }

        private static bool IsKeyValid(byte[] key)
        {
            return _validKeySizes.Contains(key.Length);
        }
        public static void ThrowIfIvInvalid(byte[] iv)
        {
            if (!IsValidIV(iv))
            {
                throw new ArgumentException("IV must be 16 bytes in length");
            }
        }
        private static bool IsValidIV(byte[] iv)
        {
            return iv.Length == ValidIVSize;
        }
    }
}
