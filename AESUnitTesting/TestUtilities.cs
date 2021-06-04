using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AESUnitTesting
{
    class TestUtilities
    {
        public static void CheckArraysEqual(byte[] arr1, byte[] arr2)
        {
            Assert.AreEqual(arr1.Length, arr2.Length);
            for (int i = 0; i < arr1.Length; i++)
            {
                Assert.AreEqual(arr1[i], arr2[i]);
            }
        }
    }
}
