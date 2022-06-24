using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowLayer : SimpleLayer
{
    public override void SwitchFrom(AbstractLayer layer)
    {
        Time.timeScale = 0.2f;
    }

    public override void SwitchTo(AbstractLayer layer)
    {
        Time.timeScale = 1f;
    }
}
