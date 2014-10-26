using UnityEngine;
using System.Collections.Generic;

public abstract class Grid<TSelf, TNode, TData> : MonoBehaviour
    where TSelf: Grid<TSelf, TNode, TData>
    where TNode: Component, INode<TSelf, TData>
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed
    
    public Transform ObjectsParent;
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region overridables
    
    protected abstract GameObject ObjectPrefab { get; }
    
    protected virtual void Create()
    {
        if(_objects == null)
        {
            _objects = new SortedDictionary<Position, TNode>(new Position.Comparer());
        }
    }
    
    public virtual void Remove(TNode node)
    {
        Remove(node.Pos, node);
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
            SetNodeTo(pair.Key, pair.Value);
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
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region protected methods
    
    protected void GenerateIfNeeded(Position pos)
    {
        if(!_objects.ContainsKey(pos))
        {
            TNode node = CreateAt(pos);
            node.GenerateContent();
        }
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region MonoBehaviour
    
    private void Awake()
    {
        Create();
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private methods
    
    private void SetNodeTo(Position pos, TData initialData)
    {
        if(_objects.ContainsKey(pos))
        {
            _objects[pos].FromData(initialData);
        }
        else
        {
            TNode node = CreateAt(pos);
            node.FromData(initialData);
        }
    }

    private TNode CreateAt(Position pos)
    {
        GameObject go = (GameObject.Instantiate(ObjectPrefab) as GameObject);
        go.transform.parent = ObjectsParent;
        
        TNode node = go.GetComponent<TNode>();
        node.Init((TSelf)this, pos);
        _objects[pos] = node;
        
        return node;
    }

    private void Remove(Position pos, TNode node)
    {
        GameObject.Destroy(node.gameObject);
        
        _objects.Remove(pos);
    }
    
    private void RemoveAt(Position pos)
    {
        Remove(pos, _objects[pos]);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private members

    private SortedDictionary<Position, TNode> _objects;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}