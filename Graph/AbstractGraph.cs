// M. Cihan Ozer - March 2017

using System;
using System.Collections.Generic;
using GraphCommons;

using UnityEngine;  // Because of SparseGraph.DebugPrint()

// Base graph class with adjacency list representation
public abstract class AbstractGraph<NodeType, EdgeType>
    where NodeType : GraphNode
    where EdgeType : GraphEdge
{
    // The index of the next node to be added
    protected int nextNodeIndex;
    public int NextFreeNodeIndex
    {
        get { return nextNodeIndex; }
    }

    // The nodes in the graph
    protected List<NodeType> nodes;

    // A list of adjacency edge lists.
    // Each node index leads to the list of associated edges
    protected List<LinkedList<EdgeType>> edges;

    public AbstractGraph()
    {
        this.nextNodeIndex = 0;

        this.nodes = new List<NodeType>();
        this.edges = new List<LinkedList<EdgeType>>();
    }

    // Invalidates the node with the given id
    public abstract void RemoveNode(int nodeId);

    // Adds an edge to the graph.
    public abstract void AddEdge(params object[] initializerList);

    // Adds an edge to the graph.
    // WARNING THIS VERSION MAY CREATE GARBAGE
    public abstract void AddEdge(EdgeType edge);

    // Removes the edge if present.
    public abstract void RemoveEdge(int from, int to);

    private void RemoveAllEdges()
    {
        for (int eli = 0; eli < this.edges.Count; ++eli)
        {
            this.edges[eli].Clear();
        }
    }

    // Checks whether the edge has already been presented in the graph.
    // Used when adding edges to the list for preventing the duplicates.
    // Returns true if an edge does not exist in the graph
    //
    // virtual, because SparseGraph needed to override it.
    protected virtual bool IsUniqueEdge(int from, int to)
    {
        if (IsNodePresent(from) && IsNodePresent(to))
        {
            foreach (var edge in this.edges[from])
            {
                if (edge.ToNode == to)
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    // Iterates through all the edges in the graph and removes invalidated edges
    protected void RemoveInvalidEdges()
    {
        for (int eli = 0; eli < this.edges.Count; eli++)
        {
            var edge = this.edges[eli].First;
            while (edge != null)
            {
                var nextEdge = edge.Next;
                if (
                        this.nodes[edge.Value.ToNode].Id == NodeCommons.InvalidNodeId ||
                        this.nodes[edge.Value.FromNode].Id == NodeCommons.InvalidNodeId
                    )
                {
                    this.edges[eli].Remove(edge);
                }
                edge = nextEdge;
            }

        }
    }

    // Clears the graph
    public void Clear()
    {
        RemoveAllEdges();
        this.edges.Clear();

        this.nextNodeIndex = 0;
        this.nodes.Clear();
    }

    public bool IsGraphEmpty()
    {
        return this.nodes.Count < 1;
    }

    // Adds a node to the graph and returns its index
    //
    // This version handles memory allocation on its own and minimise the garbage
    // creation. If a formerly invalid node is trying to adding (reactivation)
    // instead of creating a new node, the invalidated one is reactivated.
    public int AddNode(params object[] initializerList)
    {
        int nodeId = (int)initializerList[0];

        if (nodeId < this.nodes.Count)
        {
            // Make sure the client is not trying to add a node with the same ID as a currently active node
            if (this.nodes[nodeId].Id != NodeCommons.InvalidNodeId)
            {
                ErrorPrinter.PrintError("SparseGraph", "AddNode - line 89", "Attempting to add a node with a duplicate ID", "Cihan");
                return NodeCommons.InvalidNodeId;
            }

            this.nodes[nodeId].Init(initializerList);

            return nextNodeIndex;
        }
        else
        {
            // Make sure the new node has been indexed correctly
            if (nodeId == NodeCommons.InvalidNodeId)
            {
                ErrorPrinter.PrintError("SparseGraph", "AddNode - line 104", "Invalid index", "Cihan");
                return NodeCommons.InvalidNodeId;
            }

            NodeType newNode = Activator.CreateInstance<NodeType>();
            newNode.Init(initializerList);

            this.nodes.Add(newNode);
            this.edges.Add(new LinkedList<EdgeType>());

            return nextNodeIndex++;
        }
    }

    // Adds a node to the graph and returns its index
    // WARNING THIS MAY CREATE GARBAGE COLLECTION
    public int AddNode(NodeType node)
    {
        if (node.Id < this.nodes.Count)
        {
            // Make sure the client is not trying to add a node with the same ID as a currently active node
            if (this.nodes[node.Id].Id != NodeCommons.InvalidNodeId)
            {
                ErrorPrinter.PrintError("SparseGraph", "AddNode - line 89", "Attempting to add a node with a duplicate ID", "Cihan");
                return NodeCommons.InvalidNodeId;
            }

            // So the new node passed won't wander around
            this.nodes.Remove(this.nodes[node.Id]);
            this.nodes.Insert(node.Id, node);

            return nextNodeIndex;
        }
        else
        {
            // Make sure the new node has been indexed correctly
            if (node.Id == NodeCommons.InvalidNodeId)
            {
                ErrorPrinter.PrintError("SparseGraph", "AddNode - line 104", "Invalid index", "Cihan");
                return NodeCommons.InvalidNodeId;
            }

            this.nodes.Add(node);
            this.edges.Add(new LinkedList<EdgeType>());

            return nextNodeIndex++;
        }
    }

    // Returns the node with a given id/index if one exists and active
    // Returns null otherwise
    public NodeType GetNode(int nodeId)
    {
        if (nodeId >= this.nodes.Count || nodeId <= NodeCommons.InvalidNodeId)
        {
            ErrorPrinter.PrintError("SparseGraph", "GetNode", "INVALID NODE INDEX", "Cihan");
            return null;
        }

        return this.nodes[nodeId];
    }

    // Returns (# active nodes) + ( # inactive nodes) in the graph
    public int GetNumberOfAllNodes()
    {
        return this.nodes.Count;
    }

    // Returns the number of active nodes in the graph
    public int GetNumberOfActiveNodes()
    {
        // TODO Improve the performance by caching if needed. Currently, calling
        //      this method frequently is not expected.

        int count = 0;
        for (int ni = 0; ni < this.nodes.Count; ++ni)
        {
            if (this.nodes[ni].Id != NodeCommons.InvalidNodeId)
            {
                ++count;
            }
        }
        return count;
    }

    public int GetNumberOfInactiveNodes()
    {
        return (GetNumberOfAllNodes() - GetNumberOfActiveNodes());
    }

    // Returns true if a node with the given index is in the graph and active
    public bool IsNodePresent(int id)
    {
        if ((id >= this.nodes.Count) || (this.nodes[id].Id == NodeCommons.InvalidNodeId))
        {
            return false;
        }

        return true;
    }

    // Returns the edge between the nodes given if one exists.
    // Otherwise, returns null
    public virtual EdgeType GetEdge(int from, int to)
    {
        if (
                (from >= this.nodes.Count) || (from <= NodeCommons.InvalidNodeId) ||
                this.nodes[from].Id == NodeCommons.InvalidNodeId
            )
        {
            ErrorPrinter.PrintError("SparseGraph", "GetEdge - line 202", "INVALID Edge.FromNode INDEX", "Cihan");
            return null;
        }

        if (
                (to >= this.nodes.Count) || (to <= NodeCommons.InvalidNodeId) ||
                this.nodes[to].Id == NodeCommons.InvalidNodeId
            )
        {
            ErrorPrinter.PrintError("SparseGraph", "GetEdge - line 214", "INVALID Edge.ToNode INDEX", "Cihan");
            return null;
        }

        foreach (EdgeType edge in this.edges[from])
        {
            if (edge.ToNode == to)
            {
                return edge;
            }
        }

        ErrorPrinter.PrintError("SparseGraph", "GetEdge - line 229", "THE EDGE DOES NOT EXIST BETWEEN from: " + from + " to: " + to, "Cihan");
        return null;

    }

    // Sets the cost of an edge if one exists
    public virtual void SetEdgeCost(int from, int to, float cost)
    {
        // Check if the nodes are valid
        if (from >= this.nodes.Count || to >= this.nodes.Count)
        {
            ErrorPrinter.PrintError("SparseGraph", "SetEdgeCost", "INVALID INDEX! From: " + from + " to: " + to, "Cihan");
            return;
        }

        // Set cost of each from-to edges
        foreach (var edge in this.edges[from])
        {
            if (edge.ToNode == to)
            {
                edge.Cost = cost;
                break;
            }
        }
    }

    // Returns the total number of edges in the graph
    public int GetNumberOfEdges()
    {
        int count = 0;
        for (int eli = 0; eli < this.edges.Count; ++eli)
        {
            count += this.edges[eli].Count;
        }
        return count;
    }

    // Returns true if an edge is in the graph
    public virtual bool IsEdgePresent(int from, int to)
    {
        if (IsNodePresent(from) && IsNodePresent(to))
        {
            foreach (EdgeType edge in this.edges[from])
            {
                if (edge.ToNode == to)
                {
                    return true;
                }
            }

            return false;
        }

        return false;
    }

    public void DebugPrint()
    {
        Debug.Log("*** SparseGraph.DebugPrint() ***");
        Debug.Log("SP.DEBUG BEGINS!!!");

        Debug.Log("********** NODES **********");
        foreach (var node in this.nodes)
        {
            Debug.Log( node.ToString() );
        }

        Debug.Log("********** EDGES **********");
        foreach (var edgeList in this.edges)
        {
            foreach (var edge in edgeList)
            {
                Debug.Log( edge.ToString() );
            }
        }

        Debug.Log("SP.DEBUG ENDS!!!");
        Debug.Log("*** SparseGraph.DebugPrint() ***");
    }

}

