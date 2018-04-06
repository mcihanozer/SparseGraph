// M. Cihan Ozer - March 2017

using System;
using GraphCommons;

// Graph with adjacency list representation
//
// This implementation does not create extra edge for the paths between two nodes
// and decreases memory consumption (i.e., for connecting node 0 and node 1,
// only a single edge is created). Thanks to this approach, memory consumption
// is decreased to half.
//
// However, this approach may decrease the performance of removing a node from
// the graph. In this case, the whole edges are searched for finding and removing
// the edges leading to the removed node.
//
// Less memory consumption is prefered over removing a node efficiency, because
// this operation is not expected to happen frequently.
public class SparseGraph<NodeType, EdgeType> : AbstractGraph<NodeType, EdgeType>
    where NodeType : GraphNode
    where EdgeType : GraphEdge
{
    public SparseGraph() : base()
    {
        
    }

    // Checks whether the edge has already been presented in the graph.
    // Used when adding edges to the list for preventing the duplicates.
    // Returns true if an edge does not exist in the graph
    protected override bool IsUniqueEdge(int from, int to)
    {
        if (from > to)
        {
            HelperMethods.Swap<int>(ref from, ref to);
        }

        return base.IsUniqueEdge(from, to);
    }

    // Nodes are not actually removed for keeping the indices (refers as Id in here)
    // steady. The node that will be removed marked as invalid and the edges leading
    // to this node are removed (destroyed).
    public override void RemoveNode(int nodeId)
    {
        if(nodeId >= this.nodes.Count)
        {
            ErrorPrinter.PrintError("SparseGraph", "RemoveNode", "INVALID NODE INDEX", "Cihan");
        }
        else
        {
            // Mark the node as invalid
            this.nodes[nodeId].MarkAsInvalid();

            RemoveInvalidEdges();
        }
    }

    // Adds an edge to the graph.
    public override void AddEdge(params object[] initializerList)
    {
        if(initializerList.Length < 2)
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
                if (from > to)
                {
                    HelperMethods.Swap<int>(ref from, ref to);
                    initializerList[0] = from;
                    initializerList[1] = to;
                }

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

    // Adds an edge to the graph. The method ensures that the edge is valid
    // before adding it to the graph.
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
            if (edge.FromNode > edge.ToNode)
            {
                EdgeCommons.SwapEdgeNodes<EdgeType>(edge);
            }

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
        else
        {
            ErrorPrinter.PrintError(
                                        "SparseGraph", "AddEdge", "THE EDGE HAS INACTIVE NODES: fromNode: " +
                                        this.nodes[edge.FromNode].Id + " toNode: " + this.nodes[edge.ToNode].Id,
                                        "Cihan"
                                    );
        }

    }   // End of AddEdge()

    // Removes the edge if presents.
    public override void RemoveEdge(int from, int to)
    {
        if ((from >= this.nodes.Count) || (to >= this.nodes.Count))
        {
            ErrorPrinter.PrintError("SparseGraph", "RemoveEdge", "THE INVALID EDGE BETWEEN from: " + from + " to: " + to, "Cihan");
            return;
        }

        if (from > to)
        {
            HelperMethods.Swap<int>(ref from, ref to);
        }

        EdgeCommons.RemoveFromLinkedList<EdgeType>(this.edges, from, to);
    }

    // Returns the edge between the nodes given
    public override EdgeType GetEdge(int from, int to)
    {
        if (from > to)
        {
            HelperMethods.Swap<int>(ref from, ref to);
        }

        return base.GetEdge(from, to);
    }

    public override void SetEdgeCost(int from, int to, float cost)
    {
        if (from > to)
        {
            HelperMethods.Swap<int>(ref from, ref to);
        }

        base.SetEdgeCost(from, to, cost);
    }

    // Returns true if an edge is in the graph.
    public override bool IsEdgePresent(int from, int to)
    {
        if (from > to)
        {
            HelperMethods.Swap<int>(ref from, ref to);
        }

        return base.IsEdgePresent(from, to);
    }
}
