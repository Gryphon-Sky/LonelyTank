using UnityEngine;

public class GameplayController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region exposed
    
    public InputController InputController;

    public World World;
    public Tank Tank;
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region MonoBehaviour
    
    private void Awake()
    {
        Tank.Init(InputController, World.OnTankEnteredToChunk, World.OnTankTouchesObstacle);

        InputController.OnSave = Save;
        InputController.OnLoad = Load;
        InputController.OnReset = Reset;

        Load();

        BushSpawnController.StartSpawn(World);
    }
    
    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    
    #region events

        private void Save()
    {
        StorageController.Save(Tank, World);
    }

    private void Load()
    {
        StorageController.Load(Tank, World);
    }
    
    private void Reset()
    {
        Tank.Reset();
        World.Reset();
        Utils.Log("Game state resetted.");
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
