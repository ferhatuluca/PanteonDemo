using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class ProductionMenu : MonoBehaviour
	{
		[SerializeField] private ScrollRect _scrollRect;
		[SerializeField] private RectTransform _viewPort;
		[SerializeField] private RectTransform _contentPanelTransform;
		[SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;

		private BuildingUI[] _buildingUIs;
		private List<RectTransform> _items = new();
		private float _disableMarginY;
		private float _recordOffsetY;
		private float _threshold = 100f;
		private int _itemCount;
		private bool _hasDisabledLayout;
		private Vector2 _newAnchoredPosition;

		private void Start()
		{
			foreach (var buildingUI in _buildingUIs)
				_items.Add(buildingUI.GetComponent<RectTransform>());

			_itemCount = _items.Count;
			_scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
			_scrollRect.onValueChanged.AddListener(OnScroll);
		}

		private void DisableLayoutComponents()
		{
			_recordOffsetY = _items[0].anchoredPosition.y - _items[1].anchoredPosition.y;
			_disableMarginY = _recordOffsetY * _itemCount / 2;

			if (_verticalLayoutGroup != null)
				_verticalLayoutGroup.enabled = false;

			_hasDisabledLayout = true;
		}

		private void OnScroll(Vector2 position)
		{
			if (!_hasDisabledLayout)
				DisableLayoutComponents();

			for (int i = 0; i < _items.Count; i++)
			{
				if (_scrollRect.transform.InverseTransformPoint(_items[i].position).y < -_disableMarginY)
				{
					_newAnchoredPosition = _items[i].anchoredPosition;
					_newAnchoredPosition.y += _itemCount * _recordOffsetY;
					_items[i].anchoredPosition = _newAnchoredPosition;
					_scrollRect.content.GetChild(_itemCount - 1).SetAsFirstSibling();
				}
				else if (_scrollRect.transform.InverseTransformPoint(_items[i].position).y > _disableMarginY + _threshold)
				{
					_newAnchoredPosition = _items[i].anchoredPosition;
					_newAnchoredPosition.y -= _itemCount * _recordOffsetY;
					_items[i].anchoredPosition = _newAnchoredPosition;
					_scrollRect.content.GetChild(0).SetAsLastSibling();
				}
			}
		}
	}
}