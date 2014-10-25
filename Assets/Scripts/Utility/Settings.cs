using UnityEngine;

public class Settings : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region customizable

    public float TankMovingSpeed = 40.0f;
    public float TankRotationSpeed = 70.0f;

    public Position Grid = new Position(28, 21);

    public GameObject ChunkPrefab;
    public int ChunkWidth = 1024;
    public int ChunkHeight = 768;

    public GameObject ObstaclePrefab;
    public int ObstaclesInChunk = 768;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region access

    public static Settings Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<Settings>();
            }
            return _instance;
        }
    }
    private static Settings _instance;

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
