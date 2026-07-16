using System;
using Core.Enums;
using Core.Scriptables;
using UnityEngine;

namespace UI
{
	public class SoldierUI : MonoBehaviour
	{
		public static event Action<SoldierData, TeamType> OnSoldierUIClicked;
	}
}