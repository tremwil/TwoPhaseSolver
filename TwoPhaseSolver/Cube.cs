using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPhaseSolver
{
    public struct Cubie
    {
        public byte pos, orient;

        public Cubie(byte pos, byte orient)
        {
            this.pos = pos;
            this.orient = orient;
        }

        public override string ToString()
        {
            return "Cubie(" + pos.ToString() + ", " + orient.ToString() + ")";
        }
    }

    public partial class Cube
    {
        /* Definitions based on this diagram
                      +---------+
                      | 00 - 07 |
                      |    U    |
                      |         |
            +---------+---------+---------+---------+
            | 24 - 31 | 16 - 23 | 08 - 17 | 32 - 39 |
            |    L    |    F    |    R    |    B    |
            |         |         |         |         |
            +---------+---------+---------+---------+
                      | 40 - 47 |
                      |    D    |
                      |         |
                      +---------+
        */

        private static byte[][] udToPerm = BinLoad.getUdToPerm("tables\\other\\combi");

        private static byte[] Facelets = new byte[48]
        {
            00, 01, 02, 03, 04, 05, 06, 07,
            08, 09, 10, 11, 12, 13, 14, 15,
            16, 17, 18, 19, 20, 21, 22, 23,
            24, 25, 26, 27, 28, 29, 30, 31,
            32, 33, 34, 35, 36, 37, 38, 39,
            40, 41, 42, 43, 44, 45, 46, 47
        };

        private static byte[][] CornerMap = new byte[8][] 
        {
            new byte[3] {0, 1, 2},
            new byte[3] {0, 2, 3},
            new byte[3] {0, 3, 4},
            new byte[3] {0, 4, 1},
            new byte[3] {5, 2, 1},
            new byte[3] {5, 3, 2},
            new byte[3] {5, 4, 3},
            new byte[3] {5, 1, 4}
        };

        private static byte[][] EdgeMap = new byte[12][]
        {
            new byte[2] {0, 1},
            new byte[2] {0, 2},
            new byte[2] {0, 3},
            new byte[2] {0, 4},
            new byte[2] {5, 1},
            new byte[2] {5, 2},
            new byte[2] {5, 3},
            new byte[2] {5, 4},
            new byte[2] {2, 1},
            new byte[2] {2, 3},
            new byte[2] {4, 3},
            new byte[2] {4, 1}
        };

        private static byte[][] CornerFacelet = new byte[8][]
        {
            new byte[3] {0x00 + 4, 0x08 + 0, 0x10 + 2},
            new byte[3] {0x00 + 6, 0x10 + 0, 0x18 + 2},
            new byte[3] {0x00 + 0, 0x18 + 0, 0x20 + 2},
            new byte[3] {0x00 + 2, 0x20 + 0, 0x08 + 2},
            new byte[3] {0x28 + 2, 0x10 + 4, 0x08 + 6},
            new byte[3] {0x28 + 0, 0x18 + 4, 0x10 + 6},
            new byte[3] {0x28 + 6, 0x20 + 4, 0x18 + 6},
            new byte[3] {0x28 + 4, 0x08 + 4, 0x20 + 6},
        };

        private static byte[][] EdgeFacelet = new byte[12][]
        {
            new byte[2] {0x00 + 3, 0x08 + 1},
            new byte[2] {0x00 + 5, 0x10 + 1},
            new byte[2] {0x00 + 7, 0x18 + 1},
            new byte[2] {0x00 + 1, 0x20 + 1},
            new byte[2] {0x28 + 3, 0x08 + 5},
            new byte[2] {0x28 + 1, 0x10 + 5},
            new byte[2] {0x28 + 7, 0x18 + 5},
            new byte[2] {0x28 + 5, 0x20 + 5},
            new byte[2] {0x10 + 3, 0x08 + 7},
            new byte[2] {0x10 + 7, 0x18 + 3},
            new byte[2] {0x20 + 3, 0x18 + 7},
            new byte[2] {0x20 + 7, 0x08 + 3},
        };

        private static byte[] USlice = new byte[4] { 0, 1, 2, 3 };
        private static byte[] DSlice = new byte[4] { 4, 5, 6, 7 };
        private static byte[] UDSlice = new byte[4] { 8, 9, 10, 11 };

        public Cubie[] corners;
        public Cubie[] edges;

        public Cube(Cubie[] corners = null, Cubie[] edges = null)
        {
            this.corners = corners ?? new Cubie[8]
            {
                new Cubie(0, 0), new Cubie(1, 0), new Cubie(2, 0), new Cubie(3, 0),
                new Cubie(4, 0), new Cubie(5, 0), new Cubie(6, 0), new Cubie(7, 0)
            };

            this.edges = edges ?? new Cubie[12]
            {
                new Cubie(0, 0), new Cubie(1, 0), new Cubie(2, 0), new Cubie(3, 0),
                new Cubie(4, 0), new Cubie(5, 0), new Cubie(6, 0), new Cubie(7, 0),
                new Cubie(8, 0), new Cubie(9, 0), new Cubie(10, 0), new Cubie(11, 0)
            };
        }

        public Cube(byte[] faceletColors)
        {
            corners = new Cubie[8];
            edges = new Cubie[12];
            byte[] tuple, cubie;
            int i, o, val; 

            for (i = 0; i < 8; i++)
            {
                tuple = CornerFacelet[i];
                cubie = tuple.Select(x => faceletColors[x]).ToArray();
                val = CornerMap.Index(cubie, Tools.setEquals);
                o = cubie.Index(CornerMap[val][0]);
                corners[i] = new Cubie((byte)val, (byte)o);
            }

            for (i = 0; i < 12; i++)
            {
                tuple = EdgeFacelet[i];
                cubie = tuple.Select(x => faceletColors[x]).ToArray();
                val = EdgeMap.Index(cubie, Tools.setEquals);
                o = cubie.Index(EdgeMap[val][0]);
                edges[i] = new Cubie((byte)val, (byte)o);
            }
        }

        public Cube(Cube other)
        {
            corners = other.corners.Select(x => new Cubie(x.pos, x.orient)).ToArray();
            edges = other.edges.Select(x => new Cubie(x.pos, x.orient)).ToArray();
        }

        public byte[] getFacelets()
        {
            byte[] facelets = new byte[48];
            int i, j;

            for (i = 0; i < 8; i++)
            {
                Cubie c = corners[i];
                var ct = CornerFacelet[c.pos].rotate(c.orient);
                for (j = 0; j < 3; j++) { facelets[CornerFacelet[i][j]] = ct[j]; }
            }

            for (i = 0; i < 12; i++)
            {
                Cubie e = edges[i];
                var et = EdgeFacelet[e.pos].rotate(e.orient);
                for (j = 0; j < 2; j++) { facelets[EdgeFacelet[i][j]] = et[j]; }
            }

            return facelets;
        }

        public byte[] getFaceletColors()
        {
            return getFacelets().Select(x => (byte)(x / 8)).ToArray();
        }

        public void apply(Move m)
        {
            Cube c = m.apply(this);
            edges = c.edges;
            corners = c.corners;
        }

        public bool isSolved()
        {
            return (
                cornOrientCoord() == 0 &&
                edgeOrientCoord() == 0 &&
                cornPermCoord() == 0 &&
                edgePermCoord() == 0
           );
        }

        public bool isPhase2()
        {
            return (
                cornOrientCoord() == 0 &&
                edgeOrientCoord() == 0 &&
                UDSliceCoord() == 0
            );
        }
    }
}
