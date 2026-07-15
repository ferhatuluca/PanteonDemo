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
        
        protected abstract T GetObjectFromPool();
        
        protected virtual void InternalAwake(){}
        protected virtual void InternalStart(){}
        protected virtual void OnSpawnDone(T spawnObject){}

        protected int GetSpawnPointCount() => _spawnPoints.Length;
        protected SpawnPoint GetCurrentSpawnPoint() => _spawnPoints[_spawnPointIndex];
        protected SpawnPoint[] AllSpawnPoints() => _spawnPoints;
        
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
        
        private void IncreaseIndex()
        {
            _spawnPointIndex++;
            if (_spawnPointIndex >= _spawnPoints.Length)
            {
                _spawnPointIndex = 0;
            }
        }

        protected abstract GameObject GetGameObject(T spawnObject);

        public void Spawn(Action<T> onSpawnDone = null)
        {
            var spawnedObject = GetObjectFromPool();
            var gameObjectOfSpawn = GetGameObject(spawnedObject);
            if (_spawnPoints.Length > 0)
            {
                var spawnPoint = _spawnPoints[_spawnPointIndex];
                spawnPoint.spawnObject = gameObjectOfSpawn;
                gameObjectOfSpawn.transform.position = _spawnPoints[_spawnPointIndex].transform.position;
            }
            
            OnSpawnDone(spawnedObject);
            onSpawnDone?.Invoke(spawnedObject);
            IncreaseIndex();
        }
    }
}