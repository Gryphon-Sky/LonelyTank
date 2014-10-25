using System;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : ObjectsSet<Obstacle, Obstacle.Data>, ISetObject<Chunk.Data>
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    [Serializable]
    public class Data
    {
        public KeyValuePair<Position, Obstacle.Data>[] Obstacles;

        public Data(KeyValuePair<Position, Obstacle.Data>[] obstacles)
        {
            Obstacles = obstacles;
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed
    
    public Transform Terrain;
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region ObjectsSet

    protected override GameObject ObjectPrefab { get { return Settings.Instance.ObstaclePrefab; } }
    
    protected override void Awake()
    {
        base.Awake();
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region SetObject
    
    public void Init(Position pos)
    {
        Terrain.localScale = new Vector3(Settings.Instance.ChunkWidth, Settings.Instance.ChunkHeight, 1);

        Vector3 localPos = transform.localPosition;
        localPos.x = Settings.Instance.ChunkWidth * pos.X;
        localPos.y = Settings.Instance.ChunkHeight * pos.Y;
        transform.localPosition = localPos;
    }
    
    public void GenerateContent()
    {
        Position Grid = Settings.Instance.Grid;

        List<Position> positions = new List<Position>(Grid.X * Grid.Y);
        for(int y = 0; y < Grid.Y; ++y)
        {
            for(int x = 0; x < Grid.X; ++x)
            {
                positions.Add(new Position(x, y));
            }
        }

        for(int i = 0; i < Settings.Instance.ObstaclesInChunk; ++i)
        {
            int index = UnityEngine.Random.Range(0, positions.Count);
            Position pos = positions[index];

            AddIfNeeded(pos);

            positions.RemoveAt(index);
        }
    }

    public Position GetPosition()
    {
        Position pos;
        
        pos.X = Mathf.FloorToInt(transform.localPosition.x / Settings.Instance.ChunkWidth);
        pos.Y = Mathf.FloorToInt(transform.localPosition.y / Settings.Instance.ChunkHeight);
        
        return pos;
    }
    
    public Data ToData()
    {
        return new Data(ToArray());
    }

    public void FromData(Data data)
    {
        FromArray(data.Obstacles);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
