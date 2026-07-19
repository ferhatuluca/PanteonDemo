using System.Collections.Generic;
using Core.Utilities.Extensions;
using UnityEngine;

namespace Core.GameUnits.Buildings
{
	// This check if there are other building or soldier on building that will be placed
	// PlacementColliderCheck look for unavailable game area which is ColliderArea
	// This is implemented this way because PlacementColliderCheck has seperate collider for ColliderArea check
	public class BuildingPlaceChecker : MonoBehaviour
	{
		[SerializeField] private LayerMask _layerMask;
		[SerializeField] private Color _cantPlaceColor;

		private PlacementColliderCheck _placementColliderCheck;

		private SpriteRenderer _buildingSprite;
		private Color _startColorOfBuildingSprite;
		
		private Collider2D _thisCollider2D;
		private List<Collider2D> _hitColliders = new ();
		
		private bool _thisPlaceability = true;
		private bool _placementColliderPlaceability = true;

		public bool IsPlaced { private set; get; }
		public bool CanBePlaced() => _thisPlaceability && _placementColliderPlaceability;
		
		private void Awake()
		{
			_thisCollider2D = GetComponent<Collider2D>();
			_placementColliderCheck = GetComponentInChildren<PlacementColliderCheck>();
		}

		public void Init(SpriteRenderer spriteRenderer)
		{
			_thisCollider2D.enabled = true;
			_thisPlaceability = true;
			_placementColliderPlaceability = true;
			IsPlaced = false;
			
			_buildingSprite = spriteRenderer;
			_startColorOfBuildingSprite = _buildingSprite.color;
			
			_placementColliderCheck.Init();
			_placementColliderCheck.OnPlaceabilityChanged += OnPlaceabilityChanged;
		}

		public void ResetForPool()
		{
			_hitColliders.Clear();
			_thisCollider2D.enabled = false;
			_placementColliderCheck.ResetForPool();
			AstarPath.active.Scan();
		}
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.gameObject.layer.LayerMaskLayerCompare(_layerMask))
				return;

			_hitColliders.Add(other);
			ChangeThisPlaceability(false);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!other.gameObject.layer.LayerMaskLayerCompare(_layerMask))
				return;
			
			_hitColliders.Remove(other);
			ChangeThisPlaceability(_hitColliders.Count == 0);
		}

		public void Place()
		{
			IsPlaced = true;
			UpdateBuildingGraphs();
		}
		
		private void ChangeThisPlaceability(bool canPlace)
		{
			_thisPlaceability = canPlace;
			ChangeGeneralAvailability();
		}
		
		private void OnPlaceabilityChanged(bool canPlace)
		{
			_placementColliderPlaceability = canPlace;
			ChangeGeneralAvailability();
		}

		private void ChangeGeneralAvailability()
		{
			if (_thisPlaceability && _placementColliderPlaceability)
			{
				_buildingSprite.color = _startColorOfBuildingSprite;
			}
			else
			{
				_buildingSprite.color = _cantPlaceColor;
			}
		}
		
		private void UpdateBuildingGraphs()
		{
			Bounds bounds = _thisCollider2D.bounds;
			AstarPath.active.UpdateGraphs(bounds);
		}
	}
}