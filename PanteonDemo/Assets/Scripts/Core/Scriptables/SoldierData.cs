using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "Soldier", menuName = "GameUnit/Soldier", order = 1)]
	public class SoldierData : GameUnitData
	{
		[field: SerializeField] public SoldierType SoldierType { private set; get; }
		[field: SerializeField] public int Damage { private set; get; } = 5;
		[SerializeField] private List<SoldierTeamTypeData> _teamData;
		
		public SoldierTeamTypeData GetSoldierTeamData(TeamType teamType) => 
			_teamData.FirstOrDefault(t => t.TeamType == teamType);
	}

	[System.Serializable]
	public class SoldierTeamTypeData
	{
		[field: SerializeField] public TeamType TeamType { private set; get; }
		[field: SerializeField] public Sprite Icon { private set; get; }
		[field: SerializeField] public AnimatorOverrideController Controller { private set; get; }
	}
}