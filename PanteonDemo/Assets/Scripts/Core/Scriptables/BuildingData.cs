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
	}
}