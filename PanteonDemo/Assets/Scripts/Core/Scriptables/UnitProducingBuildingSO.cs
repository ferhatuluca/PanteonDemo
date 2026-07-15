using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "UnitProducing", menuName = "GameUnit/Buildings/UnitProducing/UnitProducing", order = 1)]
	public class UnitProducingBuildingSO : BuildingSO
	{
		[field: SerializeField] public SoldierSO[] UnitsToProduce { private set; get; }
	}
}