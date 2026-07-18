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
		public static event Action<GameUnitClickType> OnGameUnitClicked;
		
		[SerializeField] private Transform _nonTargetDestination;
		[SerializeField] private LayerMask _clickLayerMask;
		
		private Camera _mainCam;
		private GameUnit _selectedGameUnit;

		private void Awake()
		{
			_mainCam = Camera.main;
		}

		private void Update()
		{
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
			if (_selectedGameUnit.GameUnitObject is not Building building)
			{
				Debug.LogError("No building is selected but information panel open and soldier clicked");
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
			if (EventSystem.current.IsPointerOverGameObject()) 
				return;
			
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
				OnGameUnitClicked?.Invoke(GameUnitClickType.Empty);
				return;
			}
			
			_selectedGameUnit = hit.collider.GetComponent<GameUnit>();
			if (_selectedGameUnit == null)
			{
				OnGameUnitClicked?.Invoke(GameUnitClickType.Empty);
				return;
			}
			
			OnGameUnitClicked?.Invoke(_selectedGameUnit.GameUnitObject is Soldier 
				? GameUnitClickType.Soldier : GameUnitClickType.Building);
		}

		private void SoldierSelectedAndRightClick(Soldier soldier)
		{
			Vector2 clickPosition = PlacementManager.Instance.CurrentHoveredGridCellWorldPos;
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
			_nonTargetDestination.transform.position = clickPos;
			soldier.SoldierInteractionController.SetDestinationEmptyArea(_nonTargetDestination);
		}
	}
}