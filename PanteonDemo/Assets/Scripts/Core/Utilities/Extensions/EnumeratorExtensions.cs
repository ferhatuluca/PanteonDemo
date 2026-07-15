using System;
using System.Collections;
using UnityEngine;

namespace Core.Utilities.Extensions
{
	public static class EnumeratorExtensions
	{
		public static Coroutine StartDelayedActionCoroutine(this MonoBehaviour monoBehaviour, float delayTime, 
			Action onDelayFinished)
		{
			return monoBehaviour.StartCoroutine(DelayedActionCoroutine(delayTime, onDelayFinished));
		}
		
		private static IEnumerator DelayedActionCoroutine(float delayTime, Action onDelayFinished)
		{
			yield return new WaitForSeconds(delayTime);
			onDelayFinished?.Invoke();
		}
	}
}