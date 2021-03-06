using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
public class SelectableListAttribute : PropertyAttribute
{
    public string list;
    public string index;

    public SelectableListAttribute(string selectedList, string selectedIndex)
    {
        list = selectedList;
        index = selectedIndex;
    }
}
