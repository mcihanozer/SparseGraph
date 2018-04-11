# SparseGraph

Sparse graph implementation for Unity with C#. It includes graph and directed graph impelementations with
adjacency list representation.

Since I am using it in Unity, there are some Unity dependencies, but not more then Debug.Log() and Vector2.

**P.S.** If you can use higher level of C# in your project, do not forget to add in parameter modifier. Unity did not let me.

**P.S.** SparseGraph.cs is updated with a helper adjacency list (int based) to increase performance for removal operations,
   returning all possible paths for a pathfinding algorithm, etc.
