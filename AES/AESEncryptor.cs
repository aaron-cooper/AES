using System.Security.Cryptography;

namespace AES
{
    class AESEncryptor : ICryptoTransform
    {
        public bool CanReuseTransform => true;

        public bool CanTransformMultipleBlocks => true;

        public int InputBlockSize => AES.ValidBlockSize;

        public int OutputBlockSize => AES.ValidBlockSize;

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            throw new System.NotImplementedException();
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            throw new System.NotImplementedException();
        }
    }
}
