using System;
using UnityEngine;

public class Obstacle : MonoBehaviour, ISetObject<Obstacle.Data>
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    [Serializable]
    public class Data
    {
    }
    
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
        ;
    }

    public Data ToData()
    {
        return new Data();
    }

    public void FromData(Data data)
    {
        ;
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region public methods
    
    public Position GetPosition()
    {
        Position pos;

        pos.X = Mathf.FloorToInt(transform.localPosition.x / Settings.Instance.ChunkWidth);
        pos.Y = Mathf.FloorToInt(transform.localPosition.y / Settings.Instance.ChunkHeight);

        return pos;
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
