using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddIn1.Utils
{
    public static class DoubleUtils
    {
        public static double FeetToMet(this double feet) => feet * 0.304800;
        public static double FeetToMet(this int feet) => feet * 0.304800;


        public static double MetToFeet(this double met) => met * 3.280840;
        public static double MetToFeet(this int met) => met * 3.280840;

        public static double MmToFeet(this double mm) => mm * 3.280840 / 1000.0;
        public static double MmToFeet(this int mm) => mm * 3.280840 / 1000.0;
    }
}
