using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG_Draw
{
    public class BallActive : MonoBehaviour, ISubcriber
    {
        public List<Rigidbody2D> rigidbody2Ds = new List<Rigidbody2D>();

        private void OnEnable()
        {
            Publisher.Instant.Register(this, EnumObsever.Default);
        }
        private void OnDisable()
        {
            if (Publisher.Instant == null) return;
            Publisher.Instant.UnRegister(this, EnumObsever.Default);
        }

        public void Active()
        {
            foreach(var item in rigidbody2Ds)
            {
                item.gravityScale = 1;
            }
        }
    }
}

