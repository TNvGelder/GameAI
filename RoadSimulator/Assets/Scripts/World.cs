using Assets.Scripts;
using Assets.Scripts.SteeringBehaviours;
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

    public Car Target;

    // Use this for initialization
    void Start () {
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        //Makes it so that the worldsize is the size of the camera
        Height = cam.orthographicSize *2;
        Width = Height * Screen.width / Screen.height;
        MinDetectionBoxLength = 5;

        GameObject carGameObjects = GameObject.Find("Cars");
        foreach(Transform child in carGameObjects.transform)//Gets direct children of cars
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
