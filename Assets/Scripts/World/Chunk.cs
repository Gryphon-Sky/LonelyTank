using System;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed
    
    public Transform Terrain;
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region creation
    
    public static Chunk Create(GameObject prefab)
    {
        GameObject go = (GameObject.Instantiate(prefab) as GameObject);

        Chunk chunk = go.GetComponent<Chunk>();
        chunk.Terrain.localScale = new Vector3(Settings.Instance.ChunkWidth, Settings.Instance.ChunkHeight, 1);
        
        return chunk;
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
    
    public void SetPosition(Position pos)
    {
        Vector3 localPos = transform.localPosition;
        localPos.x = Settings.Instance.ChunkWidth * pos.X;
        localPos.y = Settings.Instance.ChunkHeight * pos.Y;
        transform.localPosition = localPos;
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
