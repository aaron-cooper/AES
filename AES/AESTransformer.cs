using System;

namespace AES
{
    public class AESTransformer
    {

        public static void Cipher(ref byte[] input, int offset, byte[] key)
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

        public static void Decipher(ref byte[] input, int offset, byte[] key)
        {
            byte[] keySchedule = KeySchedule.GenerateSchedule(key);
            int nr = keySchedule.Length / 16 - 1;

            AddRoundKey(ref input, offset, ref keySchedule, nr * 16);

            for (int round = nr - 1; round > 0; round--)
            {
                InvShiftRows(ref input, offset);
                InvSubBytes(ref input, offset);
                AddRoundKey(ref input, offset, ref keySchedule, round * 16);
                InvMixColumns(ref input, offset);
            }
            InvShiftRows(ref input, offset);
            InvSubBytes(ref input, offset);
            AddRoundKey(ref input, offset, ref keySchedule, 0);

        }

        private static void AddRoundKey(ref byte[] state, int stateOff, ref byte[] roundKey, int roundKeyOff)
        {
            for (int i = 0; i < 16; i++)
            {
                state[stateOff + i] = (byte)(state[stateOff + i] ^ roundKey[roundKeyOff + i]);
            }
        }
        private static void SubBytes(ref byte[] state, int off)
        {
            for (int i = 0; i < 16; i++)
            {
                state[i + off] = RijndaelSubBox.Sub(state[i + off]);
            }
        }
        private static void ShiftRows(ref byte[] state, int off)
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

        private static void MixColumns(ref byte[] state, int off)
        {
            int i0 = 0;
            int i1 = 1;
            int i2 = 2;
            int i3 = 3;


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
        private static void InvShiftRows(ref byte[] state, int off)
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
        private static void InvSubBytes(ref byte[] state, int off)
        {
            for (int i = 0; i < 16; i++)
            {
                state[i + off] = RijndaelSubBox.SubInverse(state[i + off]);
            }
        }
        private static void InvMixColumns(ref byte[] state, int off)
        {
            int i0 = 0;
            int i1 = 1;
            int i2 = 2;
            int i3 = 3;


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
