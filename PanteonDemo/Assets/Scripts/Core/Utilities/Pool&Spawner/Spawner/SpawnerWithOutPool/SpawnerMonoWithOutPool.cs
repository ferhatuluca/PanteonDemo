using UnityEngine;

namespace Core.Utilities.Pool_Spawner.Spawner.SpawnerWithOutPool
{
    public abstract class SpawnerMonoWithOutPool<T> : SpawnerMonoBase<T> where T: MonoBehaviour
    {
        [SerializeField] private T spawnObject;

        protected override T GetObjectFromPool()
        {
            return Instantiate(spawnObject);
        }
    }
}