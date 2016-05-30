# TwoPhaseSolver
Kociemba's algorithm implementation in C#. 

I originaly made it for a Mindstorms EV3 cube-solving robot, but it can be used in any situation. Note that I do not have any real experience in coding well, so the code is pretty much a mess without comments. I might add some documentation later on.

# Building
Simply open the solution in VisualStudio and hit "Build Solution". The "TwoPhaseSolver.dll" library and a test application named "SolverTest.exe" should be created. 

# Info
Solves the rubik's cube using a two-phase algorithm. The code first maps different permuations of the cube as natural numbers. Then, it uses precomputed tables to apply moves to these numbers. The search is IDA*-like with four different prune tables as a heuristic. The code first tries to solve the cube in a position where all edges and corners have an orientation of 0 (phase 1), to then solve it as a whole (phase 2). Prune tables for phase 1 go up to depth 12 (they cover all of it) while phase 2 tables only cover depth 12 out of a maximum of 18. This means that the code will sometimes run very slow when asked to solve for a 20 or sometimes 21 move solution. It works great from 22 to 24 moves.

# Notes
The Search class is a C# port (with some modifications) of the one in Kociemba's Java implementation. All credit for this class goes to him. His website (http://kociemba.org/cube.htm) is the main source of information I used to program this two phase solver. I highly recommend checking it out if you want to know more about Rubik's cubes.

Also note that the programs that generate the tables are NOT included in this solution. I might release them later.
