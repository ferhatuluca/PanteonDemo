using System.Collections;
using Core.Enums;
using Core.Scriptables;
using Core.Utilities.Pool_Spawner;
using Core.Utilities.Pool_Spawner.Interfaces;
using Core.Utilities.Pool_Spawner.Pools;
using UnityEngine;

namespace Core.GameUnits.Soldiers
{
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(SoldierInteractionController))]
	[RequireComponent(typeof(SoldierAnimController))]
	public class Soldier : MonoBehaviour, IGameUnitObject, IPoolMemberWithType<SoldierType>
	{
		public SoldierType SoldierType { private set; get; }
		public GameUnit GameUnit { private set; get; }
		public SoldierAnimController SoldierAnimController { private set; get; }
		public SoldierInteractionController SoldierInteractionController { private set; get; }

		private void Awake()
		{
			GameUnit = GetComponent<GameUnit>();
			SoldierInteractionController = GetComponent<SoldierInteractionController>();
			SoldierAnimController = GetComponentInChildren<SoldierAnimController>();
		}

		public void Init(SoldierData soldierData, TeamType teamType, SoldierTypeData soldierTypeData)
		{
			SoldierType = soldierData.SoldierType;
			
			GameUnit.Init(this, teamType, soldierData);
			SoldierInteractionController.Init(this);
			SoldierAnimController.Init(this, soldierTypeData);
		}
		
		public void OnSelect()
		{
			throw new System.NotImplementedException();
		}

		public void OnEnterPool()
		{
			SoldierInteractionController.ResetForPool();
			SoldierAnimController.ResetForPool();
		}

		public void OnExitPool()
		{
			
		}

		public void Death()
		{
			StartCoroutine(DeathEnumerator());
		}
		
		private IEnumerator DeathEnumerator()
		{
			// death effect
			MonoBehaviorPool<Soldier> pool = PoolsManager.Instance.GetMyPoolTyped<Soldier, SoldierType>(SoldierType);
			pool.Push(this);
			yield return null;
		}

		//interface short methods
		public SoldierType GetTypeForPool() => SoldierType;
	}
}