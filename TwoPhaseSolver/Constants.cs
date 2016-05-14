using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPhaseSolver
{
    public struct Constants
    {
        public const int N_CO = 2187; // 3^7
        public const int N_EO = 2048; // 2^12
        public const int N_UD = 495;  // 12! / (4! * 8!)

        public const int N_CP = 40320;  // 8!
        public const int N_EP2 = 40320; // 8!
        public const int N_UDS = 11880; // 12! / 8!
        public const int N_UD2 = 24;    // 4!
    }
}
