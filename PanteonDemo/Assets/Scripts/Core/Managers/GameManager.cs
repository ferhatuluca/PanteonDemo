using Core.Scriptables;
using Core.Utilities.Singleton;
using UnityEngine;

namespace Core.Managers
{
	public class GameManager : SingletonMonoBehaviour<GameManager>
	{
		protected override void InternalAwake()
		{
			Cursor.SetCursor(GeneralData.Instance.Cursor, Vector2.zero, CursorMode.ForceSoftware);
		}
	}
}