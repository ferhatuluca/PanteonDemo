using System.Linq;
using Core.Enums;
using UnityEngine;

namespace Core.Scriptables
{
	[CreateAssetMenu(fileName = "GeneralData", menuName = "GeneralData", order = 0)]
	public class GeneralData : ScriptableObject
	{
		[field: SerializeField] public Texture2D Cursor { private set; get; }
		[field: SerializeField] public SoldierData[] SoldierData{ private set; get; }
		[field: SerializeField] public BuildingData[] BuildingData { private set; get; }
		
		public SoldierData GetSoldierData(SoldierType soldierType) => 
			SoldierData.FirstOrDefault(t => t.SoldierType == soldierType);
		public BuildingData GetBuildingData(BuildingType buildingType) => 
			BuildingData.FirstOrDefault(t => t.BuildingType == buildingType);
	}
}