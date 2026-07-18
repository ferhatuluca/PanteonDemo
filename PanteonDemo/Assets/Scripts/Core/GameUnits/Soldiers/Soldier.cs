using Core.Enums;
using Core.GameUnits.Health_Damage;
using Core.Managers;
using Core.Other;
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
	[RequireComponent(typeof(SoldierDamageController))]
	public class Soldier : MonoBehaviour, IGameUnitObject, IPoolMemberWithType<SoldierType>
	{
		[field: SerializeField] public SoldierType SoldierType { private set; get; }
		
		private MonoBehaviorPool<Soldier> _myPool;

		public GameUnit GameUnit { private set; get; }
		public SoldierAnimController SoldierAnimController { private set; get; }
		public SoldierInteractionController SoldierInteractionController { private set; get; }
		public SoldierDamageController SoldierDamageController { private set; get; }

		private void Awake()
		{
			GameUnit = GetComponent<GameUnit>();
			SoldierInteractionController = GetComponent<SoldierInteractionController>();
			SoldierAnimController = GetComponentInChildren<SoldierAnimController>();
			SoldierDamageController = GetComponent<SoldierDamageController>();
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
			SoldierDamageController.Init(this, soldierData.Damage);
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
			Effect effect = EffectSpawnerManager.Instance.SpawnEffect(EffectType.SoldierDeath);
			effect.transform.position = transform.position;
			_myPool.Push(this);
		}

		//interface short methods
		public SoldierType GetTypeForPool() => SoldierType;
		// this is used for checking OnTriggerEnter2D, if false then Unit will not register to Interacts in controller
		public bool IsAvailableForInteract() => true; // there is no availability check for soldier yet
	}
}
