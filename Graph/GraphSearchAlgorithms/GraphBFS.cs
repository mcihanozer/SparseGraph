// M. Cihan Ozer - April 2017

using System.Collections.Generic;
using GraphCommons.GraphSearchCommons;

// Graph breadth first search (BFS) algorithm
//
// Perform more search operation than depth first search (DFS),
// guarantees that the path found contains the fewest edges possible
// (one of the paths if there are several paths are available with
// the smallest number of edges). More suitable for small search spaces
// (because of high branching factor: BFS will require a lot of memory
// and perform poorly if the branching factor is high).
sealed class GraphBFS<NodeType, EdgeType> : AbstractGraphSearch<NodeType, EdgeType>
        where NodeType : GraphNode
        where EdgeType : GraphEdge
{
    private Queue<HelperEdge> searchQueue;

    public GraphBFS()
    {
        Init(GraphSearchCommons.AvgMaxStackElement);
    }

    public GraphBFS(int nodeSize)
    {
        Init(nodeSize);
    }

    protected override void Init(int size)
    {
        searchQueue = new Queue<HelperEdge>(size);
        base.Init(size);
    }

    protected override void Reset()
    {
        searchQueue.Clear();
        base.Reset();
    }

    // Finds the path between source and target indices.
    //
    // Returns true if a path exists, returns false otherwise.
    //
    // TODO Replace ref with in if one day Unity supports C# 7+
    protected override bool FindPath(int sourceIndex, int targetIndex, AbstractGraph<NodeType, EdgeType> sourceGraph)
    {
        HelperEdge startEdge = new HelperEdge(sourceIndex, sourceIndex, 0f);

        searchQueue.Enqueue(startEdge);
        visitingList[sourceIndex] = GraphSearchCommons.SearchFlags.visited;

        // Search
        while (searchQueue.Count > 0)
        {
            // Get the next element in the queue
            HelperEdge next = searchQueue.Dequeue();

            // Mark for the route
            route[next.ToNode] = next.FromNode;

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
                        // Push each path leading to an unvisited node
                        searchQueue.Enqueue(edge);
                        // And mark it as visited
                        visitingList[edge.ToNode] = GraphSearchCommons.SearchFlags.visited;
                    }
                }
            }

        }   // End of while

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
        HelperEdge startEdge = new HelperEdge(sourceIndex, sourceIndex, 0f);

        searchQueue.Enqueue(startEdge);
        visitingList[sourceIndex] = GraphSearchCommons.SearchFlags.visited;

        // Search
        while (searchQueue.Count > 0)
        {
            // Get the next element in the queue
            HelperEdge next = searchQueue.Dequeue();

            // Mark for the route
            route[next.ToNode] = next.FromNode;

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
                        // Push each path leading to an unvisited node
                        searchQueue.Enqueue(edge);
                        // And mark it as visited
                        visitingList[edge.ToNode] = GraphSearchCommons.SearchFlags.visited;
                    }
                }
            }

        }   // End of while

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
        HelperEdge startEdge = new HelperEdge(sourceIndex, sourceIndex, 0f);

        searchQueue.Enqueue(startEdge);
        visitingList[sourceIndex] = GraphSearchCommons.SearchFlags.visited;

        // Search
        while(searchQueue.Count > 0)
        {
            // Get the next element in the queue
            HelperEdge next = searchQueue.Dequeue();

            // Mark for the route
            route[next.ToNode] = next.FromNode;

            // Add into the spanning tree
            if(next != startEdge)
            {
                spanningTree.Add(next);
            }

            // The target has found
            if(next.ToNode == targetIndex)
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
                        // Push each path leading to an unvisited node
                        searchQueue.Enqueue(edge);
                        // And mark it as visited
                        visitingList[edge.ToNode] = GraphSearchCommons.SearchFlags.visited;
                    }
                }
            }
            
        }   // End of while

        // No path to the target
        return false;
    }
}
