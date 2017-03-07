﻿using Assets.Scripts;
using DataStructures.GraphStructure;
using System.Linq;
using UnityEngine;
using System;
using System.Collections.Generic;

public class GraphGenerator : MonoBehaviour
{
    public Graph<GameObject> Graph = new Graph<GameObject>();
    public GUIStyle NodeGUIStyle;
    public bool Display = false;
    public Color Color = new Color(255, 255, 255);
    public Material mat;

    void Start()
    {
        Texture2D tex = new Texture2D(2, 2);
        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                tex.SetPixel(i, j, Color);
            }
        }
        tex.Apply();
        NodeGUIStyle = new GUIStyle();
        NodeGUIStyle.normal.background = tex;

        GenerateGraph();
    }

    private void GenerateGraph()
    {
        var start = GameObject.FindGameObjectsWithTag("Roads")[0];

        find_adjacent_node(start);
    }

    void find_adjacent_node(GameObject obj)
    {
        var sp = obj.GetComponent<SpriteRenderer>().sprite;
        var width = sp.bounds.size.x;
        var height = sp.bounds.size.y;
        var x = obj.transform.position.x;
        var y = obj.transform.position.y;
        var spacing = 2;
        var widthInterval = spacing + width / 2;
        var heightInterval = spacing + height / 2;


        // right
        CheckAddNode(obj, x + widthInterval, y);

        // left
        CheckAddNode(obj, x - widthInterval, y);

        // above
        CheckAddNode(obj, x, y + heightInterval);

        // under
        CheckAddNode(obj, x, y - heightInterval);
    }

    private void CheckAddNode(GameObject source, float x, float y)
    {
        GameObject adjacentRoad = GetRoad(x, y);

        if (adjacentRoad != null)
        {
            if (PlaceRoadNode(adjacentRoad, source))
                find_adjacent_node(adjacentRoad);
        }
    }

    private bool DoesRoadHaveNode(GameObject obj)
    {
        return Graph.Nodes.Any(n => n.Key.transform.position.x == obj.transform.position.x && n.Key.transform.position.y == obj.transform.position.y);
    }

    public bool HasEdge(GameObject a, GameObject b)
    {
        return _hasEdge(a, b) || _hasEdge(b, a);
    }

    private bool _hasEdge(GameObject a, GameObject b)
    {
        return Graph.Nodes.Any(edge => edge.Key == a && edge.Value.Adjacent.Any(x => x.Destination.Value == b));
    }

    private bool PlaceRoadNode(GameObject to, GameObject from)
    {
        if (HasEdge(from, to))
            return false;

        Graph.AddEdge(to, from, CalculateRoadCost(to, from));

        return true;
    }

    private float CalculateRoadCost(GameObject to, GameObject from)
    {
        var v1 = new Vector2D(to.transform.position.x, to.transform.position.y);
        var v2 = new Vector2D(from.transform.position.x, from.transform.position.y);

        var diff = (v2 - v1);

        return diff.Length();
    }

    public GameObject GetRoad(float x, float y)
    {
        foreach (var child in GameObject.FindGameObjectsWithTag("Roads"))
        {
            Vector3[] array = SpriteLocalToWorld(child);

            if (array[0].x <= x && array[1].x >= x &&
                array[0].y <= y && array[1].y >= y)
            {
                return child;
            }
        }

        return null;
    }

    private Vector3[] SpriteLocalToWorld(GameObject obj)
    {
        Sprite sp = obj.GetComponent<SpriteRenderer>().sprite;
        Vector3 pos = obj.transform.position;
        Vector3[] array = new Vector3[2];
        //top left
        array[0] = pos + sp.bounds.min;
        // Bottom right
        array[1] = pos + sp.bounds.max;
        return array;
    }

    private void Update()
    {
        if (Input.GetKeyDown("g"))
        {
            Display = !Display;
        }
    }

    private void OnGUI()
    {
        if (!Display) return;

        foreach (var node in Graph.Nodes)
        {
            // draw nodes
            var pos = new Vector2D(node.Key.transform.position.x, node.Key.transform.position.y);
            var screenPos = Camera.main.WorldToScreenPoint(pos.ToVector2());

            var size = new Vector2D(15, 15);

            var guiPosition = new Vector2D(screenPos.x, Screen.height - screenPos.y);
            guiPosition = guiPosition - new Vector2D(size.X / 2, size.Y / 2);

            GUILayout.BeginArea(new Rect(guiPosition.ToVector2(), size.ToVector2()), NodeGUIStyle);
            GUILayout.EndArea();

            // draw edges
            GL.PushMatrix();
            mat.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(Color);
            foreach (var edge in node.Value.Adjacent)
            {
                GL.Vertex(node.Key.transform.position);
                GL.Vertex(edge.Destination.Value.transform.position);
            }
            GL.End();
            GL.PopMatrix();
        }
    }
}