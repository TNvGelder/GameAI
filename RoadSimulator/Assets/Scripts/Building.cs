using UnityEngine;

namespace Assets.Scripts
{
    public class Building : BaseGameEntity
    {
        public Building(GameObject gameObject, Vector2D pos, Vector2D size) : base(gameObject, pos, size)
        {
        }

        public override void Update(float delta)
        {
            // noop
        }
    }
}
