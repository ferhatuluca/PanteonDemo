using Core.Types;
using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "Building", menuName = "GameUnit/Buildings", order = 1)]
	public class BuildingSO : GameUnitSO
	{
		[field: SerializeField] public BuildingType BuildingType { private set; get; }
	}
}