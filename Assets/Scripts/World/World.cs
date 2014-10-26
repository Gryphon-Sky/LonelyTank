using System.Collections.Generic;
using UnityEngine;

public class World : Grid<Chunk, Chunk.Data>
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region Grid
    
    protected override GameObject NodesPrefab { get { return Settings.Instance.ChunkPrefab; } }

    protected override void Create()
    {
        base.Create();
        EnlargeAround(0, 0);

        _bushDetectionRadiusSqr = Settings.Instance.BushDetectionRadius * Settings.Instance.BushDetectionRadius;
        _tankDetectionRadiusSqr = Settings.Instance.TankDetectionRadius * Settings.Instance.TankDetectionRadius;
    }
    
    protected override void RemoveNodeFrom(Position pos)
    {
        GameObject.Destroy(_nodes[pos].gameObject);

        base.RemoveNodeFrom(pos);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region public methods

    public void Reset()
    {
        List<Position> positions = new List<Position>(_nodes.Keys);
        foreach(Position pos in positions)
        {
            RemoveNodeFrom(pos);
        }
        
        Create();
    }
    
    public void SpawnBush(Tank tank)
    {
        Dictionary<Obstacle, int> slots = new Dictionary<Obstacle, int>();
        int chances = 0;
        foreach(Obstacle slot in _freeSlots)
        {
            if(Utils.GetSqrDistance(slot, tank) > _tankDetectionRadiusSqr)
            {
                int bushes = 0;
                foreach(Bush bush in _bushes)
                {
                    if(Utils.GetSqrDistance(slot, bush) <= _bushDetectionRadiusSqr)
                    {
                        ++bushes;
                    }
                }
                int chance = Settings.Instance.MaxBushAround + 1 - bushes;
                slots.Add(slot, chance);
                chances += chance;
            }
        }

        if(slots.Count == 0)
        {
            // No slots
            return;
        }
        
        int bushSeed = Random.Range(0, chances);
        Obstacle bushSlot = null;
        foreach(var slot in slots)
        {
            if(slot.Value >= bushSeed)
            {
                bushSlot = slot.Key;
                break;
            }
            else
            {
                bushSeed -= slot.Value;
            }
        }

        if(bushSlot != null)
        {
            bushSlot.SpawnBush();
            _bushes.Add(bushSlot.Bush);
            _freeSlots.Remove(bushSlot);
        }
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region events
    
    public void OnTankEnteredToChunk(Chunk chunk)
    {
        EnlargeAround(chunk.Pos.X, chunk.Pos.Y);
    }

    public void OnTankTouchesObstacle(Obstacle obstacle)
    {
        if(obstacle.Type == Obstacle.EType.Bush)
        {
            obstacle.Reset();
            _freeSlots.Add(obstacle);
            _bushes.Remove(obstacle.Bush);
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

        List<Obstacle> allObstacles = new List<Obstacle>(transform.GetComponentsInChildren<Obstacle>());
        _freeSlots = allObstacles.FindAll(o => (o.Type == Obstacle.EType.None));

        _bushes = new List<Bush>(transform.GetComponentsInChildren<Bush>());
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region private members

    private List<Obstacle> _freeSlots;
    private List<Bush> _bushes;

    private float _bushDetectionRadiusSqr;
    private float _tankDetectionRadiusSqr;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}