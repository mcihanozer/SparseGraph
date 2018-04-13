// M. Cihan Ozer - April 2017

using System.Linq;
using System.Collections.Generic;
using GraphCommons;
using GraphCommons.GraphSearchCommons;

// Graph depth first search (DFS) algorithm
//
// Performs less search operation than breadth first search (BFS),
// guarantees find the path if one exist, but does not guarantees
// that the found path contains the fewest edges possible.
public class GraphDFS<NodeType, EdgeType>
        where NodeType : GraphNode
        where EdgeType : GraphEdge
{
    private Stack<HelperEdge> searchStack;

    // To keep track of the visited nodes
    private List<GraphSearchCommons.SearchFlags> visitingList;

    // Helper list to find out the route.
    //
    // Indices of the list are associated with the nodes, and each list element
    // holds index of its parent. For example, if the path is 0 - 3 - 12, then
    // route[3] holds 0, route[12] holds 3.
    private List<int> route;

    public GraphDFS()
    {
        Init(GraphSearchCommons.AvgMaxStackElement);
    }

    public GraphDFS(int nodeSize)
    {
        Init(nodeSize);
    }

    private void Init(int size)
    {
        searchStack = new Stack<HelperEdge>(size);

        visitingList = Enumerable.Repeat(
                                            GraphSearchCommons.SearchFlags.unvisited,
                                            size
                                        ).ToList();

        route = Enumerable.Repeat(
                                    (int)GraphSearchCommons.SearchStateFlags.noParentAssinged,
                                    size
                                 ).ToList();

    }

    private void Reset()
    {
        searchStack.Clear();

        for (int li = 0; li < visitingList.Count; ++li)
        {
            visitingList[li] = GraphSearchCommons.SearchFlags.unvisited;
        }

        for (int li = 0; li < route.Count; ++li)
        {
            route[li] = (int)GraphSearchCommons.SearchStateFlags.noParentAssinged;
        }

    }

    // Finds the path between source and target indices.
    //
    // Returns true if a path exists, returns false otherwise.
    //
    // TODO Replace ref with in if one day Unity supports C# 7+
    public bool FindPath(int sourceIndex, int targetIndex, AbstractGraph<NodeType, EdgeType> sourceGraph)
    {
        var validation = validateTheSearch(sourceIndex, targetIndex, sourceGraph);
        if (validation == GraphSearchCommons.SearchValidationFlgas.invalidIndices)
        {
            return false;
        }
        else if (validation == GraphSearchCommons.SearchValidationFlgas.atTheTarget)
        {
            return true;
        }

        // Preparation for the search
        Reset();

        HelperEdge startEdge = new HelperEdge(sourceIndex, sourceIndex, 0f);

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
    public bool FindPath(
                            int sourceIndex, int targetIndex,
                            AbstractGraph<NodeType, EdgeType> sourceGraph,
                            ref List<int> path
                        )
    {
        var validation = validateTheSearch(sourceIndex, targetIndex, sourceGraph);
        if (validation == GraphSearchCommons.SearchValidationFlgas.invalidIndices)
        {
            return false;
        }
        else if (validation == GraphSearchCommons.SearchValidationFlgas.atTheTarget)
        {
            return true;
        }

        // Preparation for the search
        Reset();

        HelperEdge startEdge = new HelperEdge(sourceIndex, sourceIndex, 0f);

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
    public bool FindPath(
                            int sourceIndex, int targetIndex,
                            AbstractGraph<NodeType, EdgeType> sourceGraph,
                            ref List<int> path,
                            //ref List<GraphEdge> spanningTree
                            ref List<HelperEdge> spanningTree
                        )
    {
        var validation = validateTheSearch(sourceIndex, targetIndex, sourceGraph);
        if(validation == GraphSearchCommons.SearchValidationFlgas.invalidIndices)
        {
            return false;
        }
        else if(validation == GraphSearchCommons.SearchValidationFlgas.atTheTarget)
        {
            return true;
        }

        // Preparation for the search
        Reset();

        HelperEdge startEdge = new HelperEdge(sourceIndex, sourceIndex, 0f);

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

    private GraphSearchCommons.SearchValidationFlgas validateTheSearch(
                                                                            int sourceIndex, int targetIndex,
                                                                            AbstractGraph<NodeType, EdgeType> sourceGraph
                                                                        )
    {
        if (sourceIndex == NodeCommons.InvalidNodeId || targetIndex == NodeCommons.InvalidNodeId)
        {
            ErrorPrinter.PrintError(
                                        "GraphDFS", "validateTheSearch", "Has invalid indices for SOURCE: " +
                                        sourceIndex + " TARGET: " + targetIndex, "Cihan"
                                    );
            return GraphSearchCommons.SearchValidationFlgas.invalidIndices;
        }

        if (sourceGraph.GetNode(sourceIndex) == null || sourceGraph.GetNode(targetIndex) == null)
        {
            ErrorPrinter.PrintError(
                                        "GraphDFS", "validateTheSearch", "Has invalid nodes in the grap! SOURCE: " +
                                        sourceIndex + " TARGET: " + targetIndex, "Cihan"
                                    );
            return GraphSearchCommons.SearchValidationFlgas.invalidIndices;
        }

        if (sourceIndex == targetIndex)
        {
            return GraphSearchCommons.SearchValidationFlgas.atTheTarget;
        }

        return GraphSearchCommons.SearchValidationFlgas.valid;
    }

    // Helper method to extract the path via the route member
    private void GetTargetPath(int sourceIndex, int targetIndex, ref List<int> path)
    {
        int nd = targetIndex;
        path.Insert(0, nd);

        while (nd != sourceIndex)
        {
            nd = route[nd];
            path.Insert(0, nd);
        }
    }

}
