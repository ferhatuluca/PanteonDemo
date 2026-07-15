using System;
using Pathfinding;
using UnityEngine;

namespace Core
{
	[RequireComponent(typeof(AIPath))]
	[RequireComponent(typeof(Collider2D))]
	public class SoldierPathFinder : MonoBehaviour
	{
		private Soldier _soldier;
		private AIPath _aiPath;
		private Collider2D _collider2D;

		private Transform _targetDestination;
		
		public void Init(Soldier soldier)
		{
			_soldier = soldier;
			_aiPath = GetComponent<AIPath>();
			_collider2D = GetComponent<Collider2D>();
		}

		private void Update()
		{
			throw new NotImplementedException();
		}
		
		public void SetNewDestination(Transform target)
		{
			_targetDestination = target;
			_aiPath.destination = _targetDestination.position;
		}
		
		private void CheckEndReach()
		{
			if (_targetDestination == null) 
				return;

			if (Vector2.Distance(transform.position, _targetDestination.position) < _aiPath.endReachedDistance)
			{
				ResetMovement();
			}
		}

		public void ResetForPool()
		{
			ResetMovement();
		}

		private void ResetMovement()
		{
			_targetDestination = null;
			_aiPath.canMove = false;
		}
	}
}