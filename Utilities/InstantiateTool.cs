using UnityEngine;

public class InstantiateTool : MonoBehaviour
{
    [SerializeField] GameObject parentObject;

    private void OnValidate()
    {
        if (!parentObject)
        {
            parentObject = gameObject;
        }
    }

    public void InstantiateAsChild(GameObject prefab)
    {
        Instantiate(prefab, parentObject.transform);
    }
}
