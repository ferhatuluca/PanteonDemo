using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using UI;
using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "Building", menuName = "GameUnit/Buildings/NormalBuilding", order = 1)]
	public class BuildingData : GameUnitData
	{
		[field: SerializeField] public BuildingType BuildingType { private set; get; }
		[field: SerializeField] public BuildingUI BuildingUIPrefab { private set; get; }
		[SerializeField] private List<BuildingTeamData> _teamData;
		
		public BuildingTeamData GetBuildingTeamData(TeamType teamType) =>
			_teamData.FirstOrDefault(t => t.TeamType == teamType);
	}

	[System.Serializable]
	public class BuildingTeamData
	{
		[field: SerializeField] public TeamType TeamType { private set; get; }
		[field: SerializeField] public Sprite Icon { private set; get; }
	}
}