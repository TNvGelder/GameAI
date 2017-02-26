using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.Scripts
{
    public class Car : MovingEntity
    {
        public Car(GameObject obj, Vector2D pos, Vector2D size, World w) : base(obj, pos, size, w)
        {
            MaxSpeed = 10;
        }
    }
}
