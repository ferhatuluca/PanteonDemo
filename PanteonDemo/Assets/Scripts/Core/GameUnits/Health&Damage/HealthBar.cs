using UnityEngine;
using UnityEngine.UI;

namespace Core.GameUnits.Health_Damage
{
	public class HealthBar : MonoBehaviour
	{
		[SerializeField] private Slider _slider;

		public void SetMaxHealth(int health)
		{
			_slider.maxValue = health;
			_slider.value = health;
		}

		public void SetHealth(int health)
		{
			_slider.value = health;
		}

		public void SetActivate(bool isActive)
		{
			_slider.gameObject.SetActive(isActive);
		}
	}
}