using Core.Enums;
using Core.Managers;
using Core.Scriptables;
using Core.Utilities.Pool_Spawner.Spawner.SpawnerWithPool;

namespace Core.GameUnits.Buildings
{
	public class BuildingSpawner : SpawnerMonoWithPoolWithType<Building, BuildingType>
	{
		public Building SpawnBuilding(BuildingData buildingData, TeamType teamType)
		{
			SetSpawnType(buildingData.BuildingType);

			Building newBuilding = null;
			Spawn(b => { newBuilding = b;});
			
			//It gets object from pool, if there is no object then poolmanager spawns it, if there is then pops it
			BuildingTypeData typeData = GameManager.Instance.BuildingTeamData.GetBuildingTypeData(teamType, buildingData.BuildingType);
			newBuilding.Init(buildingData, teamType, typeData);
			return newBuilding;
		}
	}
}