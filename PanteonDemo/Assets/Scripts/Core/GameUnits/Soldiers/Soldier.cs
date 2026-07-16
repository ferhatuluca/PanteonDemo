using Core.Enums;
using Core.Scriptables;
using Core.Utilities.Pool_Spawner.Interfaces;
using UnityEngine;

namespace Core.GameUnits.Soldiers
{
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(SoldierInteractionController))]
	[RequireComponent(typeof(SoldierAnimController))]
	public class Soldier : MonoBehaviour, IClickableGameUnit, IPoolMemberWithType<SoldierType>
	{
		public SoldierAnimController SoldierAnimController { private set; get; }
		public SoldierInteractionController SoldierInteractionController { private set; get; }
		
		public SoldierType SoldierType { private set; get; }
		public TeamType TeamType { private set; get; }
		public Vector2 GridSize { private set; get; }

		public bool IsAlive() => true; // will be implemented
		public SoldierType GetTypeForPool() => SoldierType;
		public TeamType GetTeamType() => TeamType;
		public Transform GetTransform() => transform;

		public void Init(SoldierData soldierData, TeamType teamType, SoldierTypeData soldierTypeData)
		{
			SoldierType = soldierData.SoldierType;
			TeamType = teamType;
			GridSize = soldierData.GridSize;

			SoldierInteractionController = GetComponent<SoldierInteractionController>();
			SoldierInteractionController.Init(this);

			SoldierAnimController = GetComponentInChildren<SoldierAnimController>();
			SoldierAnimController.Init(this, soldierTypeData);
		}
		
		public void OnSelect()
		{
			throw new System.NotImplementedException();
		}

		public void OnEnterPool()
		{
			SoldierInteractionController.ResetForPool();
		}

		public void OnExitPool()
		{
		}
	}
}