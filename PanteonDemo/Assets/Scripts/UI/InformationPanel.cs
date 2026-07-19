using System;
using Core.Enums;
using Core.GameUnits;
using Core.GameUnits.Buildings;
using Core.Managers;
using Core.Scriptables;
using DG.Tweening;
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
		[Header("Panel")] 
		[SerializeField] private RectTransform _unitsPanel;
		[SerializeField] private RectTransform _contentPanelTransform;
		
		[Header("Tween")]
		[SerializeField] private float _tweenDuration = 0.3f;
		[SerializeField] private Ease _ease = Ease.OutQuad;

		private SoldierUI[] _soldierUis;
		private float _startPosX;
		
		private RectTransform _rectTransform;
		private Tweener _currentTween;

		private void Start()
		{
			_rectTransform = GetComponent<RectTransform>();
			_startPosX = _rectTransform.anchoredPosition.x;

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
			if (gameUnit != null && gameUnit.GameUnitObject is Building building)
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
			CancelCurrentTween();
			SetUIs(building);
			_currentTween = _rectTransform.DOAnchorPosX(0f, _tweenDuration).SetEase(_ease);
		}

		private void ClosePanel()
		{
			CancelCurrentTween();
			_currentTween = _rectTransform.DOAnchorPosX(_startPosX, _tweenDuration).SetEase(_ease);
		}

		private void CancelCurrentTween()
		{
			if (_currentTween != null && _currentTween.IsActive() && !_currentTween.IsComplete())
			{
				_currentTween.Kill();
				_currentTween = null;
			}
		}

		private void SetUIs(Building building)
		{
			TeamType teamType = building.GameUnit.TeamType;
			BuildingData buildingData = GameManager.Instance.GeneralData.GetBuildingData(building.BuildingType);
			_buildingName.text = buildingData.Name;
			_buildingHP.text = $"HP: {buildingData.Health}";
			_buildingGridCellSize.text = $"Grid: {buildingData.GridCellSize.x}x{buildingData.GridCellSize.y}";
				
			BuildingTeamData buildingTeamData = buildingData.GetBuildingTeamData(teamType);
			_buildingIcon.sprite = buildingTeamData.Icon;

			if (!building.IsUnitProducingBuilding())
			{
				_unitsPanel.gameObject.SetActive(false);
				return;
			}
			
			_unitsPanel.gameObject.SetActive(true);
			SoldierData[] soldierData = GameManager.Instance.GeneralData.SoldierData;
			for (int i = 0; i < soldierData.Length; i++)
			{
				_soldierUis[i].Init(soldierData[i], teamType);
			}
		}
	}
}