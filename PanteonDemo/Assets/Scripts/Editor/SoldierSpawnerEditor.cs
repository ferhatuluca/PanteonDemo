using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using Core.GameUnits.Buildings;
using Core.Scriptables;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	[CustomEditor(typeof(SoldierSpawner))]
	public class SoldierSpawnerEditor : UnityEditor.Editor
	{
		private Dictionary<SoldierType, SoldierData> _soldierDataByType;

		private void OnEnable()
		{
			_soldierDataByType = new Dictionary<SoldierType, SoldierData>();
			var allSoldierData = Resources.FindObjectsOfTypeAll<SoldierData>();
			foreach (var data in allSoldierData)
			{
				if (!_soldierDataByType.ContainsKey(data.SoldierType))
					_soldierDataByType[data.SoldierType] = data;
			}
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("SpawnButtons", EditorStyles.boldLabel);

			if (!Application.isPlaying)
			{
				EditorGUILayout.HelpBox("Spawn buttons work only in Play Mode.", MessageType.Info);
				return;
			}

			var soldierSpawner = (SoldierSpawner)target;
			var building = soldierSpawner.GetComponent<Building>();
			if (building == null || building.GameUnit == null)
			{
				EditorGUILayout.HelpBox("Building not initialized.", MessageType.Warning);
				return;
			}

			TeamType teamType = building.GameUnit.TeamType;

			EditorGUILayout.LabelField(teamType.ToString(), EditorStyles.boldLabel);

			using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
			{
				foreach (SoldierType soldierType in System.Enum.GetValues(typeof(SoldierType)))
				{
					if (GUILayout.Button($"{teamType}_{soldierType}"))
					{
						if (_soldierDataByType.TryGetValue(soldierType, out var soldierData))
						{
							soldierSpawner.SpawnSoldier(soldierData);
						}
						else
						{
							Debug.LogWarning($"No SoldierData found for {soldierType}");
						}
					}
				}
			}
		}
	}
}
