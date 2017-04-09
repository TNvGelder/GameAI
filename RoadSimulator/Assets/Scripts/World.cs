using Assets.Scripts;
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
    private Dictionary<Type, IEnumerable<BaseGameEntity>> entityLists = new Dictionary<Type, IEnumerable<BaseGameEntity>>();
    public float Width { get; set; }
    public float Height { get; set; }
    public bool DisplayIDs { get; set; }
    public bool DisplayGraph { get; set; }
    public bool DisplayGoals { get; set; }
    public bool DisplayStats { get; set; }
    public System.Random Random = new System.Random();
    public static World Instance { get; internal set; }

    // graph
    private Graph<Vector2D> graph;
    public Graph<Vector2D> Graph { get { return graph; } }
    private GraphGenerator graphGenerator;

    // styles
    static Material lineMaterial;
    public GUIStyle GraphNodeGUIStyle;
    public Color GraphColor = new Color(255, 255, 255);
    public Color GraphTravelingColor = new Color(255, 0, 0);

    public Car player;

    // Unity calls this function on startup, so we can do initialization logic here
    void Start() {
        Instance = this;

        // Make it so that the worldsize is the size of the camera
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        Height = cam.orthographicSize * 2;
        Width = Height * Screen.width / Screen.height;

        DisplayStats = true;

        GenerateGraph();

        // initialize styles only once at startup, to improve rendering speed significantly
        InitializeStyles();

        CreateEntities();
    }

    private void CreateEntities()
    {
        GameObject carGameObjects = GameObject.Find("Cars");
        foreach (Transform child in carGameObjects.transform)//Gets direct children of cars
        {
            GameObject obj = child.gameObject;
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

            //Gets size of the sprite in units
            Vector2D size = new Vector2D(spriteRenderer.bounds.size);
            Vector2D pos = new Vector2D(child.position);
            Car car = new Car(obj, pos, size, new PathPlanner(graph));
            Entities.Add(car);

            if (player == null)
            {
                player = car;
            }
        }

        CreateEntityForObjects<Bank>(GameObject.FindGameObjectsWithTag("Bank"));
        CreateEntityForObject<Work>(GameObject.Find("Work"));
        CreateEntityForObjects<GasStation>(GameObject.FindGameObjectsWithTag("GasStation"));
    }

    private void CreateEntityForObjects<T>(GameObject[] gameObjects) where T : BaseGameEntity
    {
        foreach(var obj in gameObjects)
        {
            CreateEntityForObject<T>(obj);
        }
    }

    private void CreateEntityForObject<T>(GameObject obj) where T :  BaseGameEntity
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

        Vector2D size = new Vector2D(spriteRenderer.bounds.size);
        Vector2D pos = new Vector2D(obj.transform.position);
        T entity = Activator.CreateInstance(typeof(T),
                  new object[] { obj, pos, size }) as T;
        Entities.Add(entity);
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

        // don't register clicks inside the clickmask.
        // the clickmask is the area behind the checkboxes in the topleft corner of the simulation
        // when clicking checkboxes we don't want the car to move there
        var mask = (RectTransform)GameObject.Find("ClickMask").GetComponent("RectTransform");
        if (!RectTransformUtility.RectangleContainsScreenPoint(mask, endPoint, GetComponent<Camera>()))
        {
            player.Think.SetGoal(new MoveToPositionGoal(player, new Vector2D(endPoint.x, endPoint.y)));
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
        if (Graph == null) return;

        // draw all graph nodes
        foreach (var node in Graph.Nodes)
        {
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
            
            foreach (var edge in node.Value.Adjacent)
            {
                GL.Color(GraphColor);
                Vector2D destPos = edge.Destination.Value;

                // when executing followpathbehavior, show active edges in red
                FollowPathBehaviour behaviour = (FollowPathBehaviour)player.GetBehaviour(typeof(FollowPathBehaviour));
                if (behaviour != null)
                {
                    LinkedList<Vector2D> wayPoints = behaviour.Path.WayPoints;
                    if (wayPoints.Contains(pos) && wayPoints.Contains(destPos))
                    {
                        GL.Color(GraphTravelingColor);
                    }
                }

                GL.Vertex(new Vector2(pos.X, pos.Y));
                GL.Vertex(new Vector2(destPos.X, destPos.Y));
            }

            GL.End();
            GL.PopMatrix();
        }
    }

    public List<MovingEntity> GetMovingEntities()
    {
        return Entities.Where(x => x is MovingEntity).Cast<MovingEntity>().ToList();
    }

    public T GetEntity<T>() where T : BaseGameEntity
    {
        return (T)Entities.FirstOrDefault(x => x is T);
    }

    public T GetEntity<T>(GameObject gameObject) where T : BaseGameEntity
    {
        return (T)GetEntitiesOfType<T>().FirstOrDefault(x => ( x.GameObject.Equals(gameObject)));
    }

    public void TagObstaclesWithinViewRange(MovingEntity entity, double range)
    {
        TagNeighbors(entity, GetMovingEntities(), range);
    }

    public List<T> GetEntitiesOfType<T>() where T: BaseGameEntity
    {
        IEnumerable<BaseGameEntity> entitiesOfType;
        if (entityLists.ContainsKey(typeof(T)))
        {
            entitiesOfType = entityLists[typeof(T)];
        }else
        {
            entitiesOfType = Entities.Where(x => x is T);
            entityLists[typeof(T)] = entitiesOfType;

        }
        return entitiesOfType.Cast<T>().ToList();
    }

    public void TagNeighbors(MovingEntity entity, List<MovingEntity> obstacles, double radius)
    {
        foreach(var e in obstacles)
        {
            e.Tagged = false;

            if (entity.ID.Equals(e.ID)) continue;

            var to = e.Pos - entity.Pos;

            var range = radius + e.BRadius;

            if (to.LengthSquared() < range * range)
            {
                e.Tagged = true;
            }
        }
    }
}
