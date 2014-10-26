using UnityEngine;

public class Settings : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    #region customizable

    public float TankMovingSpeed = 40.0f;
    public float TankRotationSpeed = 70.0f;

    public Position Grid = new Position(16, 12);

    public GameObject ChunkPrefab;
    public int ChunkWidth = 1024;
    public int ChunkHeight = 768;

    public int ObstaclesInChunk = 50;
    public GameObject ObstaclePrefab;

    public float TreeChance = 0.1f;
    public float BushChance = 0.3f;
    public float PuddleChance = 0.1f;

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
