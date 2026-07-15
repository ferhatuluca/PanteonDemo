using System;
using Core.GameUnits.Soldiers;
using Core.Types;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Managers
{
	public class GameUnitClickManager : MonoBehaviour
	{
		public static event Action<GameUnitClickType> OnGameUnitClicked;
		
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
			{
				Vector3 clickPosition = Input.mousePosition;
				clickPosition.z = _mainCam.transform.position.z;
				Vector2 worldClickPosition = _mainCam.ScreenToWorldPoint(clickPosition);

				RaycastHit2D hit = Physics2D.Raycast(worldClickPosition, Vector2.zero, 
					Mathf.Infinity, _clickLayerMask);

				if (hit.collider != null)
				{
					IClickableGameUnit unit = hit.collider.GetComponent<IClickableGameUnit>();
					if (unit != null)
					{
						unit.OnSelect();
					}
					else // Clicked on other object
					{
						OnGameUnitClicked?.Invoke(GameUnitClickType.Empty);
					}
				}
				else //Clicked on empty
				{
					OnGameUnitClicked?.Invoke(GameUnitClickType.Empty);
				}
			}
			else if (_clickedSoldier && Input.GetMouseButtonDown(1))
			{
				
			}
		}
	}
}