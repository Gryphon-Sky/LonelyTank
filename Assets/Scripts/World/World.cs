using UnityEngine;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed
    
    public Transform ChunksParent;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region utility

    public override string ToString()
    {
        string result = "";
        foreach(var pair in _chunks)
        {
            result += string.Format("[{0}: {1}]", pair.Key, pair.Value);
        }
        return result;
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region MonoDevelop
    
    private void Awake()
    {
        Create();
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region creation
    
    private void Create()
    {
        if(_chunks == null)
        {
            _chunks = new SortedDictionary<Position, Chunk>(new Position.Comparer());
        }
        EnlargeAround(0, 0);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region public methods

    public Position[] GetChunksPositionsArray()
    {
        Position[] array = new Position[_chunks.Count];
        _chunks.Keys.CopyTo(array, 0);
        return array;
    }

    public void MorpthTo(Position[] positionsArray)
    {
        List<Position> positions = new List<Position>(positionsArray);

        List<Position> chunksToRemove = new List<Position>();
        foreach(Position pos in _chunks.Keys)
        {
            if(!positions.Contains(pos))
            {
                chunksToRemove.Add(pos);
            }
        }
        foreach(Position pos in chunksToRemove)
        {
            Chunk chunk = _chunks[pos];
            GameObject.Destroy(chunk.gameObject);

            _chunks.Remove(pos);
        }

        foreach(Position pos in positions)
        {
            AddIfNeeded(pos);
        }
    }

    public void OnTankEnteredToChunk(Chunk chunk)
    {
        Position pos = chunk.GetPosition();
        EnlargeAround(pos.X, pos.Y);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private methods
    
    private void EnlargeAround(int x, int y)
    {
        for(int j = y - 1; j <= y + 1; ++j)
        {
            for(int i = x - 1; i <= x + 1; ++i)
            {
                AddIfNeeded(new Position(i, j));
            }
        }
    }

    private void AddIfNeeded(Position pos)
    {
        if(!_chunks.ContainsKey(pos))
        {
            Chunk newChunk = Chunk.Create(Settings.Instance.ChunkPrefab);
            newChunk.transform.parent = ChunksParent;
            newChunk.SetPosition(pos);
            _chunks[pos] = newChunk;
        }
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private members

    private SortedDictionary<Position, Chunk> _chunks;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}