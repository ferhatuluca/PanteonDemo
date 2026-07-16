using System;
using Core.Enums;
using Core.Scriptables;
using UnityEngine;

namespace UI
{
	public class BuildingUI : MonoBehaviour
	{
		public static event Action<BuildingData, TeamType> OnBuildingUIClicked;
	}
}