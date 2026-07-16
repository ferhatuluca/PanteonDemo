using System;
using Core.Enums;
using Core.GameUnits.Buildings;
using Core.Scriptables;
using Core.Utilities.Singleton;
using UI;
using UnityEngine;

namespace Core.Managers
{
	[RequireComponent(typeof(BuildingSpawner))]
	public class BuildingSpawnerManager : SingletonMonoBehaviour<BuildingSpawnerManager>
	{
		public static event Action<Building> OnNewBuildingSpawned;

		private BuildingSpawner _buildingSpawner;

		protected override void InternalAwake()
		{
			_buildingSpawner = GetComponent<BuildingSpawner>();
		}

		private void OnEnable()
		{
			BuildingUI.OnBuildingUIClicked += OnBuildingUIClicked;
		}

		private void OnDisable()
		{
			BuildingUI.OnBuildingUIClicked -= OnBuildingUIClicked;
		}

		public void SendBuildingToPool(Building building)
		{
			_buildingSpawner.SendBuildingToPool(building);
		}

		private void OnBuildingUIClicked(BuildingData buildingData, TeamType teamType)
		{
			Building newBuilding = _buildingSpawner.SpawnBuilding(buildingData, teamType);
			OnNewBuildingSpawned?.Invoke(newBuilding);
		}
	}
}