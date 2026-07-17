using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "GeneralData", menuName = "GeneralData", order = 0)]
	public class GeneralData : ScriptableObject
	{
		[field: SerializeField] public Texture2D Cursor { private set; get; }
	}
}