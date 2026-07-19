using System;
using System.Collections.Generic;
using Core.Utilities.Extensions;
using UnityEngine;

namespace Core.GameUnits.Buildings
{
	// It scripts for checking placement for building, whetever it is on gamearea or colliderarea
	public class PlacementColliderCheck : MonoBehaviour
	{
		public event Action<bool> OnPlaceabilityChanged;
		
		[SerializeField] private LayerMask _layerMask;
		
		private Collider2D _collider2D;
		private List<Collider2D> _hitColliders = new ();
		
		private void Awake()
		{
			_collider2D = GetComponent<Collider2D>();
		}

		public void Init()
		{
			_collider2D.enabled = true;
		}
		
		public void ResetForPool()
		{
			// This is only for placement so no need to let collider enabled
			_hitColliders.Clear();
			_collider2D.enabled = false;
			OnPlaceabilityChanged = null;
		}
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.gameObject.layer.LayerMaskLayerCompare(_layerMask))
				return;

			_hitColliders.Add(other);
			OnPlaceabilityChanged?.Invoke(false);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!other.gameObject.layer.LayerMaskLayerCompare(_layerMask))
				return;
			
			_hitColliders.Remove(other);
			OnPlaceabilityChanged?.Invoke(_hitColliders.Count == 0);
		}
	}
}