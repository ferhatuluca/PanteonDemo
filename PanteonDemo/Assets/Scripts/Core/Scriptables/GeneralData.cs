using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "GeneralData", menuName = "GeneralData", order = 0)]
	public class GeneralData : ScriptableObject
	{
		[field: SerializeField] public Texture2D Cursor { private set; get; }
		[field: SerializeField] public SoldierData[] SoldierData{ private set; get; }
		[field: SerializeField] public BuildingData[] BuildingData { private set; get; }
	}
}