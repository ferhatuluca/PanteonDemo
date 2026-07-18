using System.Collections.Generic;
using Core.Utilities.Extensions;
using Pathfinding;
using UnityEngine;

namespace Core.GameUnits.Buildings
{
	public class BuildingPlaceChecker : MonoBehaviour
	{
		[SerializeField] private LayerMask _layerMask;

		private PlacementColliderCheck _placementColliderCheck;

		private Collider2D _parentCollider2D;
		private Collider2D _thisCollider2D;
		private List<Collider2D> _hitColliders = new ();
		private bool _canBePlaced = true;

		public bool IsPlaced { private set; get; }
		public bool CanBePlaced() => _canBePlaced && _placementColliderCheck.CanBePlaced;
		
		private void Awake()
		{
			_thisCollider2D = GetComponent<Collider2D>();
			_placementColliderCheck = GetComponentInChildren<PlacementColliderCheck>();
		}

		public void Init(Collider2D parent)
		{
			_parentCollider2D = parent;
			_thisCollider2D.enabled = true;
			_canBePlaced = true;
			IsPlaced = false;
			_placementColliderCheck.Init();
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
		
		public void ResetForPool()
		{
			_hitColliders.Clear();
			_thisCollider2D.enabled = false;
			_placementColliderCheck.ResetForPool();
			UpdateBuildingGraphs();
		}

		public void Place()
		{
			IsPlaced = true;
			UpdateBuildingGraphs();
		}

		private void UpdateBuildingGraphs()
		{
			Bounds bounds = _parentCollider2D.bounds;
			AstarPath.active.UpdateGraphs(bounds);
		}
	}
}