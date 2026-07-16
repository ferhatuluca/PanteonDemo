using Core.Enums;
using UnityEngine;

namespace Core.GameUnits
{
	public interface IClickableGameUnit
	{
		void OnSelect();
		bool IsAlive();
		TeamType GetTeamType();
		Transform GetTransform();
	}
}