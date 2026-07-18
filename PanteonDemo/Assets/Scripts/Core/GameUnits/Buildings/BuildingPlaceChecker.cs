using System.Collections.Generic;
using Core.Utilities.Extensions;
using UnityEngine;

namespace Core.GameUnits.Buildings
{
	public class BuildingPlaceChecker : MonoBehaviour
	{
		[SerializeField] private LayerMask _layerMask;

		private PlacementColliderCheck _placementColliderCheck;
		
		private Collider2D _collider2D;
		private List<Collider2D> _hitColliders = new ();
		private bool _canBePlaced = true;

		public bool IsPlaced { private set; get; }
		public bool CanBePlaced() => _canBePlaced && _placementColliderCheck.CanBePlaced;
		
		private void Awake()
		{
			_collider2D = GetComponent<Collider2D>();
			_placementColliderCheck = GetComponentInChildren<PlacementColliderCheck>();
		}

		public void Init()
		{
			_collider2D.enabled = true;
			_canBePlaced = true;
			IsPlaced = false;
			_placementColliderCheck.Init();
		}

		public void ResetForPool()
		{
			_hitColliders.Clear();
			_collider2D.enabled = false;
			_placementColliderCheck.ResetForPool();
		}

		public void Place()
		{
			IsPlaced = true;
		}
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.gameObject.layer.LayerMaskLayerCompare(_layerMask))
				return;

			_hitColliders.Add(other);
			_canBePlaced = false;
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!other.gameObject.layer.LayerMaskLayerCompare(_layerMask))
				return;
			
			_hitColliders.Remove(other);
			_canBePlaced = _hitColliders.Count == 0;
		}
	}
}