﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES
{
    internal class KeySchedule
    {
        private static UInt32[] Rcon =
        {
            0x00000000,
            0x01000000,
            0x02000000,
            0x04000000,
            0x08000000,
            0x10000000,
            0x20000000,
            0x40000000,
            0x80000000,
            0x1B000000,
            0x36000000,
            0x6C000000,
            0xD8000000,
            0xAB000000,
            0x4D000000,
            0x9A000000
        };
        internal static UInt32[] GenerateSchedule(byte[] key)
        {
            int nk = key.Length / 4;
            switch(nk)
            {
                case 4:
                case 6:
                case 8:
                    break;
                default:
                    throw new ArgumentException("key must be 128, 192, or 256 bits long");
            }
            int nr = nk + 6;
            UInt32[] w = new UInt32[4 * (nr + 1)];

            for (int i = 0; i < nk; i++)
            {
                w[i] = word(key, i * 4);
            }
            for(int i = nk; i < 4 * (nr + 1); i++)
            {
                UInt32 temp = w[i - 1];
                if ((i % nk) == 0)
                {
                    temp = SubWord(Rotate(temp)) ^ Rcon[i / nk];
                }
                else if (nk > 6 && i % nk == 4)
                {
                    temp = SubWord(temp);
                }
                w[i] = w[i - nk] ^ temp;
            }
            return w;
        }

        private static UInt32 word(in byte[] key, int offset)
        {
            UInt32 i;
            i = (UInt32)key[offset] << 24 | (UInt32)key[offset + 1] << 16 | (UInt32)key[offset + 2] << 8 | (UInt32)key[offset + 3];
            return i;

        }
        private static UInt32 Rotate(UInt32 word)
        {
            UInt32 lmb = (word & 0xFF000000) >> 24;
            word <<= 8;
            return word | lmb;
        }
        private static UInt32 SubWord(UInt32 word)
        {
            UInt32 res;
            res = 0;
            res |= (UInt32)RijndaelSubBox.Sub((byte)word);
            res |= (UInt32)RijndaelSubBox.Sub((byte)(word >> 8)) << 8;
            res |= (UInt32)RijndaelSubBox.Sub((byte)(word >> 16)) << 16;
            res |= (UInt32)RijndaelSubBox.Sub((byte)(word >> 24)) << 24;
            return res;
        }
    }
}
