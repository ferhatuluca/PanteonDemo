using System;
using Core.Enums;
using Core.GameUnits.Buildings;
using Core.Scriptables;
using Core.Utilities.Singleton;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;

namespace Core.Managers
{
	[RequireComponent(typeof(BuildingSpawner))]
	public class BuildingSpawnerManager : SingletonMonoBehaviour<BuildingSpawnerManager>
	{
		public static event Action<Building> OnNewBuildingSpawned;

		private BuildingSpawner _buildingSpawner;

#if UNITY_EDITOR
		[Header("Test")]
		public BuildingData testBuildingData;
		public TeamType testTeamType;
#endif

		protected override void InternalAwake()
		{
			_buildingSpawner = GetComponent<BuildingSpawner>();
		}

#if UNITY_EDITOR
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
				OnBuildingUIClicked(testBuildingData, testTeamType);
		}
#endif

		private void OnEnable()
		{
			BuildingUI.OnBuildingUIClicked += OnBuildingUIClicked;
		}

		private void OnDisable()
		{
			BuildingUI.OnBuildingUIClicked -= OnBuildingUIClicked;
		}

		[Button]
		private void OnBuildingUIClicked(BuildingData buildingData, TeamType teamType)
		{
			Building newBuilding = _buildingSpawner.SpawnBuilding(buildingData, teamType);
			OnNewBuildingSpawned?.Invoke(newBuilding);
		}
	}
}