using Core.Enums;
using Core.Utilities.Animation;
using Core.Utilities.Pool_Spawner.Interfaces;
using UnityEngine;

namespace Core.Other
{
	public class Effect : MonoBehaviour, IPoolMemberWithType<EffectType>
	{
		[SerializeField] private EffectType _effectType;
		
		private Animator _animator;
		private float _getAnimationTime = -1f;

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_getAnimationTime = _animator.GetAnimLength(_effectType.ToString());
		}

		public void OnEnterPool()
		{
			throw new System.NotImplementedException();
		}

		public void OnExitPool()
		{
			throw new System.NotImplementedException();
		}

		public EffectType GetTypeForPool() => _effectType;
	}
}