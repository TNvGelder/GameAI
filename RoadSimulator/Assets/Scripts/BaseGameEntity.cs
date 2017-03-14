using System;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class BaseGameEntity 
    {
        public GameObject GameObject;
        public int ID { get; set; }
        public Vector2D Pos { get; set; }
        public float BRadius { get; set; }
        public bool Tagged { get; internal set; }
        private Vector2D size;
        public Vector2D Size { get {
                return size;
        } }
        public World MyWorld { get; set; }
        private GUIStyle LabelStyle { get; set; }

        public BaseGameEntity(GameObject gameObject, Vector2D pos, Vector2D size)
        {
            ID = new System.Random().Next(0, 99999);
            GameObject = gameObject;
            Pos = pos;
            MyWorld = World.Instance;
            this.size = size;
            LabelStyle = new GUIStyle();
            LabelStyle.alignment = TextAnchor.UpperLeft;
        }

        public abstract void Update(float delta);
        public virtual void OnGUI() {
            if (World.Instance.DisplayIDs) RenderID();
        }

        private void RenderID()
        {
            var screenPos = Camera.main.WorldToScreenPoint(Pos.ToVector2());
            var labelRect = new Rect(screenPos.x, Screen.height - screenPos.y - 10, 100, 50);
            GUI.Label(labelRect, UI.ColorizeText(ID.ToString(), "white"), LabelStyle);
        }
    }
}
