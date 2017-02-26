﻿using Assets.Scripts;
using Assets.Scripts.SteeringBehaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    private List<MovingEntity> entities = new List<MovingEntity>();
    private List<Car> cars = new List<Car>();
    public List<GameObject> CarObjects;
    public float Width { get; set; }
    public float Height { get; set; }
    public Car Target;

    // Use this for initialization
    void Start () {
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        //Makes it so that the worldsize is the size of the camera
        Height = cam.orthographicSize;
        Width = Height * Screen.width / Screen.height;
        GameObject carGameObjects = GameObject.Find("Cars");
        foreach(Transform child in carGameObjects.transform)//Gets direct children of cars
        {
            GameObject obj = child.gameObject;
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
            
            //Gets size of the sprite in units
            Vector2D size = new Vector2D(spriteRenderer.bounds.size/ spriteRenderer.sprite.pixelsPerUnit);
            Vector2D pos = new Vector2D(child.position);
            Car car = new Car(obj, pos, size, this);
            cars.Add(car);
            entities.Add(car);
        }
        if (cars.Count > 1)
        {
            Target = cars[0];
            Target.SB = new FleeBehaviour(Target, cars[1]);
            for (int i = 1; i < cars.Count; i++)
            {
                Car car = cars[i];
                car.SB = new SeekBehaviour(car, Target);
            }
        }
    }
	
	void FixedUpdate () {
        float timeElapsed = Time.fixedDeltaTime;
        foreach (MovingEntity me in entities)
        {
            me.Update(timeElapsed);
        }
    }
}