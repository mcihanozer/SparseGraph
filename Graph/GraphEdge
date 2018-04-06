// M. Cihan Ozer - March 2017

using System;
using GraphCommons;

// Base edge class that connects two nodes ((NodeFrom and NodeTo)).
public class GraphEdge
{
    protected int NodeFrom;
    public int FromNode
    {
        get { return this.NodeFrom; }
        set
        {
            if(value > NodeCommons.InvalidNodeId)
            {
                this.NodeFrom = value;
            }
            else
            {
                ErrorPrinter.PrintError("GraphEdge", "FromNode-setter", "INVALID NODE ID FOR EDGE(GraphEdge.NodeFrom)", "Cihan");
            }
        }
    }

    protected int NodeTo;
    public int ToNode
    {
        get { return this.NodeTo; }
        set
        {
            if (value > NodeCommons.InvalidNodeId)
            {
                this.NodeTo = value;
            }
            else
            {
                ErrorPrinter.PrintError("GraphEdge", "ToNode-setter", "INVALID NODE ID FOR EDGE(GraphEdge.NodeTo)", "Cihan");
            }
        }
    }

    protected float NodeCost;
    public float Cost
    {
        get
        {
            return this.NodeCost;
        }

        set
        {
            this.NodeCost = value;
        }
    }

    public GraphEdge()
    {
        this.NodeFrom = NodeCommons.InvalidNodeId;
        this.NodeTo = NodeCommons.InvalidNodeId;
        this.NodeCost = 1.0f;
    }

    public GraphEdge(int from, int to)
    {
        this.NodeFrom = from;
        this.NodeTo = to;
        this.NodeCost = 1.0f;
    }

    public GraphEdge(int from, int to, float cost)
    {
        this.NodeFrom = from;
        this.NodeTo = to;
        this.NodeCost = cost;
    }

    public GraphEdge(params object[] initializerList)
    {
        if (initializerList.Length < 2)
        {
            this.NodeFrom = NodeCommons.InvalidNodeId;
            this.NodeTo = NodeCommons.InvalidNodeId;
            this.NodeCost = 1.0f;
        }
        else
        {
            this.NodeFrom = (int)initializerList[0];
            this.NodeTo = (int)initializerList[1];

            if(initializerList.Length > 2)
            {
                this.NodeCost = Convert.ToSingle(initializerList[2]);
            }
            else
            {
                this.NodeCost = 1.0f;
            }
        }
    }

    public virtual void Init(params object[] initializerList)
    {
        if (initializerList.Length < 2)
        {
            ErrorPrinter.PrintError("SparseGraph", "Init", "THERE IS NOT ENOUGH INFO IN THE INIT-LIST", "Cihan");
        }
        else
        {
            this.NodeFrom = (int)initializerList[0];
            this.NodeTo = (int)initializerList[1];

            if (initializerList.Length > 2)
            {
                this.NodeCost = Convert.ToSingle(initializerList[2]);
            }
            else
            {
                this.NodeCost = 1.0f;
            }
        }
    }

    public static bool operator==(GraphEdge leftEdge, GraphEdge rightEdge)
    {
        return (
                    leftEdge.NodeFrom == rightEdge.FromNode &&
                    leftEdge.NodeTo == rightEdge.ToNode &&
                    Math.Abs(leftEdge.NodeCost - rightEdge.Cost) < EdgeCommons.EpsilonEdgeCost
                );
    }

    public static bool operator!=(GraphEdge leftEdge, GraphEdge rightEdge)
    {
        return !(leftEdge == rightEdge);
    }

	public override bool Equals(object obj)
	{
        if( obj == null || GetType() != obj.GetType() )
        {
            return false;
        }

        GraphEdge edge = (GraphEdge)obj;
        return this == edge;
	}

	public override int GetHashCode()
	{
        // https://msdn.microsoft.com/en-us/library/system.object.gethashcode(v=vs.90).aspx
        // "Frequently, a type has multiple data fields that can participate in generating the hash code.
        // One way to generate a hash code is to combine these fields using an XOR operation"
        return (FromNode ^ ToNode);
	}

	public override string ToString()
	{
        return ("EDGE FROM: " + this.FromNode + " TO: " + this.ToNode + " COST: " + this.Cost);
	}
}
