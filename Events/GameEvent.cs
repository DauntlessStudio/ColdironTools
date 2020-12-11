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
        private event System.Action eventActions;
        private event System.Action<GameEvent> eventActionsWithParam;
        #endregion

        #region Methods
        public virtual void Raise()
        {
            for(int i = eventListeners.Count -1; i >= 0; i--)
                eventListeners[i].OnEventRaised(this);

            eventActions?.Invoke();
            eventActionsWithParam?.Invoke(this);
        }

        public virtual void RegisterListener(GameEventListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public virtual void UnregisterListener(GameEventListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }

        public virtual void RegisterAction(System.Action action)
        {
            eventActions += action;
        }

        public virtual void UnregisterAction(System.Action action)
        {
            eventActions -= action;
        }

        public virtual void RegisterAction(System.Action<GameEvent> action)
        {
            eventActionsWithParam += action;
        }

        public virtual void UnregisterAction(System.Action<GameEvent> action)
        {
            eventActionsWithParam -= action;
        }
        #endregion
    }
}