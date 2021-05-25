using System;
using System.Security.Cryptography;

namespace AES
{
    public class AESEncryptor : ICryptoTransform
    {
        private byte[] roundKey;
        private int numberOfRounds;
        private byte[] iv;
        private byte[] prevCiphertext = new byte[16];

        Action<byte[], int> cbcApplier;
        public bool CanReuseTransform => true;

        public bool CanTransformMultipleBlocks => true;

        public int InputBlockSize => AES.ValidBlockSize;

        public int OutputBlockSize => AES.ValidBlockSize;

        public AESEncryptor(byte[] key, byte[] iv)
        {
            AES.ThrowIfKeyInvalid(key);
            AES.ThrowIfIvInvalid(iv);

            this.roundKey = KeySchedule.GenerateSchedule(key);
            this.iv = iv;
            this.numberOfRounds = roundKey.Length / 16 - 1;

            cbcApplier = InitialCBC;
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            // if invalid input size
            if (inputCount <= 0 || (inputCount & 0xf) != 0)
            {
                throw new ArgumentException("inputCount must be multiple of block size", "inputCount");
            }
            Array.Copy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
            for (int i = 0; i < (inputCount >> 4); i++)
            {
                cbcApplier(outputBuffer, outputOffset);
                Cipher(ref outputBuffer, outputOffset);
                outputOffset += 16;
            }
            Array.Copy(outputBuffer, outputOffset - 16, iv, 0, 16);
            cbcApplier = InitialCBC;
            return inputCount;
        }
        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (inputCount <= 0)
            {
                throw new ArgumentException("inputCount must be more than 0");
            }

            // round up to nearest multiple of 16. If we're already
            // at a multiple of 16, round up anyway (we'll always
            // need at least 1 extra byte of room)
            int countWithPadding = ((inputCount >> 4) + 1) << 4;
            byte[] outputBuffer = new byte[countWithPadding];

            Array.Copy(inputBuffer, inputOffset, outputBuffer, 0, inputCount);
            for (int i = inputCount; i < countWithPadding; i++)
            {
                outputBuffer[i] = (byte)(countWithPadding - inputCount);
            }
            TransformBlock(outputBuffer, 0, countWithPadding, outputBuffer, 0);
            return outputBuffer;
        }
        public void InitialCBC(byte[] buffer, int bufferOffset)
        {
            int i = bufferOffset;
            int j = 0;
            for (; i < bufferOffset + 16; i++, j++)
            {
                buffer[i] = (byte)(buffer[i] ^ iv[j]);
            }
            cbcApplier = LastBlockCBC;
        }
        public void LastBlockCBC(byte[] buffer, int bufferOffset)
        {
            int i = bufferOffset;
            int j = bufferOffset - 16;
            for (; i < bufferOffset + 16; i++, j++)
            {
                buffer[i] = (byte)(buffer[i] ^ buffer[j]);
            }
        }


        public void Cipher(ref byte[] input, int offset)
        {
            if (input.Length - offset < 16)
            {
                throw new ArgumentException("input length must be at least offset + 16", "input");
            }

            AddRoundKey(ref input, offset, 0);

            int round = 1;
            for (; round < numberOfRounds; round++)
            {
                SubBytes(ref input, offset);
                ShiftRows(ref input, offset);
                MixColumns(ref input, offset);
                AddRoundKey(ref input, offset, round * 16);
            }
            SubBytes(ref input, offset);
            ShiftRows(ref input, offset);
            AddRoundKey(ref input, offset, round * 16);
        }
        private void AddRoundKey(ref byte[] state, int stateOff, int roundKeyOff)
        {
            for (int i = 0; i < 16; i++)
            {
                state[stateOff + i] = (byte)(state[stateOff + i] ^ roundKey[roundKeyOff + i]);

            }
        }
        private void SubBytes(ref byte[] state, int off)
        {
            for (int i = 0; i < 16; i++)
            {
                state[i + off] = RijndaelSubBox.Sub(state[i + off]);
            }
        }
        private void ShiftRows(ref byte[] state, int off)
        {
            // row 2
            int i = 1 + off;
            byte temp = state[i];
            for (; i < off + 13; i += 4)
            {
                state[i] = state[i + 4];
            }
            state[i] = temp;

            // row 3
            i = off + 2;
            temp = state[i];
            state[i] = state[i + 8];
            state[i + 8] = temp;
            i = off + 6;
            temp = state[i];
            state[i] = state[i + 8];
            state[i + 8] = temp;

            // row 4
            i = 15 + off;
            temp = state[i];
            for (; i > off + 3; i -= 4)
            {
                state[i] = state[i - 4];
            }
            state[i] = temp;
        }
        private void MixColumns(ref byte[] state, int off)
        {
            int i0 = 0 + off;
            int i1 = 1 + off;
            int i2 = 2 + off;
            int i3 = 3 + off;


            for (int i = 0; i < 4; i++)
            {
                byte b0 = state[i0];
                byte b1 = state[i1];
                byte b2 = state[i2];
                byte b3 = state[i3];

                state[i0] = (byte)(GField.Mult(2, b0) ^ GField.Mult(3, b1) ^ b2 ^ b3);
                state[i1] = (byte)(b0 ^ GField.Mult(2, b1) ^ GField.Mult(3, b2) ^ b3);
                state[i2] = (byte)(b0 ^ b1 ^ GField.Mult(2, b2) ^ GField.Mult(3, b3));
                state[i3] = (byte)(GField.Mult(3, b0) ^ b1 ^ b2 ^ GField.Mult(2, b3));

                i0 += 4;
                i1 += 4;
                i2 += 4;
                i3 += 4;
            }
        }
    }
}
