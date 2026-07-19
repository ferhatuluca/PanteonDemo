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
		[SerializeField] private LayerMask _nonAvailableMask;
		
		private Building _building;
		
		public void Init(Building building)
		{
			_building = building;
		}
		
		[Button]
		public void SpawnSoldier(SoldierData soldierData)
		{
			SetSpawnType(soldierData.SoldierType);
			
			Spawn(newSoldier =>
			{
				//It gets object from pool, if there is no object then poolmanager spawns it, if there is then pops it
				SoldierTeamTypeData teamTypeData = soldierData.GetSoldierTeamData(_building.GameUnit.TeamType);
				newSoldier.Init(soldierData, _building.GameUnit.TeamType, teamTypeData);
			});
		}

		protected override bool CheckSpawnPointAvailability(SpawnPoint spawnPoint)
		{
			Vector3 position = spawnPoint.transform.position;
			return CheckGameArea(position) && CheckPhysics(position);
		}

		// if spawn point on unavailable are like water or other tilemaps
		private bool CheckGameArea(Vector3 spawnPosition)
		{
			Tilemap gameAreaTileMap = PlacementManager.Instance.GameAreaTileMap;
			Vector3Int cellPosition = gameAreaTileMap.WorldToCell(spawnPosition);
			return gameAreaTileMap.HasTile(cellPosition);
		}

		// If there is an object on spawn point
		private bool CheckPhysics(Vector3 spawnPosition)
		{
			Collider2D[] overlapColliders = Physics2D.OverlapPointAll(spawnPosition, _nonAvailableMask);
			foreach (Collider2D overlapCollider in overlapColliders)
			{
				Building overLap = overlapCollider.GetComponent<Building>();
				if (overLap != null && overLap != _building)
					return false;
			}
			return true;
		}
	}
}