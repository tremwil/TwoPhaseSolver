using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoPhaseSolver
{
    public class Move
    {
        private static byte[,,] edgeMoveTable = new byte[6, 12, 2]
        {
            {{3, 0},{0, 0},{1, 0},{2, 0},{4, 0},{5, 0},  //U
            {6, 0},{7, 0},{8, 0},{9, 0},{10, 0},{11, 0}},
            {{8, 0},{1, 0},{2, 0},{3, 0},{11, 0},{5, 0}, //R
            {6, 0},{7, 0},{4, 0},{9, 0},{10, 0},{0, 0}},
            {{0, 0},{9, 1},{2, 0},{3, 0},{4, 0},{8, 1},  //F
            {6, 0},{7, 0},{1, 1},{5, 1},{10, 0},{11, 0}},
            {{0, 0},{1, 0},{2, 0},{3, 0},{5, 0},{6, 0},  //D
            {7, 0},{4, 0},{8, 0},{9, 0},{10, 0},{11, 0}},
            {{0, 0},{1, 0},{10, 0},{3, 0},{4, 0},{5, 0}, //L
            {9, 0},{7, 0},{8, 0},{2, 0},{6, 0},{11, 0}},
            {{0, 0},{1, 0},{2, 0},{11, 1},{4, 0},{5, 0}, //B
            {6, 0},{10, 1},{8, 0},{9, 0},{3, 1},{7, 1}}
        };

        private static byte[,,] cornMoveTable = new byte[6, 8, 2]
        {
            {{3, 0},{0, 0},{1, 0},{2, 0},{4, 0},{5, 0},{6, 0},{7, 0}}, //U
            {{4, 2},{1, 0},{2, 0},{0, 1},{7, 1},{5, 0},{6, 0},{3, 2}}, //R
            {{1, 1},{5, 2},{2, 0},{3, 0},{0, 2},{4, 1},{6, 0},{7, 0}}, //F
            {{0, 0},{1, 0},{2, 0},{3, 0},{5, 0},{6, 0},{7, 0},{4, 0}}, //D
            {{0, 0},{2, 1},{6, 2},{3, 0},{4, 0},{1, 2},{5, 1},{7, 0}}, //L
            {{0, 0},{1, 0},{3, 1},{7, 2},{4, 0},{5, 0},{2, 2},{6, 1}}  //B
        };

        public static byte[] phase2Move = new byte[10]
        {
            0, 5, 6, 7, 8, 9, 10, 11, 12, 17
        };

        public static string[] strmove = 
            "U U2 U' R R2 R' F F2 F' D D2 D' L L2 L' B B2 B'".Split(' ');

        public readonly byte[] moveList;

        public int Length
        {
            get { return moveList.Length; }
        }

        public Move(string movestr)
        {
            moveList = movestr.Split(' ').Select(x => (byte)strmove.Index(x)).ToArray();
        }

        public Move(byte[] moveList)
        {
            this.moveList = moveList;
        }

        public static Cube apply(Cube cube, byte move, byte ap = 3)
        {
            int ax = move / 3;
            int po = move % 3;
            int x, i;
            Cubie c;
            Cubie[] edges, corns;

            for (x = 0; x <= po; x++)
            {
                corns = new Cubie[8];
                edges = new Cubie[12];

                if ((ap & 2) == 2)
                {
                    for (i = 0; i < 8; i++)
                    {
                        c = cube.corners[cornMoveTable[ax, i, 0]];
                        corns[i] = new Cubie(
                            c.pos,
                            (byte)((cornMoveTable[ax, i, 1] + c.orient) % 3)
                        );
                    }
                }
                else
                {
                    corns = cube.corners;
                }

                if ((ap & 1) == 1)
                {
                    for (i = 0; i < 12; i++)
                    {
                        c = cube.edges[edgeMoveTable[ax, i, 0]];
                        edges[i] = new Cubie(
                            c.pos,
                            (byte)((edgeMoveTable[ax, i, 1] + c.orient) % 2)
                        );
                    }
                }
                else
                {
                    edges = cube.edges;
                }

                cube = new Cube(corns, edges);
            }

            return cube;
        }

        public Cube apply(Cube cube)
        {
            foreach(byte m in moveList)
            {
                cube = Move.apply(cube, m);
            }

            return cube;
        }

        public Move inverse()
        {
            byte[] nmoves = new byte[moveList.Length];
            int[] add = new int[3] { 2, 0, -2 };
            byte m, inv;

            for (int i = 0; i < moveList.Length; i++)
            {
                m = moveList[moveList.Length - i - 1];
                inv = (byte)(m + add[m % 3]);
                nmoves[i] = inv;
            }

            return new Move(nmoves);
        }

        public static Move randmove(int maneuverLen = 20)
        {
            int[] opface = new int[6] { 3, 4, 5, 0, 1, 2 };
            Random generator = new Random();
            byte[] moves = new byte[maneuverLen];
            moves[0] = (byte)generator.Next(18);
            int face, m;

            for (int i = 1; i < maneuverLen; i++)
            {
                face = moves[i - 1] / 3;

                m = generator.Next(18);
                while (m / 3 == face || m / 3 == opface[face])
                {
                    m = generator.Next(18);
                }

                moves[i] = (byte)m;
            }

            return new Move(moves);
        }

        public override string ToString()
        {
            if (Length == 0) return "None";
            return string.Join(" ", moveList.Select(x => strmove[x]));
        }

        public static Move operator +(Move a, Move b)
        {
            return new Move(a.moveList.Concat(b.moveList).ToArray());
        }

        public static readonly Move None = new Move(new byte[0]);
    }
}
