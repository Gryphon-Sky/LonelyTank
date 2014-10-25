using System;
using UnityEngine;

public class Obstacle : MonoBehaviour, ISetObject<Obstacle.Data>
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    public enum EType
    {
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
    public GameObject Bush;
    public GameObject Puddle;
    public GameObject Rock;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region propertiex

    public bool IsBush { get { return (_type == EType.Bush); } }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region SetObject
    
    public void Init(Position pos)
    {
        int width = Settings.Instance.ChunkWidth / Settings.Instance.Grid.X;
        int height = Settings.Instance.ChunkHeight / Settings.Instance.Grid.Y;

        int left = (width - Settings.Instance.ChunkWidth) / 2;
        int bottom = (height - Settings.Instance.ChunkHeight) / 2;

        Vector3 localPos = transform.localPosition;
        localPos.x = left + pos.X * width;
        localPos.y = bottom + pos.Y * height;
        transform.localPosition = localPos;
    }
    
    public void GenerateContent()
    {
        _type = EType.Rock;
        float random = UnityEngine.Random.value;
        if(random < Settings.Instance.TreeChance)
        {
            _type = EType.Tree;
        }
        else
        {
            random -= Settings.Instance.TreeChance;
            if(random < Settings.Instance.BushChance)
            {
                _type = EType.Bush;
            }
            else
            {
                random -= Settings.Instance.BushChance;
                if(random < Settings.Instance.PuddleChance)
                {
                    _type = EType.Puddle;
                }
            }
        }
        UpdateView();
    }

    public Position GetPosition()
    {
        Position pos;

        int width = Settings.Instance.ChunkWidth / Settings.Instance.Grid.X;
        int height = Settings.Instance.ChunkHeight / Settings.Instance.Grid.Y;
        
        int left = (width - Settings.Instance.ChunkWidth) / 2;
        int bottom = (height - Settings.Instance.ChunkHeight) / 2;
        
        pos.X = Mathf.FloorToInt(transform.localPosition.x - left / width);
        pos.Y = Mathf.FloorToInt(transform.localPosition.y - bottom / height);

        return pos;
    }
    
    public Data ToData()
    {
        return new Data(_type);
    }

    public void FromData(Data data)
    {
        _type = (EType)data.Type;
        UpdateView();
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region private methods
    
    private void UpdateView()
    {
        Tree.SetActive(_type == EType.Tree);
        Bush.SetActive(_type == EType.Bush);
        Puddle.SetActive(_type == EType.Puddle);
        Rock.SetActive(_type == EType.Rock);
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
