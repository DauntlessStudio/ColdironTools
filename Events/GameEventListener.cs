// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Modified: Coldiron
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace ColdironTools.Events
{
    [System.Serializable]
    public struct PairedEventListener
    {
        public GameEvent gameEvent;
        public UnityEvent response;
    }

    public class GameEventListener : MonoBehaviour
    {
        #region Fields
        [SerializeField] private List<PairedEventListener>  Events = new List<PairedEventListener>();
        #endregion

        #region Methods
        protected virtual void OnEnable()
        {
            foreach (PairedEventListener pairedListener in Events)
            {
                pairedListener.gameEvent.RegisterListener(this);
            }
        }

        protected virtual void OnDisable()
        {
            foreach (PairedEventListener pairedListener in Events)
            {
                pairedListener.gameEvent.UnregisterListener(this);
            }
        }

        public virtual void OnEventRaised(GameEvent gameEvent)
        {
            foreach (PairedEventListener listenEvent in Events)
            {
                if (gameEvent == listenEvent.gameEvent)
                {
                    listenEvent.response.Invoke();
                }
            }
        }

        public virtual void RaiseEventByIndex(int index)
        {
            if (Events.Count >= index+1)
            {
                Events[index].response.Invoke();
            }
        }
        #endregion
    }
}