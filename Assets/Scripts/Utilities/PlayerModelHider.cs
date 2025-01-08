using UnityEngine;

public class PlayerModelHider : MonoBehaviour
{
    public string localLayer;
    public string nonLocalLayer;
    public GameObject[] meshes;

    public void SetToLocal()
    {
        foreach (var m in meshes)
        {
            m.gameObject.layer = LayerMask.NameToLayer(localLayer);
        }
    }

    public void SetToNonLocal()
    {
        foreach (var m in meshes)
        {
            m.gameObject.layer = LayerMask.NameToLayer(nonLocalLayer);
        }
    }
}
