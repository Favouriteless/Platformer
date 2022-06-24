using UnityEngine;

public abstract class AbstractLayer
{
    public abstract void Action(GameObject gameObject);
    public abstract void SwitchFrom(AbstractLayer layer);
    public abstract void SwitchTo(AbstractLayer layer);
}
