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
        public static int Width = 10;
        public static int Height = 10;

        public GameObjectType Type { get; set; }

        public Vector2D[] GetVertices()
        {
            return new Vector2D[]
            {
                //new Vector2D(x - Width / 2, y - Height / 2),
                //new Vector2D(x + Width / 2, y - Height / 2),
                //new Vector2D(x + Width / 2, y + Height / 2),
                //new Vector2D(x - Width / 2, y + Height / 2)

                new Vector2D(x, y),
                new Vector2D(x + Width - 5, y),
                new Vector2D(x, y + Height - 5),
                new Vector2D(x + Width - 5, y + Height - 5),

            };
        }

        internal void Render()
        {
            var v = GetVertices();

            //var cellPos = Camera.main.WorldToScreenPoint(new Vector3(v[0].X, v[0].Y, 0));

            //GL.Vertex3(0, 0, -99);
            //GL.Vertex3(10, 10, -99);

            //var V = Camera.main.WorldToScreenPoint(new Vector2(v[0].X, v[0].Y));
            //GL.Vertex3(V.x, V.y, V.z);
            //V = Camera.main.WorldToScreenPoint(new Vector2(v[1].X, v[1].Y));
            //GL.Vertex3(V.x, V.y, V.z);
            //V = Camera.main.WorldToScreenPoint(new Vector2(v[2].X, v[2].Y));
            //GL.Vertex3(V.x, V.y, V.z);
            //V = Camera.main.WorldToScreenPoint(new Vector2(v[3].X, v[3].Y));
            //GL.Vertex3(V.x, V.y, V.z);

            //GL.Vertex3((v[0].X) / Screen.width, (v[0].Y) / Screen.height, -99);
            //GL.Vertex3((v[1].X) / Screen.width, (v[1].Y) / Screen.height, -99);
            //GL.Vertex3((v[2].X) / Screen.width, (v[2].Y) / Screen.height, -99);
            //GL.Vertex3((v[3].X) / Screen.width, (v[3].Y) / Screen.height, -99);

            
            //GL.Vertex3((v[0].X) / Screen.width, (v[0].Y) / Screen.height, -99);
            //GL.Vertex3((v[1].X) / Screen.width, (v[1].Y) / Screen.height, -99);
            //GL.Vertex3((v[2].X) / Screen.width, (v[2].Y) / Screen.height, -99);
            //GL.Vertex3((v[3].X) / Screen.width, (v[3].Y) / Screen.height, -99);


            GUI.Box(new Rect(new Vector2(x, y), new Vector2(Width, Height)), "");

            //GL.Vertex(v[0].ToVector2());
            //GL.Vertex(v[1].ToVector2());
            //GL.Vertex(v[2].ToVector2());
            //GL.Vertex(v[3].ToVector2());

            //GL.Vertex3((v[0].X), (v[0].Y), -99);
            //GL.Vertex3((v[1].X), (v[1].Y), -99);
            //GL.Vertex3((v[2].X), (v[2].Y), -99);
            //GL.Vertex3((v[3].X), (v[3].Y), -99);
        }
    }
}
