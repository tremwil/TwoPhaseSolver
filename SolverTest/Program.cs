using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwoPhaseSolver;

namespace SolverTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Just solve a random cube with some pattern.
            Cube c = Move.randmove(200).apply(new Cube());
            Move pattern;

            // BEST RANDOM GEN. EVER. (actually not that bad,
            // since you'd have to time yourself with 100nanosecond precision
            if ((DateTime.Now.Ticks & 1) == 0)
            {
                pattern = Move.None;
                Console.WriteLine("No pattern this time...");
            }
            else
            {
                pattern = Move.randmove(20);
                Console.WriteLine("Pattern is {0}", pattern);
            }

            // Do the actual solve while printing what is happening
            Search.patternSolve(c, pattern, 22, printInfo: true);

            // End
            Console.Write("Press any key to continue...");
            Console.Read();
        }
    }
}
