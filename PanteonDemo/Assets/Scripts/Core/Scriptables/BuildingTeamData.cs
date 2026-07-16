using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "BuildingTeamData", menuName = "GameUnit/BuildingTeamData", order = 1)]
	public class BuildingTeamData : ScriptableObject
	{
		[SerializeField] private List<BuildingTeamTypeData> _teamData;

		public BuildingTypeData GetBuildingTypeData(TeamType teamType, BuildingType buildingType) =>
			_teamData.FirstOrDefault(t => t.TeamType == teamType)?
			         .BuildingTypeDataList.FirstOrDefault(b => b.BuildingType == buildingType);
	}
	
	[System.Serializable]
	public class BuildingTeamTypeData
	{
		[field: SerializeField] public TeamType TeamType { private set; get; }
		[field: SerializeField] public List<BuildingTypeData> BuildingTypeDataList { private set; get; }
	}

	[System.Serializable]
	public class BuildingTypeData
	{
		[field: SerializeField] public BuildingType BuildingType { private set; get; }
		[field: SerializeField] public Sprite Icon { private set; get; }
	}
}
