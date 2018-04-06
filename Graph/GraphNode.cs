// M. Cihan Ozer - March 2017

using GraphCommons;

// Base/basic Node class of the graph
public class GraphNode
{
    protected int NodeId;
    public int Id
    {
        get { return this.NodeId; }
        set
        {
            if(value > NodeCommons.InvalidNodeId)
            {
                this.NodeId = value;
            }
            else
            {
                ErrorPrinter.PrintError("GraphNode", "Id-setter", "NODE HAS AN INVALID ID", "Cihan");

            }
        }
    }

    public GraphNode()
    {
        this.NodeId = NodeCommons.InvalidNodeId;
    }

    public GraphNode(int id)
    {
        this.NodeId = id;
    }

    public GraphNode(params object[] initializerList)
    {
        if (initializerList.Length < 1)
        {
            this.NodeId = NodeCommons.InvalidNodeId;
        }
        else
        {
            this.NodeId = (int)initializerList[0];
        }
    }

    public virtual void Init(params object[] initializerList)
    {
        if (initializerList.Length < 1)
        {
            ErrorPrinter.PrintError("SparseGraph", "Init", "THERE IS NOT ENOUGH INFO IN THE INIT-LIST", "Cihan");
        }
        else
        {
            this.NodeId = (int)initializerList[0];
        }
    }

    public void MarkAsInvalid()
    {
        this.NodeId = NodeCommons.InvalidNodeId;
    }

	public override string ToString()
	{
        return ("NODE: " + this.Id);
	}
}
