using UnityEngine;

namespace Core.Utilities.Pool_Spawner.Spawner
{
    public abstract class SpawnerMonoBase<T> : SpawnerBase<T> where T: MonoBehaviour
    {
        protected sealed override GameObject AdjustValues(T spawnObject)
        {
            return spawnObject.gameObject;
        }
    }
}