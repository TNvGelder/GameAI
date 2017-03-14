﻿using Assets.Scripts;
using Assets.Scripts.Goals;
using Assets.Scripts.SteeringBehaviours;
using DataStructures.GraphStructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class World : MonoBehaviour {

    public List<BaseGameEntity> Entities = new List<BaseGameEntity>();
    public List<GameObject> CarObjects;
    public float Width { get; set; }
    public float Height { get; set; }
    public bool DisplayIDs { get; set; }
    public bool DisplayGraph { get; set; }
    public bool DisplayGoals { get; set; }
    public bool DisplayStats { get; set; }
    public System.Random Random = new System.Random();
    public int MinDetectionBoxLength { get; internal set; }
    public static World instance;
    public static World Instance { get { return instance; } }

    private Graph<Vector2D> graph;
    public Graph<Vector2D> Graph { get { return graph; } }
    private GraphGenerator graphGenerator;

    // styles
    static Material lineMaterial;
    public GUIStyle GraphNodeGUIStyle;
    public Color GraphColor = new Color(255, 255, 255);

    public Car player;

    // Use this for initialization
    void Start() {
        instance = this;
        GenerateGraph();
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        //Makes it so that the worldsize is the size of the camera
        Height = cam.orthographicSize * 2;
        Width = Height * Screen.width / Screen.height;
        MinDetectionBoxLength = 5;

        DisplayStats = true;

        InitializeStyles();

        GameObject carGameObjects = GameObject.Find("Cars");
        foreach (Transform child in carGameObjects.transform)//Gets direct children of cars
        {
            GameObject obj = child.gameObject;
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

            //Gets size of the sprite in units
            Vector2D size = new Vector2D(spriteRenderer.bounds.size);
            Vector2D pos = new Vector2D(child.position);
            Car car = new Car(obj, pos, size, new PathPlanner(graph));
            car.BRadius = car.Size.Y;
            Entities.Add(car);

            if (player == null)
            {
                player = car;
            }
        }

        var bankObj = GameObject.Find("Bank");
        var Bank = new Bank(bankObj, new Vector2D(bankObj.transform.position), new Vector2D(bankObj.GetComponent<SpriteRenderer>().bounds.size));
        Entities.Add(Bank);
    }

    private void InitializeStyles()
    {
        // graph
        Texture2D tex = new Texture2D(2, 2);
        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                tex.SetPixel(i, j, GraphColor);
            }
        }
        tex.Apply();
        GraphNodeGUIStyle = new GUIStyle();
        GraphNodeGUIStyle.normal.background = tex;

        // goal list

        // tag
        tex = new Texture2D(2, 2);
        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                tex.SetPixel(i, j, new Color(255, 255, 0));
            }
        }
        tex.Apply();

        // lines
        // Unity has a built-in shader that is useful for drawing
        // simple colored things.
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        lineMaterial = new Material(shader);
        lineMaterial.hideFlags = HideFlags.HideAndDontSave;
        // Turn on alpha blending
        lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        // Turn backface culling off
        lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        // Turn off depth writes
        lineMaterial.SetInt("_ZWrite", 0);
    }

    private void GenerateGraph()
    {
        graphGenerator = new GraphGenerator();
        graph = graphGenerator.Graph;
    }

	void FixedUpdate () {
        if (Input.GetKeyDown("r"))
        {
            Reload();
        }
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }

        float timeElapsed = Time.fixedDeltaTime;
        foreach (MovingEntity me in GetMovingEntities())
        {
            me.Update(timeElapsed);
        }
    }

    private void Reload()
    {
        SceneManager.LoadScene(0);
    }

    private void HandleClick()
    {
        Vector3 endPoint = Input.mousePosition;
        endPoint.z = 0f;
        endPoint = Camera.main.ScreenToWorldPoint(endPoint);

        var mask = (SpriteRenderer)GameObject.Find("ClickMask").GetComponent("SpriteRenderer");
        if (!mask.bounds.Contains(endPoint))
        {
            ((Think)player.Think).AddMoveToPosition(new Vector2D(endPoint.x, endPoint.y));
        }
    }

    private void OnGUI()
    {
        if (DisplayGraph) RenderGraph();

        foreach(var entity in Entities)
        {
            entity.OnGUI();
        }
    }

    public void RenderGraph()
    {
        foreach (var node in Graph.Nodes)
        {
            // draw nodes
            var pos = node.Key;
            var screenPos = Camera.main.WorldToScreenPoint(pos.ToVector2());

            var size = new Vector2D(15, 15);

            var guiPosition = new Vector2D(screenPos.x, Screen.height - screenPos.y);
            guiPosition = guiPosition - new Vector2D(size.X / 2, size.Y / 2);

            GUILayout.BeginArea(new Rect(guiPosition.ToVector2(), size.ToVector2()), GraphNodeGUIStyle);
            GUILayout.EndArea();

            // Apply the line material
            lineMaterial.SetPass(0);
            // draw edges
            GL.PushMatrix();
            GL.Begin(GL.LINES);
            GL.Color(GraphColor);
            foreach (var edge in node.Value.Adjacent)
            {
                GL.Vertex(new Vector2(pos.X, pos.Y));
                Vector2D destPos = edge.Destination.Value;
                GL.Vertex(new Vector2(destPos.X, destPos.Y));
            }
            GL.End();
            GL.PopMatrix();
        }
    }

    public List<MovingEntity> GetMovingEntities()
    {
        //var movingEntities = Entities.Where(x => x.GetType() == typeof(MovingEntity));
        return Entities.Where(x => x is MovingEntity).Cast<MovingEntity>().ToList<MovingEntity>();
    }

    public T GetEntity<T>() where T : BaseGameEntity
    {
        return (T)Entities.FirstOrDefault(x => x is T);
    }

    public void TagObstaclesWithinViewRange(MovingEntity entity, double range)
    {
        TagNeighbors(entity, GetMovingEntities(), range);
    }

    public void TagNeighbors(MovingEntity entity, List<MovingEntity> obstacles, double radius)
    {
        foreach(var e in obstacles)
        {
            if (entity.Equals(e)) continue;

            e.Tagged = false;

            var to = e.Pos - entity.Pos;

            var range = radius + e.BRadius;

            if (to.LengthSquared() < range * range)
            {
                e.Tagged = true;
            }
        }
    }
}
