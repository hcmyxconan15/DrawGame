using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
namespace MG_Draw
{
    
    public class Publisher : Singleton<Publisher>
    {
        public Dictionary<string, Action> subcribers = new Dictionary<string, Action>();
        public void Register(ISubcriber subcriber, EnumObsever type)
        {
            string name = type.ToString();
            if(!subcribers.ContainsKey(name))
            {
                subcribers.Add(name, subcriber.Active);
            }
            else
            {
                subcribers[name] += subcriber.Active;
            }

        }
        public void UnRegister(ISubcriber subcriber, EnumObsever type)
        {
            name = type.ToString();
            if (!subcribers.ContainsKey(name)) return;
            subcribers[name] -= subcriber.Active;
        }

        public void Broadcast(EnumObsever type)
        {
            string name = type.ToString();
            if (!subcribers.ContainsKey(name)) return;
            subcribers[name]?.Invoke();
        }
    }
}

