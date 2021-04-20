using System;
using AES;
using System.Collections.Generic;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            HashSet<byte> set = new HashSet<byte>();
            for (int i= 0; i< 256; i++)
            {
                byte b = (byte)i; 
                byte ab = RijndaelSubBox.Sub(b);
                set.Add(ab);

                if (b != RijndaelSubBox.SubInverse(ab))
                {
                    Console.WriteLine("incorrect inverse");
                }
            }
            if (set.Count != 256)
            {
                Console.WriteLine("missing inverse");
            }
        }
    }
}
