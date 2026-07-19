using System;
using Core.Enums;
using Core.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class BuildingUI : MonoBehaviour
	{
		public static event Action<BuildingData, TeamType> OnBuildingUIClicked;

		[SerializeField] private Image _image;
		[SerializeField] private TextMeshProUGUI _name;
		[SerializeField] private TextMeshProUGUI _gridCellText;

		private BuildingData _buildingData;
		private TeamType _teamType;

		private Button _button;

		private void Awake()
		{
			_button = GetComponent<Button>();
			_button.onClick.AddListener(OnButtonClicked);
		}

		public void Init(BuildingData buildingData, TeamType teamType)
		{
			_buildingData = buildingData;
			_teamType = teamType;
			
			_name.text = buildingData.Name;
			_gridCellText.text = $"{buildingData.GridCellSize.x}x{buildingData.GridCellSize.y}";
			
			BuildingTeamData teamData = buildingData.GetBuildingTeamData(teamType);
			_image.sprite = teamData.Icon;
		}
		
		private void OnButtonClicked()
		{
			OnBuildingUIClicked?.Invoke(_buildingData, _teamType);
		}
	}
}