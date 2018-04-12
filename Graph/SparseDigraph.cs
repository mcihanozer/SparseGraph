// M. Cihan Ozer - March 2017

using System;
using System.Collections.Generic;
using GraphCommons;
using GraphCommons.GraphSearchCommons;

// Digraph (directed graph) with adjacency list representation
public class SparseDigraph<NodeType, EdgeType> : AbstractGraph<NodeType, EdgeType>
    where NodeType : GraphNode
    where EdgeType : GraphEdge
{
    public SparseDigraph() : base()
    {
        
    }

    // Nodes are not actually removed for keeping the indices (refers as Id in here)
    // steady. The node that will be removed marked as invalid and the edges leading
    // to this node are removed (destroyed).
    public override void RemoveNode(int nodeId)
    {
        if (nodeId >= this.nodes.Count)
        {
            ErrorPrinter.PrintError("SparseGraph", "RemoveNode", "INVALID NODE INDEX", "Cihan");
        }
        else
        {
            // Set this node's index to invalid_node_index
            this.nodes[nodeId].MarkAsInvalid();

            // If a digraph remove the edges the slow way
            RemoveInvalidEdges();
        }
    }

    // Adds an edge to the graph.
    public override void AddEdge(params object[] initializerList)
    {
        if (initializerList.Length < 2)
        {
            ErrorPrinter.PrintError("SparseGraph", "AddEdge-with initList", "THERE IS NOT ENOUGH INFO IN THE INIT-LIST", "Cihan");
            return;
        }
        else
        {
            int from = (int)initializerList[0];
            int to = (int)initializerList[1];

            // Make sure the from and to nodes exist within the graph 
            if ((from >= nextNodeIndex) || (to >= nextNodeIndex))
            {
                ErrorPrinter.PrintError("SparseGraph", "AddEdge-with initLis", "THE EDGE HAS INVALID INDICES: from: " + from + " to: " + to, "Cihan");
                return;
            }

            // Make sure both nodes are active before adding the edge
            if (
                    (this.nodes[to].Id != NodeCommons.InvalidNodeId) &&
                    (this.nodes[from].Id != NodeCommons.InvalidNodeId)
              )
            {
                // Add the edge, first making sure it is unique
                if (IsUniqueEdge(from, to))
                {
                    EdgeType newEdge = Activator.CreateInstance<EdgeType>();
                    newEdge.Init(initializerList);
                    this.edges[newEdge.FromNode].AddLast(newEdge);
                }
                else
                {
                    ErrorPrinter.PrintError("SparseGraph", "AddEdge", "THE EDGE IS NOT UNIQUE: from: " + from + " to: " + to, "Cihan");
                }

            }
            else
            {
                ErrorPrinter.PrintError(
                                            "SparseGraph", "AddEdge", "THE EDGE HAS INACTIVE NODES: fromNode: " +
                                            this.nodes[from].Id + " toNode: " + this.nodes[to].Id,
                                            "Cihan"
                                        );
            }

        }   // End of else it is safe to assign

    }   // End of SparseGraph.AddEdge()

    // Adds an edge to the graph. The method ensures that the edge is valid before
    // adding it to the graph.
    // WARNING THIS VERSION MAY CREATE GARBAGE
    public override void AddEdge(EdgeType edge)
    {
        // Make sure the from and to nodes exist within the graph 
        if ((edge.FromNode >= nextNodeIndex) || (edge.ToNode >= nextNodeIndex))
        {
            ErrorPrinter.PrintError("SparseGraph", "AddEdge", "THE EDGE HAS INVALID INDICES: from: " + edge.FromNode + " to: " + edge.ToNode, "Cihan");
            return;
        }

        // Make sure both nodes are active before adding the edge
        if (
                (this.nodes[edge.ToNode].Id != NodeCommons.InvalidNodeId) &&
                (this.nodes[edge.FromNode].Id != NodeCommons.InvalidNodeId)
            )
        {
            // Add the edge, first making sure it is unique
            if (IsUniqueEdge(edge.FromNode, edge.ToNode))
            {
                this.edges[edge.FromNode].AddLast(edge);
            }
            else
            {
                ErrorPrinter.PrintError("SparseGraph", "AddEdge", "THE EDGE IS NOT UNIQUE: from: " + edge.FromNode + " to: " + edge.ToNode, "Cihan");
            }

        }

    }   // End of AddEdge()

    // Removes the edge if presents.
    public override void RemoveEdge(int from, int to)
    {
        if ((from >= this.nodes.Count) || (to >= this.nodes.Count))
        {
            ErrorPrinter.PrintError("SparseGraph", "RemoveEdge", "THE INVALID EDGE BETWEEN from: " + from + " to: " + to, "Cihan");
        }
        else
        {
            EdgeCommons.RemoveFromLinkedList<EdgeType>(this.edges, from, to);
        }
    }

    // Returns the all possible paths from given node
    // Serves to path finding algorithms
    public override List<HelperEdge> GetLeadingEdges(int nodeIndex)
    {
        List<HelperEdge> edgeList = new List<HelperEdge>();

        foreach (EdgeType edge in this.edges[nodeIndex])
        {
            HelperEdge newEdge = new HelperEdge(edge.FromNode, edge.ToNode, 0f);
            edgeList.Add(newEdge);
        }

        return edgeList;
    }
}
