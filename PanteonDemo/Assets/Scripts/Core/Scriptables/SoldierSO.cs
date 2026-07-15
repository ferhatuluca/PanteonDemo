using Core.Types;
using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "Soldier", menuName = "GameUnit/Units/Soldier", order = 1)]
	public class SoldierSO : GameUnitSO
	{
		[field: SerializeField] public SoldierType SoldierType { private set; get; }
		[field: SerializeField] public Soldier SoldierPrefab { private set; get; }
		[field: SerializeField] public string Name { private set; get; } = "Soldier";
		[field: SerializeField] public int Health { private set; get; } = 10;
		[field: SerializeField] public int Damage { private set; get; } = 5;
	}
}