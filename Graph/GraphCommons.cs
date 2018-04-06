// M. Cihan Ozer - March 2017

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

}
