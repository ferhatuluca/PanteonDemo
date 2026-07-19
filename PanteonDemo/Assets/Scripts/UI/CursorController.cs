using UnityEngine;

namespace UI
{
	public class CursorController : MonoBehaviour
	{
		[SerializeField] private RectTransform _cursorParentRect;

		private RectTransform _canvasRectTransform;

		private void Awake()
		{
			Cursor.visible = false;
			_canvasRectTransform = GetComponent<Canvas>().GetComponent<RectTransform>();
		}

		private void Update()
		{
			Vector2 screenPoint = Input.mousePosition;

			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
				    _canvasRectTransform, screenPoint, null, out Vector2 localPoint)) 
				return;
			
			_cursorParentRect.anchoredPosition = localPoint;
		}
	}
}