using System;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Cryptography
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
            this.rng = RandomNumberGenerator.Create();
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
            return new AESDecryptor(rgbKey, rgbIV);
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
            byte[] iv = new byte[ValidIVSize];
            rng.GetBytes(iv);
            this._iv = iv;
        }

        public void GenerateKey()
        {
            byte[] key = new byte[MaximumKeySize];
            rng.GetBytes(key);
            this._key = key;
        }

        public void Dispose()
        {
            if (rng != null)
            {
                rng.Dispose();
            }
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
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
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
            if (iv == null)
            {
                throw new ArgumentNullException("iv");
            }
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
