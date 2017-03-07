using Assets.Scripts;
using Assets.Scripts.SteeringBehaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using DataStructures.GraphStructure;
using System.Linq;

public class World : MonoBehaviour {

    public List<MovingEntity> entities = new List<MovingEntity>();
    public List<Car> cars = new List<Car>();
    public List<GameObject> CarObjects;
    public float Width { get; set; }
    public float Height { get; set; }
    public int MinDetectionBoxLength { get; internal set; }
    Graph<GameObject> graph = new Graph<GameObject>();
    public GUIStyle NodeGUIStyle = new GUIStyle();

    public Car Target;



    // Use this for initialization
    void Start () {
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        //Makes it so that the worldsize is the size of the camera
        Height = cam.orthographicSize *2;
        Width = Height * Screen.width / Screen.height;
        MinDetectionBoxLength = 5;
        
        Texture2D tex = new Texture2D(2, 2);
        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                tex.SetPixel(i, j, new Color(255, 255, 255));
            }
        }
        tex.Apply();
        NodeGUIStyle = new GUIStyle();
        NodeGUIStyle.normal.background = tex;

        GenerateGraph();
    }

    private void SetupCars()
    {
        GameObject carGameObjects = GameObject.Find("Cars");
        foreach (Transform child in carGameObjects.transform)//Gets direct children of cars
        {
            GameObject obj = child.gameObject;
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

            //Gets size of the sprite in units
            Vector2D size = new Vector2D(spriteRenderer.bounds.size);
            Vector2D pos = new Vector2D(child.position);
            Car car = new Car(obj, pos, size, this);
            car.BRadius = car.Size.Y;
            cars.Add(car);
            entities.Add(car);
            car.SteeringBehaviours.Add(new ObstacleAvoidanceBehavior(car));
        }
        if (cars.Count > 1)
        {
            Target = cars[0];
            Target.SteeringBehaviours.Add(new FleeBehaviour(Target, cars[1]));

            for (int i = 1; i < cars.Count; i++)
            {
                Car car = cars[i];
                car.SteeringBehaviours.Add(new SeekBehaviour(car, Target));
            }
        }

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
        var spacing = 2;
        var widthInterval = spacing + width / 2;
        var heightInterval = spacing + height / 2;
        var x = obj.transform.position.x;
        var y = obj.transform.position.y;

        GameObject adjacentRoad = null;

        // right
        adjacentRoad = GetRoad(x + widthInterval, y);
        if (adjacentRoad != null && !HasNode(adjacentRoad))
        {
            PlaceRoadNode(adjacentRoad, obj);
            find_adjacent_node(adjacentRoad);
        }

        // left
        adjacentRoad = GetRoad(x - widthInterval, y);
        if (adjacentRoad != null && !HasNode(adjacentRoad))
        {
            PlaceRoadNode(adjacentRoad, obj);
            find_adjacent_node(adjacentRoad);
        }
        
        // above
        adjacentRoad = GetRoad(x, y + heightInterval);
        if (adjacentRoad != null && !HasNode(adjacentRoad))
        {
            PlaceRoadNode(adjacentRoad, obj);
            find_adjacent_node(adjacentRoad);
        }

        // under
        adjacentRoad = GetRoad(x, y - heightInterval);
        if (adjacentRoad != null && !HasNode(adjacentRoad))
        {
            PlaceRoadNode(adjacentRoad, obj);
            find_adjacent_node(adjacentRoad);
        }
    }

    private bool HasNode(GameObject obj)
    {
        return graph.Nodes.Any(n => n.Key.transform.position.x == obj.transform.position.x && n.Key.transform.position.y == obj.transform.position.y);
    }

    private void PlaceRoadNode(GameObject to, GameObject from)
    {
        graph.AddEdge(to, from, CalculateRoadCost(to, from));
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

    private void OnGUI()
    {
        RenderNodes();
    }

    Vector3[] SpriteLocalToWorld(GameObject obj)
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

    private void RenderNodes()
    {
        foreach (var node in graph.Nodes)
        {
            var pos = new Vector2D(node.Key.transform.position.x, node.Key.transform.position.y);
            var screenPos = Camera.main.WorldToScreenPoint(pos.ToVector2());

            var size = new Vector2D(15, 15);

            var guiPosition = new Vector2(screenPos.x, Screen.height - screenPos.y);
            GUILayout.BeginArea(new Rect(guiPosition, size.ToVector2()), NodeGUIStyle);
            GUILayout.EndArea();
        }
    }

    void FixedUpdate () {
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(0);
        }

        float timeElapsed = Time.fixedDeltaTime;
        foreach (MovingEntity me in entities)
        {
            me.Update(timeElapsed);
        }
    }
}
