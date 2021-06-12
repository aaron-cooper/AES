using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AES
{
    internal class GField
    {
        /// <summary>
        /// Multiply two values in GF(2^8)
        /// </summary>
        internal static byte Mult(byte a, byte b)
        {
            // peasant multiplication, reduce as described in fips 197
            byte product = 0;
            while(a > 0)
            {
                if ((a & 1) != 0)
                {
                    product ^= b;
                }

                // fips 197 says to reduce if bit 7 is set
                // bit 7 will carry out so test it before
                bool shouldReduce = (b & 0x80) != 0;
                b <<= 1;
                if (shouldReduce)
                {
                    b ^= 0x1b;
                }
                a >>= 1;
            }
            return product;
        }
    }
}
