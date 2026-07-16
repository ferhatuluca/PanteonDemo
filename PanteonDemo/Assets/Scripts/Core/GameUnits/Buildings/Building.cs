using Core.Enums;
using Core.Scriptables;
using Core.Utilities.Pool_Spawner.Interfaces;
using UnityEngine;

namespace Core.GameUnits.Buildings
{
	public class Building : MonoBehaviour, IClickableGameUnit, IPoolMemberWithType<BuildingType>
	{
		[SerializeField] private SpriteRenderer _modelSprite;
		
		public BuildingType BuildingType { private set; get; }
		public TeamType TeamType { private set; get; }
		public Vector2 GridSize { private set; get; }
		
		public bool IsAlive() => true; // will be implemented
		public BuildingType GetTypeForPool() => BuildingType;
		public TeamType GetTeamType() => TeamType;
		public Transform GetTransform() => transform;
		
		public void Init(BuildingData buildingData, TeamType teamType, BuildingTypeData typeData)
		{
			BuildingType = buildingData.BuildingType;
			TeamType = teamType;
			GridSize = buildingData.GridSize;

			_modelSprite.sprite = typeData.Icon;

			if (buildingData is UnitProducingBuildingData unitProducingBuildingData)
			{
				SoldierSpawner soldierSpawner = gameObject.AddComponent<SoldierSpawner>();
				soldierSpawner.Init(this, unitProducingBuildingData);
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

		public bool IsUnitProducingBuilding()
		{
			return BuildingType is BuildingType.Barrack;
		}
	}
}