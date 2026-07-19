using System.Collections.Generic;
using Core.Utilities.Pool_Spawner.Interfaces;
using Core.Utilities.Pool_Spawner.Pools;
using UnityEngine;

namespace Core.Utilities.Pool_Spawner.Spawner.SpawnerWithPool
{
    public abstract class SpawnerMonoWithPoolWithType<T, TLogic> : SpawnerMonoBase<T> where T: MonoBehaviour, IPoolMemberWithType<TLogic>
    {
        private Dictionary<TLogic, MonoBehaviorPool<T>> _monoBehaviorPoolWithType;
        private TLogic _currentSpawnType;

        protected TLogic GetCurrentSpawnType() => _currentSpawnType;
        protected MonoBehaviorPool<T> GetPoolWithCurrentSpawnType() => _monoBehaviorPoolWithType[_currentSpawnType];
        protected MonoBehaviorPool<T> GetPool(TLogic tLogic) => _monoBehaviorPoolWithType[tLogic];
        
        protected sealed override void Awake()
        {
            base.Awake();
            _monoBehaviorPoolWithType = PoolsManager.Instance.GetMyPoolsOfTyped<T, TLogic>();
        }

        protected void SetSpawnType(TLogic type)
        {
            _currentSpawnType = type;
        }
        
        protected override T GetObjectFromPool()
        {
            return _monoBehaviorPoolWithType[_currentSpawnType].Pull();
        }
    }
}