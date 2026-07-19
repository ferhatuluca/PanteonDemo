using Core.Enums;
using Core.GameUnits.Soldiers;
using Core.Managers;
using Core.Scriptables;
using Core.Utilities.Extensions;
using Core.Utilities.Pool_Spawner.Spawner;
using Core.Utilities.Pool_Spawner.Spawner.SpawnerWithPool;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Core.GameUnits.Buildings
{
	public class SoldierSpawner : SpawnerMonoWithPoolWithType<Soldier, SoldierType>
	{
		[SerializeField] private Tilemap _gameAreaTilemap;
		[SerializeField] private Tilemap _colliderAreaTilemap;
		[SerializeField] private LayerMask _buildingLayerMask;

		private Building _building;
		
		public void Init(Building building)
		{
			_building = building;
		}

		protected override bool CheckSpawnPointAvailability(SpawnPoint spawnPoint)
		{
			Vector3 position = spawnPoint.transform.position;
			return IsOnGameArea(position) && !IsOnColliderArea(position) && !IsBlockedByBuilding(position);
		}

		private bool IsOnGameArea(Vector3 worldPosition)
		{
			Vector3Int cellPosition = _gameAreaTilemap.WorldToCell(worldPosition);
			return _gameAreaTilemap.HasTile(cellPosition);
		}

		private bool IsOnColliderArea(Vector3 worldPosition)
		{
			Vector3Int cellPosition = _colliderAreaTilemap.WorldToCell(worldPosition);
			return _colliderAreaTilemap.HasTile(cellPosition);
		}

		private bool IsBlockedByBuilding(Vector3 worldPosition)
		{
			return Physics2D.OverlapPoint(worldPosition, _buildingLayerMask) != null;
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