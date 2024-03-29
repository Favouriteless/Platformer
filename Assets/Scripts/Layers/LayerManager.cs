using UnityEngine;

public class LayerManager : MonoBehaviour
{
    public static LayerManager INSTANCE;

    public LayerZone[] zones;

    public AbstractLayer ActiveLayer { get; }
    private AbstractLayer activeLayer = Layers.NORMAL;

    private void Awake()
    {
        INSTANCE = this;
    }

    public void Switch(AbstractLayer layer)
    {
        activeLayer.SwitchTo(layer);
        layer.SwitchFrom(activeLayer);
        activeLayer = layer;
    }

    public void ToggleLayer(GameObject player)
    {
        if(activeLayer == Layers.NORMAL)
        {
            foreach(LayerZone zone in zones)
            {
                if(zone.Contains(player))
                {
                    Switch(zone.layer);
                    return;
                }
            }
        }
        Switch(Layers.NORMAL);
    }

    private void OnDestroy()
    {
        INSTANCE = null;
    }

}
