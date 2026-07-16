using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "SoldierTeamData", menuName = "GameUnit/SoldierTeamData", order = 1)]

	public class SoldierTeamData : ScriptableObject
	{
		[SerializeField] private List<TeamData> _teamData;

		public TeamData GetTeamDataWithType(TeamType type) => _teamData.FirstOrDefault(t => t.TeamType == type);
	}
	
	[System.Serializable]
	public class TeamData
	{
		[field: SerializeField] public TeamType TeamType { private set; get; }
		[field: SerializeField] public Sprite Icon { private set; get; }
		[field: SerializeField] public AnimatorOverrideController Controller { private set; get; }
	}
}