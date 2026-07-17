using Core.Enums;
using Core.Other;
using Core.Utilities.Singleton;
using UnityEngine;

namespace Core.Managers
{
	[RequireComponent(typeof(EffectSpawner))]
	public class EffectSpawnerManager : SingletonMonoBehaviour<EffectSpawnerManager>
	{
		private EffectSpawner _effectSpawner;

		protected override void InternalAwake()
		{
			_effectSpawner = GetComponent<EffectSpawner>();
		}

		public Effect SpawnEffect(EffectType effectType)
		{
			return _effectSpawner.SpawnEffect(effectType);
		}
	}
}