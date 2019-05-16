using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public int gridX; // xPos in Node array
    public int gridY; // yPos in Node array

    public bool IsWall; // Determines if this node is being obstructed
    public Vector3 Position; // World pos of the node

    public Node Parent; // For the a* algorithm, will store what node 
                        // it previously came from so it can trace the shortest path

    public int gCost; // Cost of moving to the next square
    public int hCost; // The distance to the goal from this node

    public int FCost { get { return gCost + hCost; } }

    public Node(bool _IsWall, Vector3 a_Pos, int a_gridX, int a_gridY)
    {
        IsWall = _IsWall;
        Position = a_Pos;
        gridX = a_gridX;
        gridY = a_gridY;

    }

}
