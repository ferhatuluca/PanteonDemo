using System;
using UnityEngine;

namespace Core.Utilities.Spawner
{
    public abstract class Spawner<T> : MonoBehaviour where T: MonoBehaviour
    {
        private SpawnPoint[] _spawnPoints;
        private int _spawnPointIndex;
        
        protected abstract T GetSpawnedObject();
        
        protected virtual void InternalInit(){}

        public SpawnPoint GetFirstSpawnPoint() => _spawnPoints[0];
        public SpawnPoint CurrentSpawnPoint() => _spawnPoints[_spawnPointIndex];
        public SpawnPoint[] AllSpawnPoints() => _spawnPoints;
        
        public void Init()
        {
            SetSpawnPoints();
            InternalInit();
        }

        private void SetSpawnPoints()
        {
            _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        }
        
        private void IncreaseIndex()
        {
            _spawnPointIndex++;
            if (_spawnPointIndex >= _spawnPoints.Length)
            {
                _spawnPointIndex = 0;
            }
        }

        public void Spawn(Action<T> onSpawnDone = null)
        {
            if(_spawnPoints.Length == 0)
                SetSpawnPoints();

            T spawnedObject = GetSpawnedObject();
            SpawnPoint spawnPoint = _spawnPoints[_spawnPointIndex];
            
            spawnPoint.spawnObject = spawnedObject.gameObject;
            spawnedObject.gameObject.transform.position = _spawnPoints[_spawnPointIndex].transform.position;
            
            onSpawnDone?.Invoke(spawnedObject);
            IncreaseIndex();
        }
    }
}