using System;
using Core.Enums;
using Core.Managers;
using Core.Other;
using Core.Scriptables;
using Core.Utilities.Pool_Spawner;
using Core.Utilities.Pool_Spawner.Interfaces;
using Core.Utilities.Pool_Spawner.Pools;
using UnityEngine;

namespace Core.GameUnits.Buildings
{
	public class Building : MonoBehaviour, IGameUnitObject, IPoolMemberWithType<BuildingType>
	{
		[field: SerializeField] public BuildingType BuildingType { private set; get; }
		[SerializeField] private SpriteRenderer _modelSprite;

		private SoldierSpawner _soldierSpawner;
		private MonoBehaviorPool<Building> _myPool;
		
		public GameUnit GameUnit { private set; get; }

		private void Awake()
		{
			GameUnit = GetComponent<GameUnit>();
			_soldierSpawner = GetComponent<SoldierSpawner>();
		}

		private void Start()
		{
			// this should be in start because we need to get the pool after poolsmanager finishs its jobs
			_myPool = PoolsManager.Instance.GetMyPoolTyped<Building, BuildingType>(BuildingType);
		}

		public void Init(BuildingData buildingData, TeamType teamType, BuildingTypeData typeData)
		{
			_modelSprite.sprite = typeData.Icon;
			
			GameUnit.Init(this, teamType, buildingData);

			if (buildingData is UnitProducingBuildingData unitProducingBuildingData)
			{
				if (_soldierSpawner == null)
				{
					Debug.LogError("Soldier spawner doesn't exist on this building", gameObject);
					return;
				}
				_soldierSpawner.Init(this, unitProducingBuildingData);
			}
		}
		
		public void OnEnterPool()
		{
		}

		public void OnExitPool()
		{
		}

		public void Death()
		{
			Effect effect = EffectSpawnerManager.Instance.SpawnEffect(EffectType.BuildingDestruction);
			effect.transform.position = transform.position;
			GoToPool();
		}

		public void GoToPool()
		{
			_myPool.Push(this);
		}

		public bool IsUnitProducingBuilding()
		{
			return BuildingType is BuildingType.Barrack;
		}
		
		// interface short methods
		public BuildingType GetTypeForPool() => BuildingType;
	}
}