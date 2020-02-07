using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public abstract class RuntimeSet<T> : ScriptableObject
{
    public List<T> items = new List<T>();

    public void Add(T t)
    {
        if(!items.Contains(t)) items.Add(t);
    }

    public void Remove(T t)
    {
        if(items.Contains(t)) items.Remove(t);
    }

    public int Count()
    {
        return items.Count;
    }

    public void Clear()
    {
        items.Clear();
    }
}
