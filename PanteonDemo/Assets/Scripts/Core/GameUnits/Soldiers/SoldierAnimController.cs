using Core.Enums;
using Core.Scriptables;
using UnityEngine;

namespace Core.GameUnits.Soldiers
{
	public class SoldierAnimController : MonoBehaviour
	{
		[SerializeField] private float _directionSetInterval = 0.2f;
		
		private Soldier _soldier;
		private Animator _animator;
		private SpriteRenderer _spriteRenderer;
			
		private SoldierAnimState _currentSoldierAnimState = SoldierAnimState.Idle;
		private bool _isRunning;
		private float _directionSetTimer;
		
		private static readonly int Run = Animator.StringToHash("Run");
		private static readonly int RunTrigger = Animator.StringToHash("RunTrigger");
		private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
		private static readonly int IdleTrigger = Animator.StringToHash("IdleTrigger");

		public void Init(Soldier soldier, SoldierTypeData soldierTypeData)
		{
			_soldier = soldier;
			_spriteRenderer = GetComponent<SpriteRenderer>();
			
			_animator = GetComponent<Animator>();
			_animator.runtimeAnimatorController = soldierTypeData.Controller;
		}

		private void Update()
		{
			_directionSetTimer += Time.deltaTime;

			if (!(_directionSetTimer >= _directionSetInterval)) 
				return;
			
			_directionSetTimer -= _directionSetInterval;
			SetSpriteDirection();
		}

		public void SetAnim(SoldierAnimState soldierAnimState)
		{
			SoldierAnimState prevState = _currentSoldierAnimState;
			_currentSoldierAnimState = soldierAnimState;
			
			if(prevState == _currentSoldierAnimState)
				return;

			if (prevState is SoldierAnimState.Idle or SoldierAnimState.Run)
			{
				if(_currentSoldierAnimState is SoldierAnimState.Idle or SoldierAnimState.Run)
					_animator.SetBool(Run, _currentSoldierAnimState == SoldierAnimState.Run);
				else
					_animator.SetTrigger(GetTrigger(soldierAnimState));
			}
			else
			{
				if (_currentSoldierAnimState is SoldierAnimState.Idle or SoldierAnimState.Run)
					_animator.SetBool(Run, _currentSoldierAnimState == SoldierAnimState.Run);
				
				_animator.SetTrigger(GetTrigger(_currentSoldierAnimState));
			}
		}

		private int GetTrigger(SoldierAnimState state)
		{
			if (state == SoldierAnimState.Attack)
				return AttackTrigger;
			if (state == SoldierAnimState.Run)
				return RunTrigger;
			if (state == SoldierAnimState.Idle)
				return IdleTrigger;

			return -1;
		}

		private void SetSpriteDirection()
		{
			bool flipX = false;

			if (_currentSoldierAnimState is SoldierAnimState.Attack)
			{
				float targetWorldXPos = _soldier.SoldierInteractionController.TargetUnit.GetTransform().position.x;
				float soldierWorldXPos = transform.position.x;

				flipX = !(targetWorldXPos > soldierWorldXPos);
			}
			else
			{
				flipX = !(_soldier.SoldierInteractionController.AiPath.desiredVelocity.x > 0.01f);

			}

			_spriteRenderer.flipX = flipX;
		}
	}
}