using UnityEngine;

namespace Core.Scriptables
{
	public abstract class GameUnitData : ScriptableObject
	{
		[field: SerializeField] public string Name { private set; get; } = "Unit";
		[field: SerializeField] public int Health { private set; get; } = 10;
		[field: SerializeField] public Vector2 GridSize { private set; get; }
	}
}