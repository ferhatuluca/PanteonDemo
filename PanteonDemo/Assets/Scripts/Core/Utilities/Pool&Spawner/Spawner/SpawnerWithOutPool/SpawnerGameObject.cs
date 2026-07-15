using UnityEngine;

namespace Core.Utilities.Pool_Spawner.Spawner.SpawnerWithOutPool
{
    public abstract class SpawnerGameObject : SpawnerBase<GameObject>
    { 
        protected override GameObject GetGameObject(GameObject spawnObject)
        {
            return spawnObject;
        }
    }
}