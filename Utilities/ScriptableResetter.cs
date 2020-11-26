#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using ColdironTools.Scriptables;

[InitializeOnLoad]
public static class ScriptableResetter
{
    static ScriptableResetter()
    {
        EditorApplication.playModeStateChanged += LogPlayModeState;
    }

    private static void LogPlayModeState(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            InitializeScriptables();
        }

        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            ResetScriptables();
        }
    }

    public static void InitializeScriptables()
    {
        foreach (IntScriptable item in GetAllInstances<IntScriptable>())
        {
            item.Init();
        }

        foreach (FloatScriptable item in GetAllInstances<FloatScriptable>())
        {
            item.Init();
        }

        foreach (BoolScriptable item in GetAllInstances<BoolScriptable>())
        {
            item.Init();
        }

        foreach (StringScriptable item in GetAllInstances<StringScriptable>())
        {
            item.Init();
        }

        foreach (ColorScriptable item in GetAllInstances<ColorScriptable>())
        {
            item.Init();
        }

        foreach (DateScriptable item in GetAllInstances<DateScriptable>())
        {
            item.Init();
        }
    }

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

        foreach (DateScriptable item in GetAllInstances<DateScriptable>())
        {
            item.Reset();
        }
    }

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
}

public class PreBuildReset : UnityEditor.Build.IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
    {
        ScriptableResetter.ResetScriptables();
    }
}
#endif