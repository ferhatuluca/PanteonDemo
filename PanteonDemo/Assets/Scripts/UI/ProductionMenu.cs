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

		private RectTransform[] _buildingUIRects;
		
		private void Start()
		{
			SpawnBuildingUIs();
		}

		private void SpawnBuildingUIs()
		{
			TeamType[] teamTypes = (TeamType[])Enum.GetValues(typeof(TeamType));
			BuildingData[] buildingData = GameManager.Instance.GeneralData.BuildingData;
			
			_buildingUIRects = new RectTransform[buildingData.Length * teamTypes.Length];

			int count = 0;
			foreach (TeamType teamType in teamTypes)
			{
				foreach (BuildingData data in buildingData)
				{
					BuildingUI ui = Instantiate(data.BuildingUIPrefab, _contentPanelTransform);
					ui.Init(data, teamType);
					_buildingUIRects[count++] = ui.GetComponent<RectTransform>();
				}
			}
		}
	}
}
