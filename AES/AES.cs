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


        private byte[] _key;
        public byte[] Key
        {
            get
            {
                return _key;
            }
            set
            {
                ThrowIfKeyInvalid(value);
                _key = new byte[value.Length];
                Array.Copy(value, _key, value.Length);
            }
        }
        public int KeySize
        {
            get => Key.Length;
        }

        private byte[] _iv;
        public byte[] IV
        {
            get
            {
                return _iv;
            }
            set
            {
                ThrowIfIvInvalid(value);
                _iv = new byte[value.Length];
                Array.Copy(value, _iv, value.Length);
            }
        }
        public int IVSize
        {
            get => IV.Length;
        }
        public int BlockSize
        {
            get => 16;
        }


        private RandomNumberGenerator rng;

        public AES()
        {
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            this.GenerateKey();
            this.GenerateIV();
        }

        public AES(byte[] key, byte[] iv)
        {
            this.Key = key;
            this.IV = iv;
        }

        public ICryptoTransform CreateDecryptor()
        {
            return CreateDecryptor(this.Key, this.IV);
        }
        public ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new NotImplementedException();
        }

        public ICryptoTransform CreateEncryptor()
        {
            return CreateEncryptor(this.Key, this.IV);
        }
        public ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new AESEncryptor(rgbKey, rgbIV);
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
