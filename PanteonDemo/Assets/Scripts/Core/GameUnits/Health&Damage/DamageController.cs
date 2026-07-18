using Core.GameUnits.Soldiers;
using UnityEngine;

namespace Core.GameUnits.Health_Damage
{
	public class DamageController : MonoBehaviour
	{
		[SerializeField, Min(0f)] private float _attackInterval = 0.5f;

		private Soldier _soldier;
		private int _damage;
		private float _attackTimer;

		public void Init(Soldier soldier, int damage)
		{
			_soldier = soldier;
			_damage = damage;
			_attackTimer = 0f;
		}

		private void Update()
		{
			if (_soldier == null || !_soldier.SoldierInteractionController.IsFighting)
				return;

			_attackTimer += Time.deltaTime;
			if (_attackTimer < _attackInterval)
				return;

			_attackTimer = 0f;
			DealDamage(_soldier.SoldierInteractionController.TargetUnit);
		}

		public void DealDamage(GameUnit targetUnit)
		{
			if (targetUnit == null || targetUnit.IsDead())
				return;

			targetUnit.TakeDamage(_damage);
		}
	}
}
