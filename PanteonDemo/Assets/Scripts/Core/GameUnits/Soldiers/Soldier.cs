using Core.Scriptables;
using Core.Types;
using Core.Utilities.Pool_Spawner.Interfaces;
using UnityEngine;

namespace Core.GameUnits.Soldiers
{
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(SoldierPathFinder))]
	public class Soldier : MonoBehaviour, IClickableGameUnit, IPoolMemberWithType<SoldierType>
	{
		private SoldierPathFinder _soldierPathFinder;
		
		public SoldierType SoldierType { private set; get; }
		public TeamType TeamType { private set; get; }
		public Vector2 GridSize { private set; get; }

		private void Awake()
		{
			_soldierPathFinder.Init(this);
		}

		public void SetData(SoldierData soldierData, TeamType teamType)
		{
			SoldierType = soldierData.SoldierType;
			TeamType = teamType;
			GridSize = soldierData.GridSize;
		}
		
		public void OnSelect()
		{
			throw new System.NotImplementedException();
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