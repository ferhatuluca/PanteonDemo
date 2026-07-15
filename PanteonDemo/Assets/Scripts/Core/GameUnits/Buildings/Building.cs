using Core.Scriptables;
using Core.Types;
using Core.Utilities.Pool_Spawner.Interfaces;
using UnityEngine;

namespace Core.GameUnits.Buildings
{
	public class Building : MonoBehaviour, IClickableGameUnit, IPoolMemberWithType<BuildingType>
	{
		public BuildingType BuildingType { private set; get; }
		public TeamType TeamType { private set; get; }
		public Vector2 GridSize { private set; get; }

		public void OnSelect()
		{
			throw new System.NotImplementedException();
		}

		public BuildingType GetTypeForPool()
		{
			return BuildingType;
		}

		public void OnEnterPool()
		{
			throw new System.NotImplementedException();
		}

		public void OnExitPool()
		{
			throw new System.NotImplementedException();
		}

		public void SetData(BuildingData buildingData, TeamType teamType)
		{
			BuildingType = buildingData.BuildingType;
			TeamType = teamType;
			GridSize = buildingData.GridSize;

			if (buildingData is UnitProducingBuildingData unitProducingBuildingData)
			{
				SoldierSpawner soldierSpawner = gameObject.AddComponent<SoldierSpawner>();
				soldierSpawner.Init(this, unitProducingBuildingData);
			}
		}

		public bool IsUnitProducingBuilding()
		{
			return BuildingType is BuildingType.Barrack;
		}
	}
}