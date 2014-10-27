using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class StorageController
{
    [Serializable]
    public struct StoragableData
    {
        public float TankX;
        public float TankY;
        public float TankAngle;

        public KeyValuePair<Position, Chunk.Data>[] Chunks;

        public StoragableData(Tank tank, World world)
        {
            TankX = tank.transform.localPosition.x;
            TankY = tank.transform.localPosition.y;
            TankAngle = tank.transform.eulerAngles.z;

            Chunks = world.ToArray();
        }

        public void Setup(Tank tank, World world)
        {
            Vector3 pos = tank.transform.localPosition;
            pos.x = TankX;
            pos.y = TankY;
            tank.transform.localPosition = pos;

            Vector3 rot = tank.transform.localEulerAngles;
            rot.z = TankAngle;
            tank.transform.localEulerAngles = rot;

            world.FromArray(Chunks);
        }
    }
    
    public static void Save(Tank tank, World world)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        StoragableData data = new StoragableData(tank, world);
        
        using(FileStream fs = new FileStream(PATH, FileMode.OpenOrCreate))
        {
            formatter.Serialize(fs, data);
            Utils.Log("Game state saved to " + fs.Name + ".");
        }
    }

    public static void Load(Tank tank, World world)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        if(File.Exists(PATH))
        {
            using(FileStream fs = new FileStream(PATH, FileMode.Open))
            {
                StoragableData data = (StoragableData)formatter.Deserialize(fs);
                data.Setup(tank, world);
                Utils.Log("Game state loaded from " + fs.Name + ".");
            }
        }
        else
        {
            Utils.Log("Save file was not found!");
        }
    }

    private static readonly string PATH = Application.dataPath + "/../save.dat";
}
