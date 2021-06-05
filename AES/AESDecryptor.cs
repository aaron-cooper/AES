using System;
using System.Security.Cryptography;

namespace AES
{
    class AESDecryptor : ICryptoTransform
    {
        private byte[] roundKey;
        private byte[] iv;
        private int numberOfRounds;
        public bool CanReuseTransform => throw new NotImplementedException();

        public bool CanTransformMultipleBlocks => throw new NotImplementedException();

        public int InputBlockSize => throw new NotImplementedException();

        public int OutputBlockSize => throw new NotImplementedException();

        public AESDecryptor(byte[] key, byte[] iv)
        {
            AES.ThrowIfKeyInvalid(key);
            AES.ThrowIfIvInvalid(iv);

            this.roundKey = KeySchedule.GenerateSchedule(key);
            this.iv = new byte[iv.Length];
            Array.Copy(iv, this.iv, iv.Length);
            this.numberOfRounds = roundKey.Length - 16;
        }

        public void Dispose()
        {
            // left intentionally blank
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            throw new NotImplementedException();
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            throw new NotImplementedException();
        }

        public void Decipher(ref byte[] input, int offset)
        {
            if (input.Length - offset < 16)
            {
                throw new ArgumentException("input length must be at least offset + 16", "input");
            }

            AddRoundKey(ref input, offset, numberOfRounds);

            int round = numberOfRounds - 16;
            for (; round > 0; round -= 16)
            {
                InvShiftRows(ref input, offset);
                InvSubBytes(ref input, offset);
                AddRoundKey(ref input, offset, round);
                InvMixColumns(ref input, offset);
            }
            InvShiftRows(ref input, offset);
            InvSubBytes(ref input, offset);
            AddRoundKey(ref input, offset, round);
        }

        private void AddRoundKey(ref byte[] state, int stateOff, int roundKeyOff)
        {
            for (int i = 0; i < 16; i++)
            {
                state[stateOff + i] = (byte)(state[stateOff + i] ^ roundKey[roundKeyOff + i]);

            }
        }
        private void InvShiftRows(ref byte[] state, int off)
        {
            // row 2
            int i = 13 + off;
            byte temp = state[i];
            for (; i > off + 3; i -= 4)
            {
                state[i] = state[i - 4];
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
            i = 3 + off;
            temp = state[i];
            for (; i < off + 15; i += 4)
            {
                state[i] = state[i + 4];
            }
            state[i] = temp;
        }
        private void InvSubBytes(ref byte[] state, int off)
        {
            for (int i = 0; i < 16; i++)
            {
                state[i + off] = RijndaelSubBox.SubInverse(state[i + off]);
            }
        }
        private void InvMixColumns(ref byte[] state, int off)
        {
            int i0 = off++;
            int i1 = off++;
            int i2 = off++;
            int i3 = off++;


            for (int i = 0; i < 4; i++)
            {
                byte b0 = state[i0];
                byte b1 = state[i1];
                byte b2 = state[i2];
                byte b3 = state[i3];

                state[i0] = (byte)(GField.Mult(0x0e, b0) ^ GField.Mult(0x0b, b1) ^ GField.Mult(0x0d, b2) ^ GField.Mult(0x09, b3));
                state[i1] = (byte)(GField.Mult(0x09, b0) ^ GField.Mult(0x0e, b1) ^ GField.Mult(0x0b, b2) ^ GField.Mult(0x0d, b3));
                state[i2] = (byte)(GField.Mult(0x0d, b0) ^ GField.Mult(0x09, b1) ^ GField.Mult(0x0e, b2) ^ GField.Mult(0x0b, b3));
                state[i3] = (byte)(GField.Mult(0x0b, b0) ^ GField.Mult(0x0d, b1) ^ GField.Mult(0x09, b2) ^ GField.Mult(0x0e, b3));

                i0 += 4;
                i1 += 4;
                i2 += 4;
                i3 += 4;
            }
        }
    }
}
