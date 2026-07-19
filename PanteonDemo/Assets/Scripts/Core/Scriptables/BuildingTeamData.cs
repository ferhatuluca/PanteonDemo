using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "BuildingTeamData", menuName = "GameUnit/BuildingTeamData", order = 1)]
	public class BuildingTeamData : ScriptableObject
	{
		[SerializeField] private List<BuildingTeamTypeDataOld> _teamData;

		public BuildingTypeDataOld GetBuildingTypeData(TeamType teamType, BuildingType buildingType) =>
			_teamData.FirstOrDefault(t => t.TeamType == teamType)?
			         .BuildingTypeDataList.FirstOrDefault(b => b.BuildingType == buildingType);
	}
	
	[System.Serializable]
	public class BuildingTeamTypeDataOld
	{
		[field: SerializeField] public TeamType TeamType { private set; get; }
		[field: SerializeField] public List<BuildingTypeDataOld> BuildingTypeDataList { private set; get; }
	}

	[System.Serializable]
	public class BuildingTypeDataOld
	{
		[field: SerializeField] public BuildingType BuildingType { private set; get; }
		[field: SerializeField] public Sprite Icon { private set; get; }
	}
}
