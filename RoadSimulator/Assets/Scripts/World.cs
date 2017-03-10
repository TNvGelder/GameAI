using Assets.Scripts;
using Assets.Scripts.SteeringBehaviours;
using DataStructures.GraphStructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class World : MonoBehaviour {

    public List<MovingEntity> entities = new List<MovingEntity>();
    public List<Car> cars = new List<Car>();
    public List<GameObject> CarObjects;
    public float Width { get; set; }
    public float Height { get; set; }
    public int MinDetectionBoxLength { get; internal set; }
    public static World instance;
    public static World Instance { get { return instance; } }

    private Graph<Vector2D> graph;
    public Graph<Vector2D> Graph { get { return graph; } }
    private GraphGenerator graphGenerator;
    public GUIStyle NodeGUIStyle;
    public Color Color = new Color(255, 255, 255);
    public Material mat;

    public Car Target;

    // Use this for initialization
    void Start() {
        instance = this;
        GenerateGraph();
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        //Makes it so that the worldsize is the size of the camera
        Height = cam.orthographicSize * 2;
        Width = Height * Screen.width / Screen.height;
        MinDetectionBoxLength = 5;

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
                car.SteeringBehaviours.Add(new Explore(car));
                //car.SteeringBehaviours.Add(new SeekMovingEntityBehaviour(car, Target));
            }
        }
    }

    private void GenerateGraph()
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
        graphGenerator = new GraphGenerator();
        graph = graphGenerator.Graph;
    }

	void FixedUpdate () {
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown("g"))
        {
            graphGenerator.Display = !graphGenerator.Display;
        }

        float timeElapsed = Time.fixedDeltaTime;
        foreach (MovingEntity me in entities)
        {
            me.Update(timeElapsed);
        }
    }

    private void OnGUI()
    {
        if (!graphGenerator.Display) return;

        foreach (var node in Graph.Nodes)
        {
            // draw nodes
            var pos = node.Key;
            var screenPos = Camera.main.WorldToScreenPoint(pos.ToVector2());

            var size = new Vector2D(15, 15);

            var guiPosition = new Vector2D(screenPos.x, Screen.height - screenPos.y);
            guiPosition = guiPosition - new Vector2D(size.X / 2, size.Y / 2);

            GUILayout.BeginArea(new Rect(guiPosition.ToVector2(), size.ToVector2()), NodeGUIStyle);
            GUILayout.EndArea();

            // draw edges
            GL.PushMatrix();
            //mat.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(Color);
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
}
