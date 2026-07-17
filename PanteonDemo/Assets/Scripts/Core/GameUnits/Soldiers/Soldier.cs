using System;
using Core.Enums;
using Core.Managers;
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
		[field: SerializeField] public SoldierType SoldierType { private set; get; }
		
		private MonoBehaviorPool<Soldier> _myPool;

		public GameUnit GameUnit { private set; get; }
		public SoldierAnimController SoldierAnimController { private set; get; }
		public SoldierInteractionController SoldierInteractionController { private set; get; }

		private void Awake()
		{
			GameUnit = GetComponent<GameUnit>();
			SoldierInteractionController = GetComponent<SoldierInteractionController>();
			SoldierAnimController = GetComponentInChildren<SoldierAnimController>();
		}

		private void Start()
		{
			// this should be in start because we need to get the pool after poolsmanager finishs its jobs
			_myPool = PoolsManager.Instance.GetMyPoolTyped<Soldier, SoldierType>(SoldierType);
		}

		public void Init(SoldierData soldierData, TeamType teamType, SoldierTypeData soldierTypeData)
		{
			GameUnit.Init(this, teamType, soldierData);
			SoldierInteractionController.Init(this);
			SoldierAnimController.Init(this, soldierTypeData);
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
			EffectSpawnerManager.Instance.SpawnEffect(EffectType.SoldierDeath);
			_myPool.Push(this);
		}

		//interface short methods
		public SoldierType GetTypeForPool() => SoldierType;
	}
}