// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Modified: Coldiron
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace ColdironTools.Events
{
    [CreateAssetMenu]
    public class GameEvent : ScriptableObject
    {
        #region Fields
        private List<GameEventListener> eventListeners = new List<GameEventListener>();
        public event System.Action eventActions;
        #endregion

        #region Methods
        public void Raise()
        {
            for(int i = eventListeners.Count -1; i >= 0; i--)
                eventListeners[i].OnEventRaised(this);

            eventActions?.Invoke();
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }

        public void RegisterAction(System.Action action)
        {
            eventActions += action;
        }

        public void UnregisterAction(System.Action action)
        {
            eventActions -= action;
        }
        #endregion
    }
}