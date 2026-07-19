using System.Collections;
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
		
		// we have 2 collider, one is trigger for place checking, other is non trigger for Aster path finder
		private Collider2D[] _colliders;
		private List<Collider2D> _hitColliders = new ();
		
		private bool _thisPlaceability = true;
		private bool _placementColliderPlaceability = true;

		public bool IsPlaced { private set; get; }
		public bool CanBePlaced() => _thisPlaceability && _placementColliderPlaceability;
		
		private void Awake()
		{
			_colliders = GetComponents<Collider2D>();
			_placementColliderCheck = GetComponentInChildren<PlacementColliderCheck>();
		}

		public void Init(SpriteRenderer spriteRenderer)
		{
			_thisPlaceability = true;
			_placementColliderPlaceability = true;
			IsPlaced = false;
			
			foreach (Collider2D c in _colliders)
				c.enabled = true;
			
			_buildingSprite = spriteRenderer;
			_startColorOfBuildingSprite = _buildingSprite.color;
			
			_placementColliderCheck.Init();
			_placementColliderCheck.OnPlaceabilityChanged += OnPlaceabilityChanged;
		}

		public void ResetForPool()
		{
			// we need to disable it now because after that we scan path, for that it is necessary
			foreach (Collider2D c in _colliders)
				c.enabled = false;
			
			AstarPath.active.Scan();
		}
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			// We could have disabled the _colliders instead of checking IsPlaced, but in that case new spawned
			// building will not interact with this, and It will be able to place on this
			if(IsPlaced)
				return;
			
			if (!other.gameObject.layer.LayerMaskLayerCompare(_layerMask))
				return;

			_hitColliders.Add(other);
			ChangeThisPlaceability(false);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			// We could have disabled the _colliders instead of checking IsPlaced, but in that case new spawned
			// building will not interact with this and It will be able to place on this
			if(IsPlaced)
				return;
			
			if (!other.gameObject.layer.LayerMaskLayerCompare(_layerMask))
				return;
			
			_hitColliders.Remove(other);
			ChangeThisPlaceability(_hitColliders.Count == 0);
		}

		public void Place()
		{
			IsPlaced = true;
			// both colliders have same bounds
			Bounds bounds = _colliders[0].bounds;
			AstarPath.active.UpdateGraphs(bounds);

			StartCoroutine(OneFrame());
		}

		private IEnumerator OneFrame()
		{
			yield return null;
			_hitColliders.Clear();
			_placementColliderCheck.ResetForPool();
			_placementColliderCheck.OnPlaceabilityChanged -= OnPlaceabilityChanged;
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
	}
}