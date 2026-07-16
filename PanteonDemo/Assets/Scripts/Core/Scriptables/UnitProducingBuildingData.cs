using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "UnitProducingBuilding", menuName = "GameUnit/Buildings/UnitProducingBuilding", order = 1)]
	public class UnitProducingBuildingData : BuildingData
	{
		[field: SerializeField] public SoldierData[] UnitsToProduce { private set; get; }
	}
}