using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Utilities.Pool_Spawner.Spawner
{
    public abstract class SpawnerBase<T> : MonoBehaviour where T: Object
    {
        private SpawnPoint[] _spawnPoints;
        private int _spawnPointIndex;
        
        protected int GetSpawnPointCount() => _spawnPoints.Length;
        protected SpawnPoint GetCurrentSpawnPoint() => _spawnPoints[_spawnPointIndex];
        protected SpawnPoint[] AllSpawnPoints() => _spawnPoints;
        
        protected abstract GameObject GetGameObject(T spawnObject);
        protected abstract T GetObjectFromPool();
        
        protected virtual bool CheckSpawnPointAvailability(SpawnPoint spawnPoint) => true;
        protected virtual void InternalAwake(){}
        protected virtual void InternalStart(){}
        
        protected virtual void Awake()
        {
            _spawnPoints = GetComponentsInChildren<SpawnPoint>();
            InternalAwake();
        }
        
        protected virtual void Start()
        {
            InternalStart();
        }

        protected void SpawnLoop(int count, float delay)
        {
            if(delay != 0)
                StartCoroutine(SpawnEnumerator(count, delay));
            else
            {
                for (int i = 0; i < count; i++)
                {
                    Spawn();
                }
            }
        }
        
        protected IEnumerator SpawnEnumerator(int count, float delay)
        {
            for (int i = 0; i < count; i++)
            {
                Spawn();
                yield return new WaitForSeconds(delay);
            }
        }
        
        protected void Spawn(Action<T> onSpawnDone = null)
        {
            SpawnPoint spawnPoint = FindAvailableSpawnPoint();
            T spawnedObject = GetObjectFromPool();
            GameObject gameObjectOfSpawn = GetGameObject(spawnedObject);
            
            if (spawnPoint != null)
            {
                spawnPoint.spawnObject = gameObjectOfSpawn;
                gameObjectOfSpawn.transform.position = _spawnPoints[_spawnPointIndex].transform.position;
            }
            
            onSpawnDone?.Invoke(spawnedObject);
        }

        private SpawnPoint FindAvailableSpawnPoint()
        {
            if (_spawnPoints.Length <= 0)
                return null;
            
            SpawnPoint availableSpawnPoint = null;
            int count = 0;
            while (count < _spawnPoints.Length)
            {
                availableSpawnPoint = _spawnPoints[_spawnPointIndex];
                if (!CheckSpawnPointAvailability(availableSpawnPoint))
                {
                    IncreaseIndex();
                }
                count++;
            }
            return availableSpawnPoint;
        }
        
        private void IncreaseIndex()
        {
            _spawnPointIndex++;
            if (_spawnPointIndex >= _spawnPoints.Length)
            {
                _spawnPointIndex = 0;
            }
        }
    }
}