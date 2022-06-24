using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layers
{
    public static Registry<AbstractLayer> LAYERS = new Registry<AbstractLayer>("layers");

    public static SimpleLayer NORMAL = LAYERS.Register("normal", new SimpleLayer());
    public static SimpleLayer GRAVITY = LAYERS.Register("gravity", new SimpleLayer());
    public static SlowLayer SLOW = LAYERS.Register("slow", new SlowLayer());
}
