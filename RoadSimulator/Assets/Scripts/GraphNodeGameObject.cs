using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class GraphNodeGameObject : BaseGameEntity
    {
        public GraphNodeGameObject(GameObject gameObject, Vector2D pos, Vector2D size, World w) : base(gameObject, pos, size, w)
        {
        }

        public override void Update(float delta)
        {
        }
    }
}
