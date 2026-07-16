using Core.Enums;
using Core.GameUnits.Soldiers;
using Core.Scriptables;
using Core.Utilities.Pool_Spawner.Spawner.SpawnerWithPool;
using UI;

namespace Core.GameUnits.Buildings
{
	public class SoldierSpawner : SpawnerMonoWithPoolWithType<Soldier, SoldierType>
	{
		private Building _building;
		private UnitProducingBuildingData _unitProducingBuildingData;
		
		public void Init(Building building, UnitProducingBuildingData unitProducingBuildingData)
		{
			_building = building;
			_unitProducingBuildingData = unitProducingBuildingData;
		}

		private void OnEnable()
		{
			SoldierUI.OnSoldierUIClicked += OnSoldierUIClicked;
		}

		private void OnDisable()
		{
			SoldierUI.OnSoldierUIClicked -= OnSoldierUIClicked;
		}

		private void OnSoldierUIClicked(SoldierData soldierData, TeamType teamType)
		{
			SetSpawnType(soldierData.SoldierType);
			
			//It gets object from pool, if there is no object poolmanager spawns it, if there is then pops it
			Soldier newSoldier = GetObjectFromPool();
			SoldierTypeData typeData = GeneralData.Instance.SoldierTeamData.GetSoldierTypeData(teamType, soldierData.SoldierType);
			newSoldier.Init(soldierData, _building.TeamType, typeData);
		}
	}
}