using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPhaseSolver
{
    static class Tools
    {
        public static int factorial(int n)
        {
            int k = 1;
            for (int i = 2; i < n; i++)
            {
                k *= i;
            }
            return k * n;
        }

        public static int nChooseK(int n, int k)
        {
            int i, j, s;
            if (n < k)
                return 0;
            if (k > n / 2)
                k = n - k;
            for (s = 1, i = n, j = 1; i != n - k; i--, j++)
            {
                s *= i;
                s /= j;
            }
            return s;
        }

        public static T[] rotate<T>(this IEnumerable<T> me, int amount)
        {
            var newarr = new T[me.Count()];
            int i = 0;

            foreach (T item in me)
            {
                newarr[(i + amount) % me.Count()] = item;
                i++;
            }

            return newarr;
        }

        public static int Index<T>(this T[] me, T value, IEqualityComparer<T> comparer)
        {
            for (int i = 0; i < me.Length; i++)
            {
                if (comparer.Equals(value, me[i])) { return i; }
            }

            return -1;
        }

        public static int Index<T>(this T[] me, T value, Func<T, T, bool> comparer)
        {
            for (int i = 0; i < me.Length; i++)
            {
                if (comparer(value, me[i])) { return i; }
            }

            return -1;
        }

        public static int Index<T>(this T[] me, T value) where T : IEquatable<T>
        {
            for (int i = 0; i < me.Length; i++)
            {
                if (value.Equals(me[i])) { return i; }
            }

            return -1;
        }

        public static bool setEquals<T>(this T[] a, T[] b)
        {
            var min = (a.Length >= b.Length) ? b : a;
            var max = (a.Length >= b.Length) ? a : b;

            foreach (T item in max)
            {
                if (!min.Contains(item)) { return false; }
            }

            return true;
        }

        static void ColorProcedure(object obj, Action<object> printFunc, ConsoleColor fg, ConsoleColor bg)
        {
            ConsoleColor previousFg = Console.ForegroundColor;
            ConsoleColor previousBg = Console.BackgroundColor;

            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;

            printFunc(obj);

            Console.ForegroundColor = previousFg;
            Console.BackgroundColor = previousBg;
        }

        public static void ColorPrint(object obj, ConsoleColor fg, ConsoleColor bg = ConsoleColor.Black)
        {
            ColorProcedure(obj, Console.Write, fg, bg);
        }

        public static void ColorPrintLine(object obj, ConsoleColor fg, ConsoleColor bg = ConsoleColor.Black)
        {
            ColorProcedure(obj, Console.WriteLine, fg, bg);
        }
    }
}
