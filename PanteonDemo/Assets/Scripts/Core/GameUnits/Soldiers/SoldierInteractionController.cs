using Core.Enums;
using Core.Utilities.Interact;
using Pathfinding;
using UnityEngine;

namespace Core.GameUnits.Soldiers
{
	[RequireComponent(typeof(AIPath))]
	[RequireComponent(typeof(Collider2D))]
	public class SoldierInteractionController : MultipleInteractable<IClickableGameUnit>
	{
		private Soldier _soldier;
		private bool _isFighting;
		
		private Transform _destination;
		
		public AIPath AiPath { private set; get; }
		public IClickableGameUnit TargetUnit { private set; get; }
		
		public void Init(Soldier soldier)
		{
			_soldier = soldier;
			AiPath = GetComponent<AIPath>();
		}

		private void Update()
		{
			CheckIfReachedDestination();
		}

		protected override bool ConditionAfterInteractionEnter(IClickableGameUnit actor)
		{
			// if interact is in same team as we are then no need to interact
			return actor.GetTeamType() != _soldier.GetTeamType();
		}

		protected override void OnTriggerInteract(IClickableGameUnit actor)
		{
			if (_destination)
			{
				// If we have don't have target or target is not equal to interacted unit then continue to move
				if(TargetUnit == null || TargetUnit != actor)
					return;
				
				// we reached and interacted with our target and fight
				TriggerFight();
				return;
			}
			
			// We were doing nothing, but target is appeared nearby
			if (TargetUnit == null)
			{
				SetTargetUnit(actor);
				TriggerFight();
			}
			
			// TargetUnit != null, not having dest, and having TargetUnit should not be possible
			// If we have TargetUnit then we must have dest as TargetUnit's transform
			Debug.LogError($"TargetUnit should have been null: {TargetUnit}");
			if(TargetUnit is Component component)
				Debug.LogError($"self active : {component.gameObject.activeSelf} - active In Hierarchy : {component.gameObject.activeInHierarchy}");
		}

		protected override void OnTriggerInteractExit(IClickableGameUnit actor)
		{
			if (!_destination)
			{
				// We don't have dest then we should not interact so Exit shouldn't have been possible
				Debug.LogError($"This should not happen {TargetUnit}", gameObject);
				return;
			}
			
			// If we have don't have target or target is not equal to Exited unit then continue to move
			if (TargetUnit == null || TargetUnit != actor)
				return;

			// We have target, we were fighting, but he is running away now
			if (TargetUnit.IsAlive())
			{
				ContinueChase();
				return;
			}
			
			// If interacted target is destroyed then we look if there is another interact nearby
			if (Interacts.Count <= 0)
			{
				StopFight();
				return;
			}
				
			SetTargetUnit(Interacts[0]);
			TriggerFight();
		}
		
		public void ResetForPool()
		{
			ClearInteracts();
			_destination = null;
			AiPath.canMove = false;
			TargetUnit = null;
			_isFighting = false;
		}

		public void SetDestinationEmptyArea(Transform target)
		{
			TargetUnit = null;
			SetDestination(target);
			_soldier.SoldierAnimController.SetAnim(SoldierAnimState.Run);
		}

		public void SetTargetUnit(IClickableGameUnit gameUnit)
		{
			if(TargetUnit == gameUnit)
				return;
			
			TargetUnit = gameUnit;
			SetDestination(TargetUnit.GetTransform());
			if (!Interacts.Contains(gameUnit))
			{
				_soldier.SoldierAnimController.SetAnim(SoldierAnimState.Run);
			}
		}
		
		private void SetDestination(Transform target)
		{
			_destination = target;
			AiPath.destination = _destination.position;
		}

		private void ContinueChase()
		{
			_isFighting = false;
			AiPath.canMove = true;
			_soldier.SoldierAnimController.SetAnim(SoldierAnimState.Run);
		}
		
		private void TriggerFight()
		{
			_isFighting = true;
			AiPath.canMove = false;
			_soldier.SoldierAnimController.SetAnim(SoldierAnimState.Attack);
		}

		private void StopFight()
		{
			_isFighting = false;
			_soldier.SoldierAnimController.SetAnim(SoldierAnimState.Idle);
		}
		
		private void CheckIfReachedDestination()
		{
			if (_destination == null) 
				return;
			
			if (_isFighting)
				return;

			if (Vector2.Distance(transform.position, _destination.position) >= AiPath.endReachedDistance)
				return;
			
			_destination = null;
			AiPath.canMove = false;
			_soldier.SoldierAnimController.SetAnim(SoldierAnimState.Idle);
		}
	}
}