using Core.Enums;
using Core.Scriptables;
using Core.Utilities.Pool_Spawner;
using Core.Utilities.Pool_Spawner.Interfaces;
using Core.Utilities.Pool_Spawner.Pools;
using UnityEngine;

namespace Core.GameUnits.Buildings
{
	public class Building : MonoBehaviour, IGameUnitObject, IPoolMemberWithType<BuildingType>
	{
		[SerializeField] private SpriteRenderer _modelSprite;

		private SoldierSpawner _soldierSpawner;
		
		public BuildingType BuildingType { private set; get; }
		public GameUnit GameUnit { private set; get; }

		private void Awake()
		{
			GameUnit = GetComponent<GameUnit>();
			_soldierSpawner = GetComponent<SoldierSpawner>();
		}

		public void Init(BuildingData buildingData, TeamType teamType, BuildingTypeData typeData)
		{
			_modelSprite.sprite = typeData.Icon;
			BuildingType = buildingData.BuildingType;
			
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
		
		public void OnSelect()
		{
			throw new System.NotImplementedException();
		}
		
		public void OnEnterPool()
		{
			throw new System.NotImplementedException();
		}

		public void OnExitPool()
		{
			throw new System.NotImplementedException();
		}

		public void Death()
		{
			// destruction effect
			GoToPool();
		}

		public void GoToPool()
		{
			MonoBehaviorPool<Building> pool = PoolsManager.Instance.GetMyPoolTyped<Building, BuildingType>(BuildingType);
			pool.Push(this);
		}

		public bool IsUnitProducingBuilding()
		{
			return BuildingType is BuildingType.Barrack;
		}
		
		// interface short methods
		public BuildingType GetTypeForPool() => BuildingType;
	}
}