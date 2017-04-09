using System;
using UnityEngine;
namespace Assets.Scripts
{
    public class Car : MovingEntity
    {
        public GUIStyle GoalListStyle { get; private set; }
        

        public Car(GameObject obj, Vector2D pos, Vector2D size, PathPlanner pathPlanner) : base(obj, pos, size , pathPlanner)
        {
            BRadius = Size.Y;
            GoalListStyle = new GUIStyle();
            GoalListStyle.alignment = TextAnchor.UpperLeft;
            MaxSpeed = 3;
        }

        public override void OnGUI()
        {
            if (World.Instance.DisplayStats) RenderStats();
            if (World.Instance.DisplayGoals) RenderGoals();

            base.OnGUI();
        }

        private void RenderGoals()
        {
            var screenPos = Camera.main.WorldToScreenPoint(Pos.ToVector2());
            var text = Think.GetDisplayText();
            var labelRect = new Rect(screenPos.x, Screen.height - screenPos.y, 100, 50);
            GUI.Label(labelRect, text, GoalListStyle);
        }

        public void RenderStats()
        {
            var screenPos = Camera.main.WorldToScreenPoint(Pos.ToVector2());
            var color = Fuel > 30f ? "white" : "red";
            var text = UI.ColorizeText("Fuel : " + Math.Round(Fuel, 2).ToString(), color);
            var labelRect = new Rect(screenPos.x - 80, Screen.height - screenPos.y - 30, 100, 50);
            GUI.Label(labelRect, text, GoalListStyle);
        }
    }
}
