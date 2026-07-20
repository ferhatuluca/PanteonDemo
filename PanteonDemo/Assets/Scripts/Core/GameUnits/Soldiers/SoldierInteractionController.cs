using System;
using System.Linq;
using Core.Enums;
using Core.Managers;
using Core.Utilities.Interact;
using Pathfinding;
using UnityEngine;

namespace Core.GameUnits.Soldiers
{
	[RequireComponent(typeof(AIPath))]
	[RequireComponent(typeof(Collider2D))]
	public class SoldierInteractionController : MultipleInteractable<GameUnit>
	{
		private Soldier _soldier;
		private bool _isFighting;

		private Rigidbody2D _rigidbody2D;
		private Collider2D _collider2D;
		
		private Transform _destination;
		private Transform _nonTargetDestination;
		
		public AIPath AiPath { private set; get; }
		public GameUnit TargetUnit { private set; get; }

		private void Awake()
		{
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_collider2D = GetComponent<Collider2D>();
			AiPath = GetComponent<AIPath>();
		}

		private void Start()
		{
			_nonTargetDestination = new GameObject($"{name}'s NonTargetDestination").transform;
			_nonTargetDestination.parent = GameManager.Instance.NonTargetDestinationsParent;
		}

		public void Init(Soldier soldier)
		{
			_soldier = soldier;
			_collider2D.enabled = true;
		}

		private void Update()
		{
			CheckDestinationReachAndUpdateDestination();
		}

		protected override bool ConditionAfterInteractionEnter(GameUnit actor)
		{
			// if interact is available and in same team as we are then no need to interact
			return actor.IsAvailableForInteract() && actor.TeamType != _soldier.GameUnit.TeamType;
		}

		protected override void OnTriggerInteract(GameUnit actor)
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
				return;
			}
			
			// TargetUnit != null, not having dest, and having TargetUnit should not be possible
			// If we have TargetUnit then we must have dest as TargetUnit's transform
			Debug.LogError($"TargetUnit should have been null: {TargetUnit}");
			if(TargetUnit is Component component)
				Debug.LogError($"self active : {component.gameObject.activeSelf} - active In Hierarchy : {component.gameObject.activeInHierarchy}");
		}

		protected override void OnTriggerInteractExit(GameUnit actor)
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
			if (!TargetUnit.IsDead())
			{
				ContinueChase();
				return;
			}
			
			// Target is destroyed and there is no target nearby
			// There is a another target nearby
			if (Interacts.Count > 0)
			{
				SetTargetUnit(Interacts.First());
				TriggerFight();
				return;
			}
			
			// There is no another target nearby
			// This will be check in OnTargetUnitDead
		}
		
		public void ResetForPool()
		{
			ClearInteracts();
			_rigidbody2D.velocity = Vector2.zero;
			_rigidbody2D.angularVelocity = 0f;
			_rigidbody2D.position = Vector2.zero;
			
			AiPath.canMove = false;
			_collider2D.enabled = false;
			_isFighting = false;
			SetDestination(null);
			SetTargetUnitNull();
		}

		public void SetDestinationEmptyArea(Vector2 position)
		{
			_nonTargetDestination.position = position;
			SetTargetUnitNull();
			SetDestination(_nonTargetDestination);
			_soldier.SoldierAnimController.SetAnim(SoldierAnimState.Run);
		}

		public void SetTargetUnit(GameUnit gameUnit)
		{
			if(TargetUnit == gameUnit)
				return;

			SetTargetUnitNull();
			TargetUnit = gameUnit;
			TargetUnit.OnDead += OnTargetUnitDead;
			SetDestination(TargetUnit.transform);
			if (!Interacts.Contains(gameUnit))
			{
				_soldier.SoldierAnimController.SetAnim(SoldierAnimState.Run);
			}
		}
		
		private void SetTargetUnitNull()
		{
			if(TargetUnit == null)
				return;
			
			TargetUnit.OnDead -= OnTargetUnitDead;
			TargetUnit = null;
		}

		private void OnTargetUnitDead()
		{
			if (TargetUnit == null)
			{
				Debug.LogError("This should not happen, we must unsubscribe to OnDead event if we change TargetUnit");
			}
			
			if (_destination != TargetUnit.transform)
			{
				Debug.LogError("This should not happen, if we have target unit then destination must be target unit");
			}
			
			if(Interacts.Count > 0)
				return;

			SetTargetUnitNull();
			SetDestination(null);
			_isFighting = false;
			_soldier.SoldierAnimController.SetAnim(SoldierAnimState.Idle);
		}

		private void SetDestination(Transform target)
		{
			_destination = target;
			if (_destination)
			{
				AiPath.destination = _destination.position;
				AiPath.canMove = true;
			}
			else
			{
				AiPath.canMove = false;
			}
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
		
		private void CheckDestinationReachAndUpdateDestination()
		{
			if (_destination == null || _isFighting)
				return;

			// if dest moved more than 1 unit
			float gap = Vector2.Distance(transform.position, _destination.position);
			if (gap > 0.5f)
			{
				AiPath.destination = _destination.position;
			}
			
			// did we reach the destination
			if (gap >= AiPath.endReachedDistance)
				return;
			
			SetDestination(null);
			_soldier.SoldierAnimController.SetAnim(SoldierAnimState.Idle);
		}
	}
}
