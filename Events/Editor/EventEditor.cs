// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Modified: Caleb Coldiron
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

namespace ColdironTools.Events
{
    /// <summary>
    /// Custom editor for Game Events.
    /// </summary>
    [CustomEditor(typeof(GameEvent))]
    public class EventEditor : Editor
    {
        /// <summary>
        /// Adds a button to the Inspector Panel when the editor is in Play Mode.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            GameEvent e = target as GameEvent;
            if (GUILayout.Button("Raise"))
                e.Raise();
        }
    }
}