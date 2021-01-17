// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Modified: Coldiron
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ColdironTools.Events
{
    /// <summary>
    /// A ScriptableObject that designers and developers can use to call functions without dependencies.
    /// </summary>
    [CreateAssetMenu]
    public class GameEvent : ScriptableObject
    {
        #region Fields
        private List<GameEventListener> eventListeners = new List<GameEventListener>();
        private event Action eventActions;
        #endregion

        #region Methods
        /// <summary>
        /// Calls all of the registered GameEventListeners' OnEventRaisedFunction.
        /// Calls all of the registered Actions.
        /// </summary>
        public virtual void Raise()
        {
            for(int i = eventListeners.Count -1; i >= 0; i--)
                eventListeners[i].OnEventRaised(this);

            eventActions?.Invoke();
        }

        /// <summary>
        /// Registers a GameEventListener. As a rule of thumb, this is used by designers rather than developers.
        /// </summary>
        /// <param name="listener">The GameEventListener to register</param>
        public virtual void RegisterListener(GameEventListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        /// <summary>
        /// Unregisters a GameEventListener.
        /// </summary>
        /// <param name="listener">The GameEventListener to unregister</param>
        public virtual void UnregisterListener(GameEventListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }

        /// <summary>
        /// Registers an Action. As a rule of thumb, this is used by developers rather than designers.
        /// </summary>
        /// <param name="action">The Action to register</param>
        public virtual void RegisterAction(Action action)
        {
            eventActions += action;
        }

        /// <summary>
        /// Unregisters an Action.
        /// </summary>
        /// <param name="action">The Action to unregister</param>
        public virtual void UnregisterAction(Action action)
        {
            eventActions -= action;
        }
        #endregion
    }
}