using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    public class Assert
    {
        public static bool Equals(string expected, string actual, string errMsg)
        {
            if (expected == actual)
            {
                Assert.printOk();
                return true;
            }
            else
            {
                Assert.printErr(expected, actual, errMsg);
                return false;
            }    
        }

        public static bool Equals(int expected, int actual, string errMsg)
        {
            if (expected == actual)
            {
                Assert.printOk();
                return true;
            }
            else
            {
                Assert.printErr(expected.ToString(), actual.ToString(), errMsg);
                return false;
            }
        }

        private static void printOk()
        {
            Console.WriteLine("=> OK");
        }

        private static void printErr(string expected, string actual, string msg)
        {
            Console.WriteLine("** FAILED EXPECTED: " + expected + ", ACTUAL:"  + actual + " -- " + msg);
        }

    }
}
