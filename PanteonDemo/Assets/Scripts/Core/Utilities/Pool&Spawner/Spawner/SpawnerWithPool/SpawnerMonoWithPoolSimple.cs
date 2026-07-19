using Core.Utilities.Pool_Spawner.Interfaces;
using Core.Utilities.Pool_Spawner.Pools;
using UnityEngine;

namespace Core.Utilities.Pool_Spawner.Spawner.SpawnerWithPool
{
    public abstract class SpawnerMonoWithPoolSimple<T> : SpawnerMonoBase<T> where T: MonoBehaviour, IPoolMemberSimple
    {
        protected MonoBehaviorPool<T> MonoBehaviorPool;

        protected sealed override void Awake()
        {
            base.Awake();
            MonoBehaviorPool = PoolsManager.Instance.GetMyPoolSimple<T>();
        }

        protected override T GetObjectFromPool()
        {
            return MonoBehaviorPool.Pull();
        }
    }
}