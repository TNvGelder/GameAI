using UnityEngine;

namespace Assets.Scripts
{
    public abstract class BaseGameEntity 
    {
        public GameObject GameObject;
        public Vector2D Pos { get; set; }
        private Vector2D size;
        public Vector2D Size { get {
                return size;
        } }
        public World MyWorld { get; set; }

        public BaseGameEntity(GameObject gameObject, Vector2D pos, Vector2D size)
        {
            GameObject = gameObject;
            Pos = pos;
            MyWorld = World.Instance;
            this.size = size;
        }

        public abstract void Update(float delta);

    }
}
