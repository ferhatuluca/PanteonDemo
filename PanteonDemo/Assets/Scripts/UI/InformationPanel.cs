using System;
using Core.Enums;
using Core.GameUnits;
using Core.GameUnits.Buildings;
using Core.Managers;
using Core.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class InformationPanel : MonoBehaviour
	{
		[SerializeField] private Image _buildingIcon;
		[SerializeField] private TextMeshProUGUI _buildingName;
		[SerializeField] private TextMeshProUGUI _buildingHP;
		[SerializeField] private TextMeshProUGUI _buildingGridCellSize;
		[SerializeField] private RectTransform _contentPanelTransform;

		private SoldierUI[] _soldierUis;
		
		private void Start()
		{
			SoldierType[] soldierTypes = (SoldierType[])Enum.GetValues(typeof(SoldierType));
			_soldierUis = new SoldierUI[soldierTypes.Length];
			
			SoldierData[] soldierData = GameManager.Instance.GeneralData.SoldierData;

			for (int i = 0; i < soldierData.Length; i++)
			{
				_soldierUis[i] = Instantiate(soldierData[i].SoldierUIPrefab, _contentPanelTransform);
			}
		}

		private void OnEnable()
		{
			GameUnitClickManager.OnGameUnitClicked += OnGameUnitClicked;
		}

		private void OnDisable()
		{
			GameUnitClickManager.OnGameUnitClicked -= OnGameUnitClicked;
		}
		
		private void OnGameUnitClicked(GameUnit gameUnit)
		{
			if (gameUnit.GameUnitObject is Building building && building.IsUnitProducingBuilding())
			{
				OpenPanel(building);
			}
			else
			{
				ClosePanel();
			}
		}

		private void OpenPanel(Building building)
		{
			SetUIs(building);
		}

		private void ClosePanel()
		{
			
		}

		private void SetUIs(Building building)
		{
			BuildingData buildingDataBase = GameManager.Instance.GeneralData.GetBuildingData(building.BuildingType);
			UnitProducingBuildingData buildingData = buildingDataBase as UnitProducingBuildingData;
			_buildingName.text = buildingData.Name;
			_buildingHP.text = $"HP: {buildingData.Health}";
			_buildingGridCellSize.text = $"Grid: {buildingData.GridCellSize.x}x{buildingData.GridCellSize.y}";
				
			BuildingTeamData buildingTeamData = buildingData.GetBuildingTeamData(building.GameUnit.TeamType);
			_buildingIcon.sprite = buildingTeamData.Icon;
			
			SoldierData[] soldierData = GameManager.Instance.GeneralData.SoldierData;

			for (int i = 0; i < soldierData.Length; i++)
			{
				_soldierUis[i].Init(soldierData[i], building.GameUnit.TeamType);
			}
		}
	}
}