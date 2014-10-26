using System;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : Grid<Obstacle, Obstacle.Data>, INode<Chunk.Data>
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
    
    #region Grid
    
    protected override GameObject NodesPrefab { get { return Settings.Instance.ObstaclePrefab; } }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region INode

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

        for(int y = 0; y < Grid.Y; ++y)
        {
            for(int x = 0; x < Grid.X; ++x)
            {
                CreateNodeAt(new Position(x, y));
            }
        }

        List<Obstacle> obstacles = new List<Obstacle>(_nodes.Values);

        for(int i = 0; i < Settings.Instance.ObstaclesInChunk; ++i)
        {
            int index = UnityEngine.Random.Range(0, obstacles.Count);
            obstacles[index].GenerateContent();
            obstacles.RemoveAt(index);
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
