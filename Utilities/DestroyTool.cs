using UnityEngine;

public class DestroyTool : MonoBehaviour
{
    public void DestroyObject(GameObject go)
    {
        Destroy(go);
    }

    public void DestroyChild(int childIndex)
    {
        GameObject go = transform.GetChild(childIndex).gameObject;
        if(go) Destroy(go);
    }
}
