using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPhaseSolver
{
    public partial class Cube
    {
        // Combinations

        public ushort UDSliceCoord()
        {
            int k = -1, i = 0, s = 0;

            foreach (Cubie e in edges)
            {
                if (UDSlice.Contains(e.pos)) { k += 1; }
                else if (k != -1) { s += Tools.nChooseK(i, k); }

                i += 1;
            }

            return (ushort)s;
        }

        public ushort USliceCoord()
        {
            int k = -1, i = 0, s = 0;

            foreach (Cubie e in edges.Reverse())
            {
                if (USlice.Contains(e.pos)) { k += 1; }
                else if (k != -1) { s += Tools.nChooseK(i, k); }

                i += 1;
            }

            return (ushort)s;
        }

        public ushort DSliceCoord()
        {
            int k = -1, i = 0, s = 0;

            foreach (Cubie e in edges.rotate(4))
            {
                if (DSlice.Contains(e.pos)) { k += 1; }
                else if (k != -1) { s += Tools.nChooseK(i, k); }

                i += 1;
            }

            return (ushort)s;
        }

        // Orients

        public ushort cornOrientCoord()
        {
            int i, s = 0;
            for (i = 0; i < 7; i++)
            {
                s = 3 * s + corners[i].orient;
            }

            return (ushort)s;
        }

        public ushort edgeOrientCoord()
        {
            int i, s = 0;
            for (i = 0; i < 11; i++)
            {
                s = 2 * s + edges[i].orient;
            }

            return (ushort)s;
        }

        // Permutations

        public ushort cornPermCoord()
        {
            int i, j, s, x = 0;

            for (i = 7; i > 0; i--)
            {
                s = 0;
                for (j = i; j >= 0; j--)
                {
                    if (corners[j].pos > corners[i].pos) { s++; }
                }

                x = (x + s) * i;
            }

            return (ushort)x;
        }

        public int edgePermCoord()
        {
            int i, j, s, x = 0;

            for (i = 11; i > 0; i--)
            {
                s = 0;
                for (j = i; j >= 0; j--)
                {
                    if (edges[j].pos > edges[i].pos) { s++; }
                }

                x = (x + s) * i;
            }

            return x;
        }

        public ushort edgePermCoord2()
        {
            int i, j, s, x = 0;

            for (i = 7; i > 0; i--)
            {
                s = 0;
                for (j = i; j >= 0; j--)
                {
                    if (edges[j].pos > edges[i].pos) { s++; }
                }

                x = (x + s) * i;
            }

            return (ushort)x;
        }

        // Sorted combinations

        public ushort UDSliceCoordS()
        {
            List<byte> arr = new List<byte>(4);
            int i, j, s, x = 0;

            foreach (Cubie e in edges)
            {
                if (UDSlice.Contains(e.pos)) { arr.Add(e.pos); }
            }

            for (i = 3; i > 0; i--)
            {
                s = 0;
                for (j = i; j >= 0; j--)
                {
                    if (arr[j] > arr[i]) { s++; }
                }

                x = (x + s) * i;
            }

            return (ushort)(UDSliceCoord() * 24 + x);
        }

        public ushort USliceCoordS()
        {
            List<byte> arr = new List<byte>(4);
            int i, j, s, x = 0;

            foreach (Cubie e in edges)
            {
                if (USlice.Contains(e.pos)) { arr.Add(e.pos); }
            }

            for (i = 3; i > 0; i--)
            {
                s = 0;
                for (j = i; j >= 0; j--)
                {
                    if (arr[j] > arr[i]) { s++; }
                }

                x = (x + s) * i;
            }

            return (ushort)(USliceCoord() * 24 + x);
        }

        public ushort DSliceCoordS()
        {
            List<byte> arr = new List<byte>(4);
            int i, j, s, x = 0;

            foreach (Cubie e in edges)
            {
                if (DSlice.Contains(e.pos)) { arr.Add(e.pos); }
            }

            for (i = 3; i > 0; i--)
            {
                s = 0;
                for (j = i; j >= 0; j--)
                {
                    if (arr[j] > arr[i]) { s++; }
                }

                x = (x + s) * i;
            }

            return (ushort)(DSliceCoord() * 24 + x);
        }
    }

    public static class MoveTables
    {
        // Move tables, max size 8! * 18 = 725760

        public static readonly ushort[,] moveCO = BinLoad.loadShortTable2D("tables\\move\\co_move");
        public static readonly ushort[,] moveEO = BinLoad.loadShortTable2D("tables\\move\\eo_move");
        public static readonly ushort[,] moveUD = BinLoad.loadShortTable2D("tables\\move\\ud_move");

        public static readonly ushort[,] moveCP = BinLoad.loadShortTable2D("tables\\move\\cp_move");
        public static readonly ushort[,] moveEP2 = BinLoad.loadShortTable2D("tables\\move\\ep2_move");
        public static readonly ushort[,] moveUDS = BinLoad.loadShortTable2D("tables\\move\\uds_move");

        public static readonly ushort[,] moveUS = BinLoad.loadShortTable2D("tables\\move\\us_move");
        public static readonly ushort[,] moveDS = BinLoad.loadShortTable2D("tables\\move\\ds_move");

        // Merge table (US-DS to EP2), size 11880 * 24 = 285120

        public static readonly ushort[,] mergeEP2 = BinLoad.loadShortTable2D("tables\\other\\ep2_merge", 24);
    }
}
