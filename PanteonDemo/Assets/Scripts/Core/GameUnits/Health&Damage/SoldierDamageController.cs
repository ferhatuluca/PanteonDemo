using Core.GameUnits.Soldiers;
using UnityEngine;

namespace Core.GameUnits.Health_Damage
{
	public class SoldierDamageController : MonoBehaviour
	{
		private Soldier _soldier;
		private int _damage;

		public void Init(Soldier soldier, int damage)
		{
			_soldier = soldier;
			_damage = damage;
		}

		public void DealDamage()
		{
			GameUnit targetUnit = _soldier.SoldierInteractionController.TargetUnit;
			if (targetUnit == null || targetUnit.IsDead())
				return;
			
			targetUnit.TakeDamage(_damage);
		}
	}
}
