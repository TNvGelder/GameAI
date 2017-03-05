using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public enum GameObjectType
    {
        Shop,
        Road,
        Grass
    };

    public class GridCell
    {
        public float x { get; set; }
        public float y { get; set; }
        public static int Width = 20;
        public static int Height = 20;

        public GameObjectType Type { get; set; }

        public GridCell()
        {
            Type = GameObjectType.Grass;
        }

        internal void Render()
        {
            Color color = new Color();

            switch(Type)
            {
                case GameObjectType.Grass:
                    color = new Color(0, 255, 0, 0.3f);
                    break;
                case GameObjectType.Shop:
                    color = new Color(255, 0, 0, 0.3f);
                    break;
                case GameObjectType.Road:
                    color = new Color(0, 0, 0, 0.3f);
                    break;
            }

            var texture = new Texture2D(Width, Height);
            for (int y = 0; y < texture.height; ++y)
            {
                for (int x = 0; x < texture.width; ++x)
                {
                    texture.SetPixel(x, y, color);
                }
            }
            texture.Apply();

            GUI.Box(new Rect(new Vector2(x, y), new Vector2(Width, Height)), texture);
        }
    }
}
