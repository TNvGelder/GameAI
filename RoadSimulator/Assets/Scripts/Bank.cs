using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Bank : Building
    {
        public int MoneyInBank { get; set; }

        public Bank(GameObject gameObject, Vector2D pos, Vector2D size) : base(gameObject, pos, size)
        {
            MoneyInBank = 0;
        }

        internal void AddMoney(int v)
        {
            MoneyInBank += v;
        }
    }
}
