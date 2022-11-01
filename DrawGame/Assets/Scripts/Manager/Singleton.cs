using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MG_Draw
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instant;

        public static T Instant
        {
            get
            {
                if(instant == null)
                {
                    instant = FindObjectOfType<T>();
                }
                return instant;
            }
        }


    }
}

