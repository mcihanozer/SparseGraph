// M. Cihan Ozer - April 2017

using System.Collections.Generic;
using GraphCommons;
using GraphCommons.GraphSearchCommons;

// Graph depth first search (DFS) algorithm
//
// Perform less search operation than breadth first search (BFS),
// guarantees find the path if one exist, but does not guarantees
// that the found path contains the fewest edges possible.
//
// Since DFS produces a spanning tree, it does not take the edge weights
// into account. So, DFS is not perfectly suitable for using the weighted
// graphs.
sealed class GraphDFS<NodeType, EdgeType> : AbstractGraphSearch<NodeType, EdgeType>
        where NodeType : GraphNode
        where EdgeType : GraphEdge
{
    private Stack<HelperEdge> searchStack;

    public GraphDFS()
    {
        Init(GraphSearchCommons.AvgMaxStackElement);
    }

    public GraphDFS(int nodeSize)
    {
        Init(nodeSize);
    }

    protected override void Init(int size)
    {
        searchStack = new Stack<HelperEdge>(size);
        base.Init(size);
    }

    protected override void Reset()
    {
        searchStack.Clear();
        base.Reset();
    }

    // Finds the path between source and target indices.
    //
    // Returns true if a path exists, returns false otherwise.
    //
    // TODO Replace ref with in if one day Unity supports C# 7+
    protected override bool FindPath(int sourceIndex, int targetIndex, AbstractGraph<NodeType, EdgeType> sourceGraph)
    {
        HelperEdge startEdge = new HelperEdge(sourceIndex, sourceIndex, EdgeCommons.DefaultEdgeCost);

        searchStack.Push(startEdge);

        // Search
        while (searchStack.Count > 0)
        {
            // Get the top element of the stack
            //EdgeType next = searchStack.Peek();
            HelperEdge next = searchStack.Peek();
            searchStack.Pop();

            // Mark for the route
            route[next.ToNode] = next.FromNode;

            // And mark as visited
            visitingList[next.ToNode] = GraphSearchCommons.SearchFlags.visited;

            // The target has found
            if (next.ToNode == targetIndex)
            {
                return true;
            }

            List<HelperEdge> edgeList = sourceGraph.GetLeadingEdges(next.ToNode);
            if (edgeList.Count > 0)
            {
                foreach (HelperEdge edge in edgeList)
                {
                    // Push each path leading to an unvisited node
                    if (visitingList[edge.ToNode] == GraphSearchCommons.SearchFlags.unvisited)
                    {
                        searchStack.Push(edge);
                    }
                }
            }

        }

        // No path to the target
        return false;
    }

    // Finds and returns the path between source and target indices.
    //
    // Returns true if a path exists, returns false otherwise.
    //
    // TODO Replace ref with in if one day Unity supports C# 7+
    protected override bool FindPath(
                                    int sourceIndex, int targetIndex,
                                    AbstractGraph<NodeType, EdgeType> sourceGraph,
                                    ref List<int> path
                                )
    {
        HelperEdge startEdge = new HelperEdge(sourceIndex, sourceIndex, EdgeCommons.DefaultEdgeCost);

        searchStack.Push(startEdge);

        // Search
        while (searchStack.Count > 0)
        {
            // Get the top element of the stack
            //EdgeType next = searchStack.Peek();
            HelperEdge next = searchStack.Peek();
            searchStack.Pop();

            // Mark for the route
            route[next.ToNode] = next.FromNode;

            // And mark as visited
            visitingList[next.ToNode] = GraphSearchCommons.SearchFlags.visited;

            // The target has found
            if (next.ToNode == targetIndex)
            {
                GetTargetPath(sourceIndex, targetIndex, ref path);
                return true;
            }

            List<HelperEdge> edgeList = sourceGraph.GetLeadingEdges(next.ToNode);
            if (edgeList.Count > 0)
            {
                foreach (HelperEdge edge in edgeList)
                {
                    // Push each path leading to an unvisited node
                    if (visitingList[edge.ToNode] == GraphSearchCommons.SearchFlags.unvisited)
                    {
                        searchStack.Push(edge);
                    }
                }
            }

        }   // End of while()

        // No path to the target
        return false;
    }

    // Finds and returns the path between source and target indices
    // and the spanning tree of the search.
    //
    // Returns true if a path exists, returns false otherwise.
    //
    // TODO Replace ref with in if one day Unity supports C# 7+
    protected override bool FindPath(
                                    int sourceIndex, int targetIndex,
                                    AbstractGraph<NodeType, EdgeType> sourceGraph,
                                    ref List<int> path,
                                    ref List<HelperEdge> spanningTree
                                )
    {
        HelperEdge startEdge = new HelperEdge(sourceIndex, sourceIndex, EdgeCommons.DefaultEdgeCost);
        searchStack.Push(startEdge);

        // Search
        while (searchStack.Count > 0)
        {
            // Get the top element of the stack
            HelperEdge next = searchStack.Peek();
            searchStack.Pop();

            // Mark for the route
            route[next.ToNode] = next.FromNode;

            // Add into the spanning tree
            if (next != startEdge)
            {
                spanningTree.Add(next);
            }

            // And mark as visited
            visitingList[next.ToNode] = GraphSearchCommons.SearchFlags.visited;

            // The target has found
            if (next.ToNode == targetIndex)
            {
                GetTargetPath(sourceIndex, targetIndex, ref path);
                return true;
            }

            List<HelperEdge> edgeList = sourceGraph.GetLeadingEdges(next.ToNode);
            if (edgeList.Count > 0)
            {
                foreach (HelperEdge edge in edgeList)
                {
                    // Push each path leading to an unvisited node
                    if (visitingList[edge.ToNode] == GraphSearchCommons.SearchFlags.unvisited)
                    {
                        searchStack.Push(edge);
                    }
                }
            }

        }   // End of while()

        // No path to the target
        return false;
    }
}
