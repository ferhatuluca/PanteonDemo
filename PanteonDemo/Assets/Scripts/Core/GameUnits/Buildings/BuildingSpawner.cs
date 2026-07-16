using Core.Enums;
using Core.Scriptables;
using Core.Utilities.Pool_Spawner.Spawner.SpawnerWithPool;

namespace Core.GameUnits.Buildings
{
	public class BuildingSpawner : SpawnerMonoWithPoolWithType<Building, BuildingType>
	{
		public void SendBuildingToPool(Building building)
		{
			GetPool(building.BuildingType).Push(building);	
		}
		
		public Building SpawnBuilding(BuildingData buildingData, TeamType teamType)
		{
			SetSpawnType(buildingData.BuildingType);
			
			//It gets object from pool, if there is no object poolmanager spawns it, if there is then pops it
			Building newBuilding = GetObjectFromPool();
			newBuilding.Init(buildingData, teamType);
			return newBuilding;
		}
	}
}