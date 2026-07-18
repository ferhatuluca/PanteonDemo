using System.Collections.Generic;
using Core.Utilities.Extensions;
using UnityEngine;

namespace Core.GameUnits.Buildings
{
	public class PlacementColliderCheck : MonoBehaviour
	{
		[SerializeField] private LayerMask _layerMask;
		
		private Collider2D _collider2D;
		private HashSet<Collider2D> _hitColliders = new ();
		
		public bool CanBePlaced { private set; get; } = true;
		
		private void Awake()
		{
			_collider2D = GetComponent<Collider2D>();
		}

		public void Init()
		{
			_collider2D.enabled = true;
			CanBePlaced = true;
		}

		public void ResetForPool()
		{
			_hitColliders.Clear();
			_collider2D.enabled = false;
		}
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.gameObject.layer.LayerMaskLayerCompare(_layerMask))
				return;

			_hitColliders.Add(other);
			CanBePlaced = false;
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!other.gameObject.layer.LayerMaskLayerCompare(_layerMask))
				return;
			
			_hitColliders.Remove(other);
			CanBePlaced = _hitColliders.Count == 0;
		}
	}
}