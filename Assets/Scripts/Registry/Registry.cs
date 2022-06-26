using System;
using System.Collections.Generic;

public class Registry<T>
{
    public readonly string registryName;
    private Dictionary<string, T> registeredObjects = new Dictionary<string, T>();

    public Registry(string name)
    {
        this.registryName = name;
    }

    public T Get(string name)
    {
        return registeredObjects[name];
    }

    public V Register<V>(string name, V value) where V : T
    {
        if (registeredObjects.ContainsKey(name))
            throw new ArgumentException(string.Format("Cannot register {0} to registry {1} as it already exists", name, registryName));
        
        registeredObjects.Add(name, value);
        return value;
    }
}
