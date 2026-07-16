using Core.Utilities.Singleton;
using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "GeneralData", menuName = "GeneralData", order = 0)]
	public class GeneralData : SingletonScriptableObject<GeneralData>
	{
		[field: SerializeField] public Texture2D Cursor { private set; get; }
		[field: SerializeField] public SoldierTeamData SoldierTeamData { private set; get; }
		[field: SerializeField] public BuildingTeamData BuildingTeamData { private set; get; }
	}
}