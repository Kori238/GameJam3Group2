using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditorInternal;
using UnityEngine;


public class Node
{
    private NodeGrid grid;
    public int x, y;
    public int gCost, hCost, fCost;

    public Node previousNode;
    public bool isWalkable;

    public void updateFCost()
    {
        fCost = gCost + hCost;
    }

    public Node(NodeGrid grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.isWalkable = true;
    }


}

public class NodeGrid
{
    private int width;
    private int height;
    private float cellSize;
    public Node[,] gridArray;

    public NodeGrid(int width, int height, float cellSize)
    {
        this.width = width; // this is called by Init to define the width, height and cellSize there
        this.height = height;
        this.cellSize = cellSize;


        gridArray = new Node[width, height]; // creates a 2D array of GridCell type

        for (int x = 0; x < gridArray.GetLength(0); x++) // iterates through each cell
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new Node(this, x, y);
            }
        }
    }

    public IEnumerator DrawNodeOutline()
    {
        for (int x = 0; x < gridArray.GetLength(0); x++) // iterates through each cell
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Color color = Color.gray;
                if (!gridArray[x, y].isWalkable) color = Color.black;
                Debug.DrawLine(GetCellWorldPosition(x, y), GetCellWorldPosition(x, y + 1), color, 10f); // visual outline of cell gizmos
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

    public int GetWidth() { return width; }
    public int GetHeight() { return height; }
    


}

public class AStar
{
    private const int DIAGONAL_COST = 14;
    private const int STRAIGHT_COST = 10;

    private NodeGrid grid;
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

    public List<Node> FindPath(int x0, int y0, int xn, int yn)
    {
        Node startNode = grid.GetNodeFromPosition(x0, y0);
        Node endNode = grid.GetNodeFromPosition(xn, yn);

        unsearchedNodes = new List<Node> { startNode };
        searchedNodes = new List<Node>();

        for (int x=0; x<grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                Node node = grid.GetNodeFromPosition(x, y);
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
            Node currentNode = FindLowestFCostNode(unsearchedNodes);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }
            unsearchedNodes.Remove(currentNode);
            searchedNodes.Add(currentNode);

            foreach (Node adjacentNode in GetAdjacentNodes(currentNode))
            {
                if (searchedNodes.Contains(adjacentNode)) continue;
                if(!adjacentNode.isWalkable)
                {
                    searchedNodes.Add(adjacentNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, adjacentNode);
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
            }
        }
        // No possible path;
        return null;
    }

    private List<Node> GetAdjacentNodes(Node currentNode)
    {
        List<Node> adjacentNodes = new List<Node>();
        if (currentNode.x-1 >= 0)
        {
            adjacentNodes.Add(grid.GetNodeFromPosition(currentNode.x-1, currentNode.y));
            if (currentNode.y-1 >= 0) adjacentNodes.Add(grid.GetNodeFromPosition(currentNode.x-1, currentNode.y-1));
            if (currentNode.y+1 < grid.GetHeight()) adjacentNodes.Add(grid.GetNodeFromPosition(currentNode.x-1, currentNode.y+1));
        }
        if (currentNode.x+1 < grid.GetWidth())
        {
            adjacentNodes.Add(grid.GetNodeFromPosition(currentNode.x+1, currentNode.y));
            if (currentNode.y-1 >= 0) adjacentNodes.Add(grid.GetNodeFromPosition(currentNode.x+1, currentNode.y-1));
            if (currentNode.y+1 < grid.GetHeight()) adjacentNodes.Add(grid.GetNodeFromPosition(currentNode.x+1, currentNode.y+1));
        }
        if (currentNode.y-1 >= 0) adjacentNodes.Add(grid.GetNodeFromPosition(currentNode.x, currentNode.y-1));
        if (currentNode.y+1 < grid.GetHeight()) adjacentNodes.Add(grid.GetNodeFromPosition(currentNode.x, currentNode.y+1));

        return adjacentNodes;
    }

    private List<Node> CalculatePath(Node endNode)
    {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node currentNode = endNode;
        while(currentNode.previousNode != null)
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }
        path.Reverse();
        return path;
    }
    private int CalculateDistanceCost(Node a, Node b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + STRAIGHT_COST * remaining;
    }

    private Node FindLowestFCostNode(List<Node> nodeList)
    {
        Node lowestFCostNode = nodeList[0];
        foreach (Node node in nodeList)
        {
            if (node.fCost < lowestFCostNode.fCost) {
                lowestFCostNode = node;
            }
        }
        return lowestFCostNode;
    }

}
