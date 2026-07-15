using Core.Scriptables;
using Core.Types;
using Core.Utilities.Pool_Spawner.Interfaces;
using UnityEngine;

namespace Core
{
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(SoldierPathFinder))]
	public class Soldier : MonoBehaviour, IPoolMemberWithType<SoldierType>
	{
		private SoldierPathFinder _soldierPathFinder;
		private int _damage;
		
		public SoldierType SoldierType { private set; get; }
		public int Health { private set; get; }
		public Vector2 GridSize { private set; get; }

		private void Awake()
		{
			_soldierPathFinder.Init(this);
		}

		public void SetData(SoldierSO soldierSo)
		{
			SoldierType = soldierSo.SoldierType;
			GridSize = soldierSo.GridSize;
			Health = soldierSo.Health;
			_damage = soldierSo.Damage;
		}

		public SoldierType GetTypeForPool()
		{
			return SoldierType;
		}

		public void OnEnterPool()
		{
			_soldierPathFinder.ResetForPool();
		}

		public void OnExitPool()
		{
		}
	}
}