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
    
    #region ISetObject

    public Position Pos { get; private set; }
    
    public void Init(Position pos)
    {
        Pos = pos;

        Terrain.localScale = new Vector3(Settings.Instance.ChunkWidth, Settings.Instance.ChunkHeight, 1);

        Vector3 localPos = transform.localPosition;
        localPos.x = Settings.Instance.ChunkWidth * Pos.X;
        localPos.y = Settings.Instance.ChunkHeight * Pos.Y;
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

            GenerateIfNeeded(pos);

            positions.RemoveAt(index);
        }
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
