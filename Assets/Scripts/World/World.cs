using UnityEngine;

public class World : ObjectsSet<Chunk, Chunk.Data>
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region ObjectsSet
    
    protected override GameObject ObjectPrefab { get { return Settings.Instance.ChunkPrefab; } }

    protected override void Awake()
    {
        base.Awake();
    }
    
    protected override void Create()
    {
        base.Create();
        EnlargeAround(0, 0);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region public events
    
    public void OnTankEnteredToChunk(Chunk chunk)
    {
        EnlargeAround(chunk.Pos.X, chunk.Pos.Y);
    }

    public void OnTankTouchesObstacle(Obstacle obstacle)
    {
        if(obstacle.IsBush)
        {
            Chunk chunk = obstacle.transform.parent.parent.GetComponent<Chunk>();
            chunk.Remove(obstacle);
        }
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
                GenerateIfNeeded(new Position(i, j));
            }
        }
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}