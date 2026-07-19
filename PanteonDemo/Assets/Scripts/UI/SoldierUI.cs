using System;
using Core.Enums;
using Core.Scriptables;
using Core.Utilities.Pool_Spawner.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class SoldierUI : MonoBehaviour
	{
		public static event Action<SoldierData, TeamType> OnSoldierUIClicked;

		[SerializeField] private Image _icon;
		[SerializeField] private TextMeshProUGUI _name;

		private SoldierData _soldierData;
		private TeamType _teamType;
		
		private Button _button;

		private void Awake()
		{
			_button = GetComponent<Button>();
			_button.onClick.AddListener(OnButtonClicked);
		}

		public void Init(SoldierData soldierData, TeamType teamType)
		{
			_soldierData = soldierData;
			_teamType = teamType;
			
			_name.text = soldierData.Name;
			
			SoldierTeamData teamData = soldierData.GetSoldierTeamData(teamType);
			_icon.sprite = teamData.Icon;
		}
		
		private void OnButtonClicked()
		{
			OnSoldierUIClicked?.Invoke(_soldierData, _teamType);
		}
	}
}