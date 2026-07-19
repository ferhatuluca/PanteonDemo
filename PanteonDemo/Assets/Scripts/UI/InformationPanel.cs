using System;
using Core.Enums;
using Core.Utilities.Pool_Spawner.Spawner.SpawnerWithPool;
using UnityEngine;

namespace UI
{
	public class InformationPanel : SpawnerMonoWithPoolWithType<SoldierUI, SoldierType>
	{
		[SerializeField] private RectTransform _contentPanelTransform;
		
		private void Start()
		{
			throw new NotImplementedException();
		}
	}
}