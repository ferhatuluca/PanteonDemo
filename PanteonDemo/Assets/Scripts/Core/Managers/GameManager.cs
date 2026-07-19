using Core.Scriptables;
using Core.Utilities.Singleton;
using UnityEngine;

namespace Core.Managers
{
	// There is no game states right now, normally It manages game states
	public class GameManager : SingletonMonoBehaviour<GameManager>
	{
		[field: SerializeField] public GeneralData GeneralData { private set; get; }
		
		protected override void InternalAwake()
		{
			Cursor.SetCursor(GeneralData.Cursor, Vector2.zero, CursorMode.ForceSoftware);
		}
	}
}