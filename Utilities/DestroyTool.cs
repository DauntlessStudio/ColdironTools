// ------------------------------
// Coldiron Tools
// Author: Caleb Coldiron
// Version: 1.0, 2021
// ------------------------------

using UnityEngine;

/// <summary>
/// Simple script to allow objects to be destroyed via Unity Events.
/// </summary>
public class DestroyTool : MonoBehaviour
{
    /// <summary>
    /// Destroys a game object reference.
    /// </summary>
    /// <param name="go">The game object to be destroyed.</param>
    public void DestroyObject(GameObject go)
    {
        Destroy(go);
    }

    /// <summary>
    /// Destroys a child of the object.
    /// </summary>
    /// <param name="childIndex">The index of the child to be destroyed.</param>
    public void DestroyChild(int childIndex)
    {
        GameObject go = transform.GetChild(childIndex).gameObject;
        if(go) Destroy(go);
    }
}
