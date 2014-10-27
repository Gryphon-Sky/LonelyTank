using System.Collections.Generic;
using System.Linq;
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
    
    public void SpawnBush()
    {
        Dictionary<Obstacle, int> slots = new Dictionary<Obstacle, int>();
        int chances = 0;
        foreach(Obstacle slot in _freeSlots)
        {
            int bushes = 0;
            foreach(Obstacle bush in _bushes)
            {
                if(Position.IsNear(slot.GlobalPos, bush.GlobalPos))
                {
                    ++bushes;
                }
            }
            int chance = Settings.Instance.MaxBushAround + 1 - bushes;
            slots.Add(slot, chance);
            chances += chance;
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

            bushSeed -= slot.Value;
        }

        if(bushSlot != null)
        {
            bushSlot.SpawnBush();
            _bushes.Add(bushSlot);
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
            _bushes.Remove(obstacle);
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

        List<Bush> bushes = new List<Bush>(transform.GetComponentsInChildren<Bush>());
        _bushes = bushes.Select(b => b.Parent).ToList();
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region private members

    private List<Obstacle> _freeSlots;
    private List<Obstacle> _bushes;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}