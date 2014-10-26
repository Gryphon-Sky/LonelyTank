using UnityEngine;
using System.Collections.Generic;

public abstract class Grid<TSelf, TNode, TData> : MonoBehaviour
    where TSelf: Grid<TSelf, TNode, TData>
    where TNode: Component, INode<TSelf, TData>
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed
    
    public Transform NodesParent;
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region overridables
    
    protected abstract GameObject NodesPrefab { get; }
    
    protected virtual void Create()
    {
        if(_nodes == null)
        {
            _nodes = new SortedDictionary<Position, TNode>(new Position.Comparer());
        }
    }
    
    public virtual void RemoveNode(TNode node)
    {
        RemoveNode(node.Pos, node);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region public methods
    
    public KeyValuePair<Position, TData>[] ToArray()
    {
        KeyValuePair<Position, TData>[] array = new KeyValuePair<Position, TData>[_nodes.Count];
        int i = 0;
        foreach(var pair in _nodes)
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
        foreach(Position pos in _nodes.Keys)
        {
            if(list.Find(p => p.Key == pos).Equals(default(KeyValuePair<Position, TData>)))
            {
                toRemove.Add(pos);
            }
        }
        foreach(Position pos in toRemove)
        {
            RemoveNodeAt(pos);
        }
        
        foreach(var pair in list)
        {
            SetNodeTo(pair.Key, pair.Value);
        }
    }
    
    public void Reset()
    {
        List<Position> positions = new List<Position>(_nodes.Keys);
        foreach(Position pos in positions)
        {
            RemoveNodeAt(pos);
        }
        
        Create();
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region protected methods
    
    protected void GenerateIfNeeded(Position pos)
    {
        if(!_nodes.ContainsKey(pos))
        {
            TNode node = CreateNodeAt(pos);
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
        if(_nodes.ContainsKey(pos))
        {
            _nodes[pos].FromData(initialData);
        }
        else
        {
            TNode node = CreateNodeAt(pos);
            node.FromData(initialData);
        }
    }

    private TNode CreateNodeAt(Position pos)
    {
        GameObject go = (GameObject.Instantiate(NodesPrefab) as GameObject);
        go.transform.parent = NodesParent;
        
        TNode node = go.GetComponent<TNode>();
        node.Init((TSelf)this, pos);
        _nodes[pos] = node;
        
        return node;
    }

    private void RemoveNode(Position pos, TNode node)
    {
        GameObject.Destroy(node.gameObject);
        
        _nodes.Remove(pos);
    }
    
    private void RemoveNodeAt(Position pos)
    {
        RemoveNode(pos, _nodes[pos]);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private members

    private SortedDictionary<Position, TNode> _nodes;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}