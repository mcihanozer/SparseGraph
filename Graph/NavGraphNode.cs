// M. Cihan Ozer - March 2017

using GraphCommons;
using UnityEngine;

// Graph node for creating a navigation graph
public class NavGraphNode : GraphNode
{
    protected Vector2 NodePos;
    public Vector2 Pos
    {
        get { return this.NodePos; }
        set
        {
            this.NodePos.x = value.x;
            this.NodePos.y = value.y;
        }
    }
    
    // TODO May need a reference to point a GameObject or sth

    public NavGraphNode() : base()
    {
        Pos = new Vector2();
    }

    public NavGraphNode(int id, Vector2 pos) : base(id)
    {
        this.NodePos = pos;
    }

    public NavGraphNode(params object[] initializerList)
    {
        if (initializerList.Length < 1)
        {
            this.NodeId = NodeCommons.InvalidNodeId;
            Pos = new Vector2();
        }
        else
        {
            this.NodeId = (int)initializerList[0];
            if(initializerList.Length > 1)
            {
                this.NodePos = (Vector2)initializerList[1];
            }
            else
            {
                this.NodePos = new Vector2();
            }
        }
    }

    public override void Init(params object[] initializerList)
    {
        if (initializerList.Length < 1)
        {
            ErrorPrinter.PrintError("SparseGraph", "Init-with initList", "THERE IS NOT ENOUGH INFO IN THE INIT-LIST", "Cihan");
        }
        else
        {
            this.NodeId = (int)initializerList[0];
            if (initializerList.Length > 1)
            {
                this.NodePos = (Vector2)initializerList[1];
            }
            else
            {
                this.NodePos = new Vector2();
            }
        }
    }

	public override string ToString()
	{
        return (base.ToString() + " POS: " + this.Pos );
	}
}
