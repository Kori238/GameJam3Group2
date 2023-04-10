using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private GameObject attackPointReference;
    public int gCost, hCost, fCost;
    private NodeGrid grid;
    public bool isWalkable;

    public Node previousNode;
    public int x, y;

    public Node(NodeGrid grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
        attackPointReference = null;
    }

    public void updateFCost()
    {
        fCost = gCost + hCost;
    }

    public void RemoveAttackPoint()
    {
        Object.Destroy(attackPointReference);
        attackPointReference = null;
    }

    public void SetAttackPoint(GameObject reference)
    {
        attackPointReference = reference;
    }

    public GameObject GetAttackPoint()
    {
        return attackPointReference;
    }
}

public class Path
{
    public Collider2D attackPoint;
    public int fCost;
    public List<Node> nodes;
    public GameObject structure;
    public int tCost;

    public Path()
    {
        fCost = 0;
        tCost = 0;
        nodes = new List<Node>();
        structure = null;
        attackPoint = null;
    }
}

public class NodeGrid
{
    private readonly float cellSize;
    private readonly int height;
    private readonly int width;
    public Node[,] gridArray;

    public NodeGrid(int width, int height, float cellSize)
    {
        this.width = width; // this is called by Init to define the width, height and cellSize there
        this.height = height;
        this.cellSize = cellSize;


        gridArray = new Node[width, height]; // creates a 2D array of GridCell type

        for (var x = 0; x < gridArray.GetLength(0); x++) // iterates through each cell
        {
            for (var y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new Node(this, x, y);
            }
        }
    }

    public IEnumerator DrawNodeOutline()
    {
        for (var x = 0; x < gridArray.GetLength(0); x++) // iterates through each cell
        {
            for (var y = 0; y < gridArray.GetLength(1); y++)
            {
                var color = Color.gray;
                if (!gridArray[x, y].isWalkable) color = Color.black;
                Debug.DrawLine(GetCellWorldPosition(x, y), GetCellWorldPosition(x, y + 1), color,
                    10f); // visual outline of cell gizmos
                Debug.DrawLine(GetCellWorldPosition(x, y), GetCellWorldPosition(x + 1, y), color, 10f);
            }
        }
        yield return new WaitForSeconds(10f);
        yield return DrawNodeOutline();
    }
    private Vector2 GetCellWorldPosition(int x, int y)
    {
        return new Vector2(x, y) * cellSize;
    }

    public Vector2 GetWorldCellPosition(float x, float y)
    {
        return new Vector2(Mathf.FloorToInt(x / cellSize), Mathf.FloorToInt(y / cellSize));
    }

    public Node GetNodeFromPosition(int x, int y)
    {
        return gridArray[x, y];
    }

    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
}

public class AStar
{
    private const int DIAGONAL_COST = 14;
    private const int STRAIGHT_COST = 10;

    private readonly NodeGrid grid;
    private List<Node> searchedNodes;
    private List<Node> unsearchedNodes;

    public AStar(int width, int height, float cellSize)
    {
        grid = new NodeGrid(width, height, cellSize);
    }

    public NodeGrid GetGrid()
    {
        return grid;

    }

    public Path FindPath(int x0, int y0, int xn, int yn, int viewDistance = int.MaxValue, bool ignoreWalls = false)
    {
        var startNode = grid.GetNodeFromPosition(x0, y0);
        var endNode = grid.GetNodeFromPosition(xn, yn);

        unsearchedNodes = new List<Node> { startNode };
        searchedNodes = new List<Node>();

        /*if (viewDistance != int.MaxValue && searchedNodes.Count >= viewDistance / 10)
        {
            return null; //path is outside of acceptable view distance
        }*/

        for (var x = 0; x < grid.GetWidth(); x++)
        {
            for (var y = 0; y < grid.GetHeight(); y++)
            {
                var node = grid.GetNodeFromPosition(x, y);
                node.gCost = int.MaxValue;
                node.updateFCost();
                node.previousNode = null;
            }
        }
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.updateFCost();

        while (unsearchedNodes.Count > 0)
        {
            var currentNode = FindLowestFCostNode(unsearchedNodes);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }
            unsearchedNodes.Remove(currentNode);
            searchedNodes.Add(currentNode);

            foreach (var adjacentNode in GetAdjacentNodes(currentNode))
            {
                if (searchedNodes.Contains(adjacentNode)) continue;
                if (!ignoreWalls && !adjacentNode.isWalkable)
                {
                    searchedNodes.Add(adjacentNode);
                    continue;
                }

                var tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, adjacentNode);
                if (tentativeGCost < adjacentNode.gCost)
                {
                    adjacentNode.previousNode = currentNode;
                    adjacentNode.gCost = tentativeGCost;
                    adjacentNode.hCost = CalculateDistanceCost(adjacentNode, endNode);
                    adjacentNode.updateFCost();

                    if (!unsearchedNodes.Contains(adjacentNode))
                    {
                        unsearchedNodes.Add(adjacentNode);
                    }
                }
                if (viewDistance != int.MaxValue && tentativeGCost >= viewDistance * 2)
                {
                    return null; //path is outside of acceptable view distance
                }
            }

            
        }
        // No possible path;
        return null;
    }

    private List<Node> GetAdjacentNodes(Node currentNode)
    {
        var adjacentNodes = new List<Node>();
        if (currentNode.x - 1 >= 0)
        {
            adjacentNodes.Add(grid.GetNodeFromPosition(currentNode.x - 1, currentNode.y));
            if (currentNode.y - 1 >= 0) adjacentNodes.Add(grid.GetNodeFromPosition(currentNode.x - 1, currentNode.y - 1));
            if (currentNode.y + 1 < grid.GetHeight())
                adjacentNodes.Add(grid.GetNodeFromPosition(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < grid.GetWidth())
        {
            adjacentNodes.Add(grid.GetNodeFromPosition(currentNode.x + 1, currentNode.y));
            if (currentNode.y - 1 >= 0) adjacentNodes.Add(grid.GetNodeFromPosition(currentNode.x + 1, currentNode.y - 1));
            if (currentNode.y + 1 < grid.GetHeight())
                adjacentNodes.Add(grid.GetNodeFromPosition(currentNode.x + 1, currentNode.y + 1));
        }
        if (currentNode.y - 1 >= 0) adjacentNodes.Add(grid.GetNodeFromPosition(currentNode.x, currentNode.y - 1));
        if (currentNode.y + 1 < grid.GetHeight())
            adjacentNodes.Add(grid.GetNodeFromPosition(currentNode.x, currentNode.y + 1));

        return adjacentNodes;
    }

    private Path CalculatePath(Node endNode)
    {
        var path = new Path { fCost = endNode.fCost };
        path.nodes.Add(endNode);
        var currentNode = endNode;
        while (currentNode.previousNode != null)
        {
            path.nodes.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }
        path.nodes.Reverse();
        return path;
    }
    private int CalculateDistanceCost(Node a, Node b)
    {
        var xDistance = Mathf.Abs(a.x - b.x);
        var yDistance = Mathf.Abs(a.y - b.y);
        var remaining = Mathf.Abs(xDistance - yDistance);

        return DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + STRAIGHT_COST * remaining;
    }

    private Node FindLowestFCostNode(List<Node> nodeList)
    {
        var lowestFCostNode = nodeList[0];
        foreach (var node in nodeList)
        {
            if (node.fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = node;
            }
        }
        return lowestFCostNode;
    }
}
