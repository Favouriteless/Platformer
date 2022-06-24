using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LayerZone : MonoBehaviour
{
    public AbstractLayer layer;
    public string layerName;
    public BoxCollider2D col;

    private void Awake()
    {
        layer = Layers.LAYERS.Get(layerName);
        if (layer == null)
            throw new ArgumentException(String.Format("LayerZone could not find layer with name {0}", layerName));
    }

    public bool Contains(GameObject gameObject)
    {
        return col.bounds.Contains(gameObject.transform.position);
    }
}
