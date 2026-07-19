using System;
using Core.Enums;
using Core.Managers;
using Core.Scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class ProductionMenu : MonoBehaviour
	{
		[SerializeField] private ScrollRect _scrollRect;
		[SerializeField] private RectTransform _viewPortTransform;
		[SerializeField] private RectTransform _contentPanelTransform;
		[SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;

		private RectTransform _firstItem;
		private float _totalRepeatedItemHeight;
		private float _totalItemAllHeight;

		private Vector2 _oldVelocity;
		private bool _isUpdated;
		
		private void Start()
		{
			SpawnBuildingUIs();
		}

		private void SpawnBuildingUIs()
		{
			TeamType[] teamTypes = (TeamType[])Enum.GetValues(typeof(TeamType));
			BuildingData[] buildingData = GameManager.Instance.GeneralData.BuildingData;
			
			foreach (TeamType teamType in teamTypes)
			{
				foreach (BuildingData data in buildingData)
				{
					BuildingUI ui = Instantiate(data.BuildingUIPrefab, _contentPanelTransform);
					ui.Init(data, teamType);
					if(_firstItem == null)
						_firstItem = ui.GetComponent<RectTransform>();
				}
			}
			
			int repeatedItemCount = buildingData.Length;
			int totalItemCount = buildingData.Length * teamTypes.Length;
			float oneItemHeight = _firstItem.rect.height + _verticalLayoutGroup.spacing;

			_totalRepeatedItemHeight = repeatedItemCount * oneItemHeight;
			_totalItemAllHeight = totalItemCount * oneItemHeight;
			
			_contentPanelTransform.localPosition = new Vector3(0f, _totalRepeatedItemHeight, 0f);
		}

		private void Update()
		{
			if (_isUpdated)
			{
				_isUpdated = false;
				_scrollRect.velocity = _oldVelocity;
			}
			
			if (_contentPanelTransform.localPosition.y > _totalItemAllHeight)
			{
				Canvas.ForceUpdateCanvases();
				_oldVelocity = _scrollRect.velocity;
				_contentPanelTransform.localPosition -= new Vector3(0f, _totalRepeatedItemHeight, 0f);
				_isUpdated = true;
			}
			if (_contentPanelTransform.localPosition.y < 0)
			{
				Canvas.ForceUpdateCanvases();
				_oldVelocity = _scrollRect.velocity;
				_contentPanelTransform.localPosition += new Vector3(0f, _totalRepeatedItemHeight, 0f);
				_isUpdated = true;
			}
		}
	}
}
