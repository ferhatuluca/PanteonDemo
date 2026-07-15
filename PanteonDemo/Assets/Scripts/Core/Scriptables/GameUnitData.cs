using UnityEngine;

namespace Core.Scriptables
{
	public abstract class GameUnitData : ScriptableObject
	{
		[field: SerializeField] public string Name { private set; get; } = "Unit";
		[field: SerializeField] public Sprite Sprite { private set; get; }
		[field: SerializeField] public Vector2 GridSize { private set; get; }
	}
}