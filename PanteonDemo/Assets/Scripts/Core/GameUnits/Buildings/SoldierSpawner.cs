using Core.Enums;
using Core.GameUnits.Soldiers;
using Core.Managers;
using Core.Scriptables;
using Core.Utilities.Pool_Spawner.Spawner.SpawnerWithPool;
using Sirenix.OdinInspector;
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

		[Button]
		private void OnSoldierUIClicked(SoldierData soldierData, TeamType teamType)
		{
			if(_building.GameUnit.TeamType != teamType)
				return;
			
			SetSpawnType(soldierData.SoldierType);
			
			Spawn(newSoldier =>
			{
				//It gets object from pool, if there is no object then poolmanager spawns it, if there is then pops it
				SoldierTypeData typeData = GameManager.Instance.SoldierTeamData.GetSoldierTypeData(teamType, soldierData.SoldierType);
				newSoldier.Init(soldierData, _building.GameUnit.TeamType, typeData);
			});
		}
	}
}