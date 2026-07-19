using System.Collections.Generic;
using Core.Enums;
using UI;
using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "Building", menuName = "GameUnit/Buildings/NormalBuilding", order = 1)]
	public class BuildingData : GameUnitData
	{
		[field: SerializeField] public BuildingType BuildingType { private set; get; }
		[field: SerializeField] public BuildingUI BuildingUI { private set; get; }
		[field: SerializeField] public List<BuildingTeamTypeData> TeamTypeData { private set; get; }
	}

	[System.Serializable]
	public class BuildingTeamTypeData
	{
		[field: SerializeField] public TeamType TeamType { private set; get; }
		[field: SerializeField] public Sprite Icon { private set; get; }
	}
}