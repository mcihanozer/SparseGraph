// M. Cihan Ozer - April 2017

using System.Linq;
using System.Collections.Generic;
using GraphCommons;
using GraphCommons.GraphSearchCommons;

// Base class for graph search algorithms
public abstract class AbstractGraphSearch<NodeType, EdgeType>
        where NodeType : GraphNode
        where EdgeType : GraphEdge
{
    // To keep track of the visited nodes
    protected List<GraphSearchCommons.SearchFlags> visitingList;

    // Helper list to find out the route.
    //
    // Indices of the list are associated with the nodes, and each list element
    // holds index of its parent. For example, if the path is 0 - 3 - 12, then
    // route[3] holds 0, route[12] holds 3.
    protected List<int> route;

    // Finds the path between source and target indices.
    //
    // Returns true if a path exists, returns false otherwise.
    //
    // TODO Replace ref with in if one day Unity supports C# 7+
    protected abstract bool FindPath(int sourceIndex, int targetIndex, AbstractGraph<NodeType, EdgeType> sourceGraph);

    // Finds and returns the path between source and target indices.
    //
    // Returns true if a path exists, returns false otherwise.
    //
    // TODO Replace ref with in if one day Unity supports C# 7+
    protected abstract bool FindPath(
                                    int sourceIndex, int targetIndex,
                                    AbstractGraph<NodeType, EdgeType> sourceGraph,
                                    ref List<int> path
                                );

    // Finds and returns the path between source and target indices
    // and the spanning tree of the search.
    //
    // Returns true if a path exists, returns false otherwise.
    //
    // TODO Replace ref with in if one day Unity supports C# 7+
    protected abstract bool FindPath(
                                    int sourceIndex, int targetIndex,
                                    AbstractGraph<NodeType, EdgeType> sourceGraph,
                                    ref List<int> path,
                                    ref List<HelperEdge> spanningTree
                                );

    protected virtual void Init(int size)
    {

        visitingList = Enumerable.Repeat(
                                            GraphSearchCommons.SearchFlags.unvisited,
                                            size
                                        ).ToList();

        route = Enumerable.Repeat(
                                    (int)GraphSearchCommons.SearchStateFlags.noParentAssinged,
                                    size
                                 ).ToList();
    }

    protected virtual void Reset()
    {
        for (int li = 0; li < visitingList.Count; ++li)
        {
            visitingList[li] = GraphSearchCommons.SearchFlags.unvisited;
        }

        for (int li = 0; li < route.Count; ++li)
        {
            route[li] = (int)GraphSearchCommons.SearchStateFlags.noParentAssinged;
        }

    }

    protected GraphSearchCommons.SearchValidationFlgas validateTheSearch(
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
    protected void GetTargetPath(int sourceIndex, int targetIndex, ref List<int> path)
    {
        int nd = targetIndex;
        path.Insert(0, nd);

        while (nd != sourceIndex)
        {
            nd = route[nd];
            path.Insert(0, nd);
        }
    }

    // Search interface for the client
    // TODO Replace ref with in if one day Unity supports C# 7+
    public bool Search(int sourceIndex, int targetIndex, AbstractGraph<NodeType, EdgeType> sourceGraph)
    {
        // TODO Update with a better approach in the future
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
        return FindPath(sourceIndex, targetIndex, sourceGraph);
    }

    // Search interface for the client
    // TODO Replace ref with in if one day Unity supports C# 7+
    public bool Search(
                                    int sourceIndex, int targetIndex,
                                    AbstractGraph<NodeType, EdgeType> sourceGraph,
                                    ref List<int> path
                                )
    {
        // TODO Update with a better approach in the future
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
        return FindPath(sourceIndex, targetIndex, sourceGraph, ref path);
    }

    // Search interface for the client
    // TODO Replace ref with in if one day Unity supports C# 7+
    public bool Search(
                                    int sourceIndex, int targetIndex,
                                    AbstractGraph<NodeType, EdgeType> sourceGraph,
                                    ref List<int> path,
                                    ref List<HelperEdge> spanningTree
                                )
    {
        // TODO Update with a better approach in the future
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
        return FindPath(sourceIndex, targetIndex, sourceGraph, ref path, ref spanningTree);
    }

}
