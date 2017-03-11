using UnityEngine;
namespace Assets.Scripts
{
    public class Car : MovingEntity
    {
        public Car(GameObject obj, Vector2D pos, Vector2D size, PathPlanner pathPlanner) : base(obj, pos, size , pathPlanner)
        {
            MaxSpeed = 3;
        }
    }
}
