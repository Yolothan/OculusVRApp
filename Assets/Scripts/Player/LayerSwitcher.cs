using UnityEngine;

public class LayerSwitcher : MonoBehaviour
{
    public string targetLayer = "Persistent";
    private string originalLayer = string.Empty;

    private void Awake()
    {
        originalLayer = LayerMask.LayerToName(gameObject.layer);
    }

    

    private void SwitchToLoadLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(targetLayer);
    }

    private void ResetLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(originalLayer);
    }
}
