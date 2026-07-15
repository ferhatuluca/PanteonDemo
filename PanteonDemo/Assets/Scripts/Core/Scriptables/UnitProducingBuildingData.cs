using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "UnitProducing", menuName = "GameUnit/Buildings/UnitProducing/UnitProducing", order = 1)]
	public class UnitProducingBuildingData : BuildingData
	{
		[field: SerializeField] public SoldierData[] UnitsToProduce { private set; get; }
	}
}