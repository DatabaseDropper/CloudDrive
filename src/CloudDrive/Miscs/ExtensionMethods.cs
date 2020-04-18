using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudDrive.Miscs
{
    public static class ExtensionMethods
    {
        public static int MBtoKB(this int input) => input * 1024 * 1024;
        public static long MBtoKB(this long input) => input * 1024 * 1024;
    }
}
