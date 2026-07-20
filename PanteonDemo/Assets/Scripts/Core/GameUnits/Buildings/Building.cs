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

		private MonoBehaviorPool<Building> _myPool;
		
		public BuildingPlaceChecker BuildingPlaceChecker { private set; get; }
		public SoldierSpawner SoldierSpawner { private set; get; }
		public GameUnit GameUnit { private set; get; }

		private void Awake()
		{
			GameUnit = GetComponent<GameUnit>();
			SoldierSpawner = GetComponent<SoldierSpawner>();
			BuildingPlaceChecker = GetComponent<BuildingPlaceChecker>();
		}

		private void Start()
		{
			// this should be in start because we need to get the pool after poolsmanager finishs its jobs
			_myPool = PoolsManager.Instance.GetMyPoolTyped<Building, BuildingType>(BuildingType);
		}

		public void Init(BuildingData buildingData, TeamType teamType, BuildingTeamData data)
		{
			_modelSprite.sprite = data.Icon;
			
			GameUnit.Init(this, teamType, buildingData);
			BuildingPlaceChecker.Init(_modelSprite);

			if (IsUnitProducingBuilding())
			{
				if (SoldierSpawner == null)
				{
					Debug.LogError("Soldier spawner doesn't exist on this building", gameObject);
					return;
				}
				SoldierSpawner.Init(this);
			}
		}
		
		public void OnEnterPool()
		{
			BuildingPlaceChecker.ResetForPool();
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
		// this is used for checking OnTriggerEnter2D, if false then Unit will not register to Interacts in controller
		public bool IsAvailableForInteract() => BuildingPlaceChecker.IsPlaced;
	}
}