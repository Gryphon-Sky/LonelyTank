using UnityEngine;
using System.Collections.Generic;

public abstract class ObjectsSet<TComponent, TData> : MonoBehaviour
    where TComponent: Component, ISetObject<TData>
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed
    
    public Transform Parent;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region properties

    protected abstract GameObject ObjectPrefab { get; }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region utility
    
    public override string ToString()
    {
        string result = "";
        foreach(var pair in _objects)
        {
            result += string.Format("[{0}: {1}]", pair.Key, pair.Value);
        }
        return result;
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region MonoDevelop
    
    protected virtual void Awake()
    {
        Create();
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region creation
    
    protected virtual void Create()
    {
        if(_objects == null)
        {
            _objects = new SortedDictionary<Position, TComponent>(new Position.Comparer());
        }
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region public methods

    public KeyValuePair<Position, TData>[] ToArray()
    {
        KeyValuePair<Position, TData>[] array = new KeyValuePair<Position, TData>[_objects.Count];
        int i = 0;
        foreach(var pair in _objects)
        {
            array[i] = new KeyValuePair<Position, TData>(pair.Key, pair.Value.ToData());
            ++i;
        }
        return array;
    }

    public void FromArray(KeyValuePair<Position, TData>[] array)
    {
        var list = new List<KeyValuePair<Position, TData>>(array);

        List<Position> toRemove = new List<Position>();
        foreach(Position pos in _objects.Keys)
        {
            if(list.Find(p => p.Key == pos).Equals(default(KeyValuePair<Position, TData>)))
            {
                toRemove.Add(pos);
            }
        }
        foreach(Position pos in toRemove)
        {
            RemoveAt(pos);
        }

        foreach(var pair in list)
        {
            AddIfNeeded(pair.Key, pair.Value);
        }
    }

    public void Reset()
    {
        List<Position> positions = new List<Position>(_objects.Keys);
        foreach(Position pos in positions)
        {
            RemoveAt(pos);
        }

        Create();
    }

    public void RemoveAt(Position pos)
    {
        Remove(pos, _objects[pos]);
    }

    public void Remove(TComponent component)
    {
        Remove(component.GetPosition(), component);
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region protected methods
    
    protected void AddIfNeeded(Position pos)
    {
        if(!_objects.ContainsKey(pos))
        {
            Create(pos);
        }
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private methods
    
    private void AddIfNeeded(Position pos, TData initialData)
    {
        if(_objects.ContainsKey(pos))
        {
            _objects[pos].FromData(initialData);
        }
        else
        {
            Create(pos, initialData);
        }
    }

    private TComponent Create(Position pos, TData initialData)
    {
        GameObject go = (GameObject.Instantiate(ObjectPrefab) as GameObject);
        go.transform.parent = Parent;

        TComponent component = go.GetComponent<TComponent>();
        component.Init(pos);
        component.FromData(initialData);
        _objects[pos] = component;

        return component;
    }
    
    private TComponent Create(Position pos)
    {
        GameObject go = (GameObject.Instantiate(ObjectPrefab) as GameObject);
        go.transform.parent = Parent;
        
        TComponent component = go.GetComponent<TComponent>();
        component.Init(pos);
        component.GenerateContent();
        _objects[pos] = component;
        
        return component;
    }
    
    private void Remove(Position pos, TComponent component)
    {
        GameObject.Destroy(component.gameObject);
        
        _objects.Remove(pos);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private members

    private SortedDictionary<Position, TComponent> _objects;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}