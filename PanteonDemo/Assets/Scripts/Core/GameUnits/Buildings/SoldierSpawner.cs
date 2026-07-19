using Core.Enums;
using Core.GameUnits.Soldiers;
using Core.Managers;
using Core.Scriptables;
using Core.Utilities.Pool_Spawner.Spawner;
using Core.Utilities.Pool_Spawner.Spawner.SpawnerWithPool;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Core.GameUnits.Buildings
{
	public class SoldierSpawner : SpawnerMonoWithPoolWithType<Soldier, SoldierType>
	{
		private Building _building;
		
		public void Init(Building building)
		{
			_building = building;
		}

		protected override bool CheckSpawnPointAvailability(SpawnPoint spawnPoint)
		{
			Tilemap gameAreaTileMap = PlacementManager.Instance.GameAreaTileMap;
			Vector3 position = spawnPoint.transform.position;
			Vector3Int cellPosition = gameAreaTileMap.WorldToCell(position);
			bool isAvailable = gameAreaTileMap.HasTile(cellPosition);
			return isAvailable;
		}

		[Button]
		public void SpawnSoldier(SoldierData soldierData)
		{
			SetSpawnType(soldierData.SoldierType);
			
			Spawn(newSoldier =>
			{
				//It gets object from pool, if there is no object then poolmanager spawns it, if there is then pops it
				SoldierTypeData typeData = GameManager.Instance.SoldierTeamData.
					GetSoldierTypeData(_building.GameUnit.TeamType, soldierData.SoldierType);
				newSoldier.Init(soldierData, _building.GameUnit.TeamType, typeData);
			});
		}
	}
}