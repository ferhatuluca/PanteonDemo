using System;
using Core.Scriptables;
using Core.Types;
using UnityEngine;

namespace UI
{
	public class BuildingUI : MonoBehaviour
	{
		public static event Action<BuildingData, TeamType> OnBuildingUIClicked;
	}
}