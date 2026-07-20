using System;
using Core.Enums;
using Core.GameUnits.Health_Damage;
using Core.Scriptables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.GameUnits
{
	// GameUnit is not parent for building and soldier because when code grows it become harder to manage
	// inheritance, such these cases composition is better
	public class GameUnit : MonoBehaviour
	{
		public event Action OnDead;
		
		public IGameUnitObject GameUnitObject { private set; get; }
		public TeamType TeamType { private set; get; }
		public Vector2 GridSize { private set; get; }
		public HealthController HealthController { private set; get; }

		public void Init(IGameUnitObject gameUnitObject, TeamType teamType, GameUnitData gameUnitData)
		{
			GameUnitObject = gameUnitObject;
			TeamType = teamType;
			GridSize = gameUnitData.GridCellSize;
			
			HealthController = GetComponent<HealthController>();
			HealthController.Init(this, gameUnitData.Health);
		}
		
		public void Death()
		{
			GameUnitObject.Death();
			OnDead?.Invoke();
			OnDead = null;
		}

		public void TakeDamage(int damage)
		{
			HealthController.TakeDamage(damage);	
		}
		
		public bool IsAvailableForInteract()
		{
			return !HealthController.IsDead && GameUnitObject.IsAvailableForInteract();
		}
		
		public bool IsDead()
		{
			return HealthController.IsDead;
		}
	}
}