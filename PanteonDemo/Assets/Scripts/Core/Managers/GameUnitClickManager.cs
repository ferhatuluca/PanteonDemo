using System;
using Core.Enums;
using Core.GameUnits;
using Core.GameUnits.Soldiers;
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
		private Soldier _clickedSoldier = null;

		private void Awake()
		{
			_mainCam = Camera.main;
		}

		private void Update()
		{
			CheckClick();
		}

		private void CheckClick()
		{
			if (EventSystem.current.IsPointerOverGameObject()) 
				return;
			
			if (Input.GetMouseButtonDown(0))
				LeftClick();				
			else if (_clickedSoldier && Input.GetMouseButtonDown(1))
				RightClick();
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
				OnGameUnitClicked?.Invoke(GameUnitClickType.Empty);
				return;
			}
			
			GameUnit unit = hit.collider.GetComponent<GameUnit>();
			if (unit == null)
			{
				OnGameUnitClicked?.Invoke(GameUnitClickType.Empty);
				return;
			}
			
			if (unit.GameUnitObject is Soldier soldier)
				_clickedSoldier = soldier;
		}

		private void RightClick()
		{
			Vector2 clickPosition = PlacementManager.Instance.CurrentHoveredGridCellWorldPos;
			RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero, Mathf.Infinity, _clickLayerMask);

			if (hit.collider == null)
			{
				MoveToEmpty(clickPosition);
				return;
			}

			GameUnit gameUnit = hit.collider.GetComponent<GameUnit>();
			if (gameUnit == null)
			{
				MoveToEmpty(clickPosition);
				return;
			}
			
			if(gameUnit.TeamType == _clickedSoldier.GameUnit.TeamType)
				return;
			
			_clickedSoldier.SoldierInteractionController.SetTargetUnit(gameUnit);
		}

		private void MoveToEmpty(Vector2 clickPos)
		{
			_nonTargetDestination.transform.position = clickPos;
			_clickedSoldier.SoldierInteractionController.SetDestinationEmptyArea(_nonTargetDestination);
		}
	}
}