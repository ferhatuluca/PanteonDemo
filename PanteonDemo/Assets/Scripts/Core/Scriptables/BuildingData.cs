using Core.Enums;
using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "Building", menuName = "GameUnit/Buildings", order = 1)]
	public class BuildingData : GameUnitData
	{
		[field: SerializeField] public BuildingType BuildingType { private set; get; }
		[field: SerializeField] public Sprite Sprite { private set; get; }
	}
}