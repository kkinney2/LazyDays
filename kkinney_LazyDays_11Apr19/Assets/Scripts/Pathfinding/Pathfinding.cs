﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    [HideInInspector]
    public GameObject GameController;
    public NPCController owner;
    public Grid grid;
    //public Transform StartPosition;
    //public Transform TargetPosition;

    /*private void Awake()
    {
        if (gameObject.name == "GameController")
        {
            grid = GetComponent<Grid>();
        }
        else grid = GameController.GetComponent<Grid>();
        //grid = GetComponent<Grid>();
    }*/

    private void Start()
    {
        grid = owner.GetGrid();
    }

    private void Update()
    {
        /*if (GameController == null)
        {
            if (gameObject.name == "GameController")
            {
                grid = GetComponent<Grid>();
            }
            else grid = owner.GetGrid();
        }*/

        /*if (TargetPosition != null)
        {
            FindPath(StartPosition.position, TargetPosition.position);
        }*/

    }

    public void FindPathtoTarget(Vector3 a_StartPos , Vector3 a_TargetPos)
    {
        FindPath(a_StartPos, a_TargetPos);
    }


    private void FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {
        Node StartNode = grid.NodeFromWorldPosition(a_StartPos);
        Node TargetNode = grid.NodeFromWorldPosition(a_TargetPos);

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];
            for (int i = 0; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost || (OpenList[i].FCost == CurrentNode.FCost && OpenList[i].hCost < CurrentNode.hCost))
                {
                    CurrentNode = OpenList[i];
                }
            }

            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);
                break;
            }

            foreach (Node NeighborNode in grid.GetNeighborNodes(CurrentNode))
            {
                if (!NeighborNode.IsWall || ClosedList.Contains(NeighborNode))
                {
                    continue;
                }
                int MoveCost = CurrentNode.gCost + GetManhattenDistance(CurrentNode, NeighborNode);

                if(MoveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.gCost = MoveCost;
                    NeighborNode.hCost = GetManhattenDistance(NeighborNode, TargetNode);
                    NeighborNode.Parent = CurrentNode;

                    if (!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }
        }
    }

    private void GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        List<Node> FinalPath = new List<Node>();
        Node CurrentNode = a_EndNode;

        while (CurrentNode != a_StartingNode)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }

        FinalPath.Reverse();

        grid.FinalPath = FinalPath;
    }

    private int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.gridX - a_nodeB.gridX);
        int iy = Mathf.Abs(a_nodeA.gridY - a_nodeB.gridY);

        return ix + iy;
    }
}
