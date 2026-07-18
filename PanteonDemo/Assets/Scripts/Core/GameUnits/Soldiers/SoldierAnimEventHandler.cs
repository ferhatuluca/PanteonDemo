using UnityEngine;

namespace Core.GameUnits.Soldiers
{
	public class SoldierAnimEventHandler : MonoBehaviour
	{
		private Soldier _soldier;

		private void Awake()
		{
			_soldier = GetComponentInParent<Soldier>();
		}

		public void OnAttackFinish()
		{
			_soldier.SoldierDamageController.DealDamage();
		}
	}
}