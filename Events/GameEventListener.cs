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
    /// <summary>
    /// A Unity Event and the Game Event that should Invoke it.
    /// </summary>
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

        /// <summary>
        /// Called when the Game Object becomes active.
        /// Registers all of the PairedEventListeners.
        /// </summary>
        protected virtual void OnEnable()
        {
            foreach (PairedEventListener pairedListener in Events)
            {
                pairedListener.gameEvent.RegisterListener(this);
            }
        }

        /// <summary>
        /// Called when the Game Object becomes inactive. Unregisters all of the PairedListeners.
        /// Prevents null references.
        /// </summary>
        protected virtual void OnDisable()
        {
            foreach (PairedEventListener pairedListener in Events)
            {
                pairedListener.gameEvent.UnregisterListener(this);
            }
        }

        /// <summary>
        /// Called when a registered GameEvent is raised.
        /// Invokes the UnityEvent that is paired with that GameEvent.
        /// </summary>
        /// <param name="gameEvent">The GameEvent that was raised</param>
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

        /// <summary>
        /// Invokes a UnityEvent by index.
        /// </summary>
        /// <param name="index">The index of the PairedEventListener to invoke</param>
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