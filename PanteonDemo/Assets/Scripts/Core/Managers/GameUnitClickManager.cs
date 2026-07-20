using System;
using Core.Enums;
using Core.GameUnits;
using Core.GameUnits.Buildings;
using Core.GameUnits.Soldiers;
using Core.Scriptables;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Managers
{
	public class GameUnitClickManager : MonoBehaviour
	{
		public static event Action<GameUnit> OnGameUnitClicked;
		
		[SerializeField] private LayerMask _clickLayerMask;
		
		private Camera _mainCam;
		private GameUnit _selectedGameUnit;

		private void Awake()
		{
			_mainCam = Camera.main;
		}

		private void Update()
		{
			if(EventSystem.current.IsPointerOverGameObject())
				return;
			
			CheckClick();
		}
		
		private void OnEnable()
		{
			SoldierUI.OnSoldierUIClicked += OnSoldierUIClicked;
		}

		private void OnDisable()
		{
			SoldierUI.OnSoldierUIClicked -= OnSoldierUIClicked;
		}

		private void OnSoldierUIClicked(SoldierData data, TeamType teamType)
		{
			if (_selectedGameUnit == null)
			{
				Debug.LogError("Information panel is open but there is no selected game unit");
				return;
			}
			
			if (_selectedGameUnit.GameUnitObject is not Building building)
			{
				Debug.LogError("No building is selected but information panel open and soldier clicked");
				return;
			}

			if (!building.IsUnitProducingBuilding())
			{
				Debug.LogError($"Selected building is {building.BuildingType} but it can't not spawn soldier");
				return;
			}

			if (building.GameUnit.TeamType != teamType)
			{
				Debug.LogError($"Clicked soldier's team is {teamType} but selected building is {building.GameUnit.TeamType}");
			}
			
			building.SoldierSpawner.SpawnSoldier(data);
		}

		private void CheckClick()
		{
			if (Input.GetMouseButtonDown(0))
				LeftClick();
			else if (Input.GetMouseButtonDown(1) && _selectedGameUnit && _selectedGameUnit.GameUnitObject is Soldier soldier)
				SoldierSelectedAndRightClick(soldier);
		}

		private void LeftClick()
		{
			Vector3 clickPosition = Input.mousePosition;
			clickPosition.z = _mainCam.transform.position.z;
			Vector2 worldClickPosition = _mainCam.ScreenToWorldPoint(clickPosition);

			RaycastHit2D hit = Physics2D.Raycast(worldClickPosition, Vector2.zero, 
				Mathf.Infinity, _clickLayerMask);

			if (hit.collider == null)
			{
				_selectedGameUnit = null;
				OnGameUnitClicked?.Invoke(null);
				return;
			}
			
			GameUnit hitGameUnit = hit.collider.GetComponent<GameUnit>();
			if (hitGameUnit == null)
			{
				SetSelectedGameUnit(null);
				return;
			}

			// Haven't placed the building yet
			if (hitGameUnit.GameUnitObject is Building building && !building.BuildingPlaceChecker.IsPlaced)
			{
				return;
			}

			SetSelectedGameUnit(hitGameUnit);
		}

		private void SetSelectedGameUnit(GameUnit gameUnit)
		{
			if (gameUnit)
			{
				if(gameUnit == _selectedGameUnit)
					return;

				_selectedGameUnit = gameUnit;
				OnGameUnitClicked?.Invoke(_selectedGameUnit);
				_selectedGameUnit.OnDead += OnSoldierDead;
			}
			else
			{
				if (_selectedGameUnit)
					_selectedGameUnit.OnDead -= OnSoldierDead;
				_selectedGameUnit = null;
			}
		}

		private void OnSoldierDead()
		{
			SetSelectedGameUnit(null);
		}

		private void SoldierSelectedAndRightClick(Soldier soldier)
		{
			Vector2 clickPosition = PlacementManager.Instance.CurrentHoveredGridCell;
			RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero, Mathf.Infinity, _clickLayerMask);

			if (hit.collider == null)
			{
				MoveToEmpty(soldier, clickPosition);
				return;
			}

			GameUnit gameUnit = hit.collider.GetComponent<GameUnit>();
			if (gameUnit == null)
			{
				MoveToEmpty(soldier, clickPosition);
				return;
			}
			
			// if we right click to ally soldier then no need to do anything
			if(gameUnit.TeamType == soldier.GameUnit.TeamType)
				return;
			
			soldier.SoldierInteractionController.SetTargetUnit(gameUnit);
		}

		private void MoveToEmpty(Soldier soldier, Vector2 clickPos)
		{
			soldier.SoldierInteractionController.SetDestinationEmptyArea(clickPos);
		}
	}
}