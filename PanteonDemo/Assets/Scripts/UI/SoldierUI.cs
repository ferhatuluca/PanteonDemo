using System;
using Core.Scriptables;
using UnityEngine;

namespace UI
{
	public class SoldierUI : MonoBehaviour
	{
		public static event Action<SoldierData> OnSoldierUIClicked;
	}
}