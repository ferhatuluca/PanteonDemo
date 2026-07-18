using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.GameUnits.Health_Damage
{
	public class HealthController : MonoBehaviour
	{
		private GameUnit _gameUnit;
		private HealthBar _healthBar;

		private int _maxHp;
		private int _currentHealth;
		
		public bool IsDead { private set; get; }

		private void Awake()
		{
			_healthBar = GetComponentInChildren<HealthBar>();
		}

		public void Init(GameUnit gameUnit, int hp)
		{
			IsDead = false;
			_gameUnit = gameUnit;
			_maxHp = hp;
			_currentHealth = _maxHp;
			
			_healthBar.SetActivate(true);
			_healthBar.SetMaxHealth(_maxHp);
		}
		
		[Button]
		public void TakeDamage(int damage)
		{
			_currentHealth -= damage;
			if (_currentHealth <= 0)
			{
				IsDead = true;
				_healthBar.SetActivate(false);
				_gameUnit.Death();
				return;
			}

			_healthBar.SetHealth(_currentHealth);
		}
	}
}