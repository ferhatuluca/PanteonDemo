using Core.Enums;
using Core.Utilities.Animation;
using Core.Utilities.Extensions;
using Core.Utilities.Pool_Spawner;
using Core.Utilities.Pool_Spawner.Interfaces;
using Core.Utilities.Pool_Spawner.Pools;
using UnityEngine;

namespace Core.Other
{
	public class Effect : MonoBehaviour, IPoolMemberWithType<EffectType>
	{
		[SerializeField] private EffectType _effectType;

		private MonoBehaviorPool<Effect> _myPool;
		
		private Animator _animator;
		private float _getAnimationTime = -1f;
		private static readonly int Run = Animator.StringToHash("Run");

		private void Awake()
		{
			_animator = GetComponentInChildren<Animator>();
			_getAnimationTime = _animator.GetAnimLength(_effectType.ToString());
		}

		private void Start()
		{
			_myPool = PoolsManager.Instance.GetMyPoolTyped<Effect, EffectType>(_effectType);
		}

		public void OnEnterPool()
		{
		}

		public void OnExitPool()
		{
			_animator.SetTrigger(Run);
			this.StartDelayedActionCoroutine(_getAnimationTime, () =>
			{
				_myPool.Push(this);
			});
		}

		public EffectType GetTypeForPool() => _effectType;
	}
}