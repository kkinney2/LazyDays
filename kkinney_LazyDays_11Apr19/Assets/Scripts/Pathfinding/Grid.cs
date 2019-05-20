using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public Transform StartPosition;
    public LayerMask WallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float Distance;

    public bool DrawGrid = false;

    Node[,] grid;
    public List<Node> FinalPath;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[(int)gridSizeX, (int)gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2f - Vector3.forward * gridWorldSize.y / 2f;
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool Wall = true;

                if (Physics.CheckSphere(worldPoint,nodeRadius, WallMask))
                {
                    Wall = false;
                }

                grid[x, y] = new Node(Wall, worldPoint, x, y);
            }
        }
    }

    public Node NodeFromWorldPosition(Vector3 a_WorldPosition)
    {
        float xpoint = ((a_WorldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float ypoint = ((a_WorldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y);

        xpoint = Mathf.Clamp01(xpoint);
        ypoint = Mathf.Clamp01(ypoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xpoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * ypoint);

        return grid[x, y];
    }

    public List<Node> GetNeighborNodes(Node a_Node)
    {
        List<Node> NeighboringNodes = new List<Node>();
        int xCheck;
        int yCheck;

        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0:
                    // Right Side
                    xCheck = a_Node.gridX + 1;
                    yCheck = a_Node.gridY;
                    break;
                case 1:
                    // Left Side
                    xCheck = a_Node.gridX - 1;
                    yCheck = a_Node.gridY;
                    break;
                case 2:
                    // Top Side
                    xCheck = a_Node.gridX;
                    yCheck = a_Node.gridY + 1;
                    break;
                case 3:
                    // Bottom Side
                    xCheck = a_Node.gridX;
                    yCheck = a_Node.gridY - 1;
                    break;
                default:
                    Debug.Log("Too many sides to a node");
                    xCheck = 0;
                    yCheck = 0;
                    break;
            }

            if (xCheck >= 0 && xCheck < gridSizeX)
            {
                if (yCheck >= 0 && yCheck < gridSizeY)
                {
                    NeighboringNodes.Add(grid[xCheck, yCheck]);
                }
            }
        }

        return NeighboringNodes;
    }

    private void OnDrawGizmosSelected()
    {
        if (DrawGrid)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if (grid != null)
            {
                foreach (Node n in grid)
                {
                    if (n.IsWall)
                    {
                        Gizmos.color = Color.white;
                    }
                    else
                    {
                        Gizmos.color = Color.yellow;
                    }


                    if (FinalPath != null)
                    {
                        if (FinalPath.Contains(n))
                        {
                            Gizmos.color = Color.red;
                        }
                    }

                    Gizmos.DrawCube(n.Position, Vector3.one * (nodeDiameter - Distance));
                }
            }
        }
        
    }
}
