using Assets.Scripts;
using Assets.Scripts.SteeringBehaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using DataStructures.GraphStructure;
using Assets.Scripts.DataStructures.GraphStructure;
using System.Linq;

public class World : MonoBehaviour {

    public List<MovingEntity> entities = new List<MovingEntity>();
    public List<Car> cars = new List<Car>();
    public List<GameObject> CarObjects;
    public Grid Grid = new Grid();
    public float Width { get; set; }
    public float Height { get; set; }
    public int MinDetectionBoxLength { get; internal set; }
    Graph<GameNode> graph = new Graph<GameNode>();
    public bool ShouldRenderGrid = true;
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
        var start = GameObject.FindGameObjectsWithTag("Roads")[0].transform;

        find_adjacent_node(start.position.x, start.position.y);
    }


    float interval = 5.0f;
    void find_adjacent_node(float x, float y)
    {
        //if (x < -50 || x > 50) return;

        if(!IsNode(x + interval, y) && IsRoad(x + interval, y))
        {
            PlaceRoadNode(x + interval, y);
            find_adjacent_node(x + interval, y);
        }

        if (!IsNode(x - interval, y) && IsRoad(x - interval, y))
        {
            PlaceRoadNode(x - interval, y);
            find_adjacent_node(x - interval, y);
        }


        if (!IsNode(x, y + interval) && IsRoad(x, y + interval))
        {
            PlaceRoadNode(x, y + interval);
            find_adjacent_node(x, y + interval);
        }

        if (!IsNode(x, y - interval) && IsRoad(x, y - interval))
        {
            PlaceRoadNode(x, y - interval);
            find_adjacent_node(x, y - interval);
        }
    }

    private bool IsNode(float x, float y)
    {
        return graph.Nodes.Any(n => n.Key.x == x && n.Key.y == y);
    }

    private void PlaceRoadNode(float x, float y)
    {
        graph.AddNode(new GameNode()
        {
            IsRoad = true,
            x = x,
            y = y
        });
    }

    bool IsRoad(float x, float y)
    {
        foreach (var child in GameObject.FindGameObjectsWithTag("Roads"))
        {
            Vector3[] array = SpriteLocalToWorld(child);

            //var x1 = child.transform.position.x;
            //var y1 = child.transform.position.y;
            //var x2 = 
            //var p1 = Camera.main.WorldToScreenPoint(array[0]);
            //var p2 = Camera.main.WorldToScreenPoint(array[1]);

            if (array[0].x <= x && array[1].x >= x &&
                array[0].y <= y && array[1].y >= y)
            {
                return true;
            }
        }

        return false;
    }

    private void OnGUI()
    {
        RenderNodes();

        //if (ShouldRenderGrid)
        //{
        //    RenderGrid();
        //}
    }

    //private void RenderGrid()
    //{
    //    foreach (var cell in Grid.Cells)
    //    {
    //        cell.Render();
    //    }
    //}

    //private GameObjectType GetCellType(GridCell cell)
    //{
    //    foreach (var child in GameObject.FindGameObjectsWithTag("Roads"))
    //    {
    //        Vector3[] array = SpriteLocalToWorld(child);

    //        var p1 = Camera.main.WorldToScreenPoint(array[0]);
    //        var p2 = Camera.main.WorldToScreenPoint(array[1]);

    //        if (p1.x <= cell.x && p2.x >= cell.x + 20 &&
    //            p1.y <= cell.y && p2.y >= cell.y + 20)
    //        {
    //            return GameObjectType.Road;
    //        }
    //    }

    //    return GameObjectType.Grass;
    //}

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
            var pos = new Vector2D(node.Key.x, node.Key.y);
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

        if (Input.GetKeyDown("g"))
        {
            ShouldRenderGrid = !ShouldRenderGrid;
        }

        float timeElapsed = Time.fixedDeltaTime;
        foreach (MovingEntity me in entities)
        {
            me.Update(timeElapsed);
        }
    }
}
