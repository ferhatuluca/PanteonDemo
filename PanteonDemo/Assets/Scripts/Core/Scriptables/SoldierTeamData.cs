using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "SoldierTeamData", menuName = "GameUnit/SoldierTeamData", order = 1)]
	public class SoldierTeamData : ScriptableObject
	{
		[SerializeField] private List<TeamTypeDataOld> _teamData;

		public SoldierTeamTypeDataOld GetSoldierTypeData(TeamType teamType, SoldierType soldierType) =>
			_teamData.FirstOrDefault(t => t.TeamType == teamType)?
			         .SoldierTypeDataList.FirstOrDefault(s => s.SoldierType == soldierType);
	}
	
	[System.Serializable]
	public class TeamTypeDataOld
	{
		[field: SerializeField] public TeamType TeamType { private set; get; }
		[field: SerializeField] public List<SoldierTeamTypeDataOld> SoldierTypeDataList { private set; get; }
	}

	[System.Serializable]
	public class SoldierTeamTypeDataOld
	{
		[field: SerializeField] public SoldierType SoldierType { private set; get; }
		[field: SerializeField] public Sprite Icon { private set; get; }
		[field: SerializeField] public AnimatorOverrideController Controller { private set; get; }
	}
}