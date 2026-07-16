using Core.Enums;
using Core.GameUnits.Soldiers;
using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "Soldier", menuName = "GameUnit/Soldier", order = 1)]
	public class SoldierData : GameUnitData
	{
		[field: SerializeField] public SoldierType SoldierType { private set; get; }
		[field: SerializeField] public Soldier SoldierPrefab { private set; get; }
		[field: SerializeField] public int Health { private set; get; } = 10;
		[field: SerializeField] public int Damage { private set; get; } = 5;
	}
}