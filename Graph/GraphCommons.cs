// M. Cihan Ozer - March 2017

using System;
using System.Collections.Generic;

namespace GraphCommons
{
    public static class NodeCommons
    {
        public const int InvalidNodeId = -1;
    }

    public static class EdgeCommons
    {
        public const float EpsilonEdgeCost = 0.0001f;
        public const float DefaultEdgeCost = 0f;

        public static bool RemoveFromLinkedList<EdgeType>(List<LinkedList<EdgeType>> edgeList, int sourceId, int removeId)
            where EdgeType : GraphEdge
        {
            var node = edgeList[sourceId].First;
            while (node != null)
            {
                var nextNode = node.Next;
                if (node.Value.ToNode == removeId)
                {
                    edgeList[sourceId].Remove(node);
                    return true;
                }
                node = nextNode;
            }

            return false;
        }

        public static void SwapEdgeNodes<EdgeType>(EdgeType edge)
            where EdgeType : GraphEdge
        {
            var tempTo = edge.ToNode;
            edge.ToNode = edge.FromNode;
            edge.FromNode = tempTo;
        }

    }

    //public static class GraphSearchCommons
    namespace GraphSearchCommons
    {
        public static class GraphSearchCommons
        {
            public enum SearchValidationFlgas { invalidIndices, atTheTarget, valid };
            public enum SearchFlags { unvisited = 0, visited };
            public enum SearchStateFlags { noParentAssinged = -1 };

            public const int AvgMaxStackElement = 128;
        }
       
        // Helper edge structure for graph search algorithms
        // Struct based approach decreases the garbage creation and CPU usage
        public struct HelperEdge
        {
            public int FromNode;
            public int ToNode;
            public float Cost;

            public HelperEdge(int fromNode, int toNode, float cost)
            {
                this.FromNode = fromNode;
                this.ToNode = toNode;
                this.Cost = cost;
            }

            public static bool operator ==(HelperEdge leftEdge, HelperEdge rightEdge)
            {
                return (
                            leftEdge.FromNode == rightEdge.FromNode &&
                            leftEdge.ToNode == rightEdge.ToNode &&
                            Math.Abs(leftEdge.Cost - rightEdge.Cost) < EdgeCommons.EpsilonEdgeCost
                        );
            }

            public static bool operator !=(HelperEdge leftEdge, HelperEdge rightEdge)
            {
                return !(leftEdge == rightEdge);
            }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                HelperEdge edge = (HelperEdge)obj;
                return this == edge;
            }

            public override int GetHashCode()
            {
                // https://msdn.microsoft.com/en-us/library/system.object.gethashcode(v=vs.90).aspx
                // "Frequently, a type has multiple data fields that can participate in generating the hash code.
                // One way to generate a hash code is to combine these fields using an XOR operation"
                return (FromNode ^ ToNode);
            }
        }
    }

}
