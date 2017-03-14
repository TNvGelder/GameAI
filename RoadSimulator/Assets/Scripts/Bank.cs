using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Bank : Building
    {
        public GUIStyle StatsStyle { get; private set; }
        public int MoneyInBank { get; set; }

        public Bank(GameObject gameObject, Vector2D pos, Vector2D size) : base(gameObject, pos, size)
        {
            StatsStyle = new GUIStyle();
            StatsStyle.alignment = TextAnchor.UpperLeft;
            MoneyInBank = 0;
        }

        internal void AddMoney(int v)
        {
            MoneyInBank += v;
        }

        public override void OnGUI()
        {
            if (World.Instance.DisplayStats) RenderStats();

            base.OnGUI();
        }

        private void RenderStats()
        {
            var screenPos = Camera.main.WorldToScreenPoint(Pos.ToVector2());
            var text = UI.ColorizeText("Money: " + MoneyInBank.ToString(), "white");
            var labelRect = new Rect(screenPos.x - 80, Screen.height - screenPos.y - 30, 100, 50);
            GUI.Label(labelRect, text, StatsStyle);
        }
    }
}
