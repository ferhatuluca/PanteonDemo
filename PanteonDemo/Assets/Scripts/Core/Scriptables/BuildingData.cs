using Core.Enums;
using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "Building", menuName = "GameUnit/Buildings/NormalBuilding", order = 1)]
	public class BuildingData : GameUnitData
	{
		[field: SerializeField] public BuildingType BuildingType { private set; get; }
	}
}