using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class ProductionMenu : MonoBehaviour
	{
		[SerializeField] private ScrollRect _scrollRect;
		[SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;

		private BuildingUI[] _buildingUIs;
		private List<RectTransform> _items = new();
		private float _itemOffset;
		private float _scrollMargin;
		private int _itemCount;

		private void Awake()
		{
			_buildingUIs = GetComponentsInChildren<BuildingUI>();
			foreach (BuildingUI buildingUI in _buildingUIs)
				_items.Add(buildingUI.GetComponent<RectTransform>());

			_itemCount = _items.Count;
			_itemOffset = _items[0].anchoredPosition.y - _items[1].anchoredPosition.y;
			_scrollMargin = _itemOffset * _itemCount / 2;

			_verticalLayoutGroup.enabled = false;
			_scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
			_scrollRect.onValueChanged.AddListener(OnScroll);
		}

		private void OnScroll(Vector2 position)
		{
			for (int i = 0; i < _itemCount; i++)
			{
				float itemY = _scrollRect.transform.InverseTransformPoint(_items[i].position).y;

				if (itemY < -_scrollMargin)
				{
					_items[i].anchoredPosition += Vector2.up * _itemCount * _itemOffset;
					_scrollRect.content.GetChild(_itemCount - 1).SetAsFirstSibling();
				}
				else if (itemY > _scrollMargin + 100f)
				{
					_items[i].anchoredPosition -= Vector2.up * _itemCount * _itemOffset;
					_scrollRect.content.GetChild(0).SetAsLastSibling();
				}
			}
		}
	}
}
