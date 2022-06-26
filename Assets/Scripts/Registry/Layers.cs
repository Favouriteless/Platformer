using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layers
{
    public static Registry<AbstractLayer> REGISTRY = new Registry<AbstractLayer>("layers");

    public static SimpleLayer NORMAL = REGISTRY.Register("normal", new SimpleLayer());
    public static SimpleLayer GRAVITY = REGISTRY.Register("gravity", new SimpleLayer());
    public static SlowLayer SLOW = REGISTRY.Register("slow", new SlowLayer());
}
