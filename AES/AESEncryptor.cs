using System;
using System.Security.Cryptography;

namespace AES
{
    class AESEncryptor : ICryptoTransform
    {
        private byte[] roundKey;
        private byte[] iv;
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
        }

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
        public void Cipher(ref byte[] input, int offset, byte[] key)
        {
            if (input.Length - offset < 16)
            {
                throw new ArgumentException("input length must be at least offset + 16", "input");
            }
            byte[] keySchedule = KeySchedule.GenerateSchedule(key);
            int nr = keySchedule.Length / 16 - 1;

            AddRoundKey(ref input, offset, ref keySchedule, 0);

            int round = 1;
            for (; round < nr; round++)
            {
                SubBytes(ref input, offset);
                ShiftRows(ref input, offset);
                MixColumns(ref input, offset);
                AddRoundKey(ref input, offset, ref keySchedule, round * 16);
            }
            SubBytes(ref input, offset);
            ShiftRows(ref input, offset);
            AddRoundKey(ref input, offset, ref keySchedule, round * 16);
        }
        private static void AddRoundKey(ref byte[] state, int stateOff, ref byte[] roundKey, int roundKeyOff)
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
