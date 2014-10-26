using System;
using UnityEngine;

public class Obstacle : MonoBehaviour, INode<Obstacle.Data>
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    public enum EType
    {
        None,
        Tree,
        Bush,
        Puddle,
        Rock
    }
    
    [Serializable]
    public class Data
    {
        public int Type;

        public Data(EType type)
        {
            Type = (int)type;
        }
    }
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed

    public GameObject Tree;
    public GameObject Puddle;
    public GameObject Rock;

    public Bush Bush;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region properties

    public EType Type
    {
        get { return _type; }
        private set
        {
            _type = value;
            UpdateView();
        }
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region public methods

    public void SpawnBush()
    {
        Type = EType.Bush;
    }

    public void Reset()
    {
        Type = EType.None;
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region INode
    
    public Position Pos { get; private set; }

    public void Init(Position pos)
    {
        Pos = pos;
        
        int width = Settings.Instance.ChunkWidth / Settings.Instance.Grid.X;
        int height = Settings.Instance.ChunkHeight / Settings.Instance.Grid.Y;

        int left = (width - Settings.Instance.ChunkWidth) / 2;
        int bottom = (height - Settings.Instance.ChunkHeight) / 2;

        Vector3 localPos = transform.localPosition;
        localPos.x = left + Pos.X * width;
        localPos.y = bottom + Pos.Y * height;
        transform.localPosition = localPos;

        Type = EType.None;
    }
    
    public void GenerateContent()
    {
        float random = UnityEngine.Random.value;
        if(random < Settings.Instance.TreeChance)
        {
            Type = EType.Tree;
        }
        else
        {
            random -= Settings.Instance.TreeChance;
            if(random < Settings.Instance.BushChance)
            {
                Type = EType.Bush;
            }
            else
            {
                random -= Settings.Instance.BushChance;
                if(random < Settings.Instance.PuddleChance)
                {
                    Type = EType.Puddle;
                }
                else
                {
                    Type = EType.Rock;
                }
            }
        }
    }

    public Data ToData()
    {
        return new Data(Type);
    }

    public void FromData(Data data)
    {
        Type = (EType)data.Type;
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private methods
    
    private void UpdateView()
    {
        Tree.SetActive(Type == EType.Tree);
        Puddle.SetActive(Type == EType.Puddle);
        Rock.SetActive(Type == EType.Rock);

        Bush.gameObject.SetActive(Type == EType.Bush);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region private members

    private EType _type;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
