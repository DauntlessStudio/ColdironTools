// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using ColdironTools.Scriptables;

namespace ColdironTools.Utilities
{
    /// <summary>
    /// Editor only script that resets the scriptable variables automatically when exiting Play Mode.
    /// </summary>
    [InitializeOnLoad]
    public static class ScriptableResetter
    {
        #region Methods
        /// <summary>
        /// Registers listener in the constructor.
        /// </summary>
        static ScriptableResetter()
        {
            EditorApplication.playModeStateChanged += CheckPlayModeState;
        }

        /// <summary>
        /// Calls ResetScriptables when exiting play mode.
        /// </summary>
        /// <param name="state"></param>
        private static void CheckPlayModeState(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                ResetScriptables();
            }
        }

        /// <summary>
        /// Calls Reset on all scriptable variables.
        /// </summary>
        public static void ResetScriptables()
        {
            foreach (IntScriptable item in GetAllInstances<IntScriptable>())
            {
                item.Reset();
            }

            foreach (FloatScriptable item in GetAllInstances<FloatScriptable>())
            {
                item.Reset();
            }

            foreach (BoolScriptable item in GetAllInstances<BoolScriptable>())
            {
                item.Reset();
            }

            foreach (StringScriptable item in GetAllInstances<StringScriptable>())
            {
                item.Reset();
            }

            foreach (ColorScriptable item in GetAllInstances<ColorScriptable>())
            {
                item.Reset();
            }
        }

        /// <summary>
        /// Gets all the instances of a given scriptable object.
        /// </summary>
        /// <typeparam name="T">Scriptable object type</typeparam>
        /// <returns>An array of all scriptable objects of type T.</returns>
        private static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            T[] a = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a;
        }
        #endregion
    }

    /// <summary>
    /// Resets scriptable variables before creating a build.
    /// </summary>
    public class PreBuildReset : UnityEditor.Build.IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
        {
            ScriptableResetter.ResetScriptables();
        }
    }
}
#endif