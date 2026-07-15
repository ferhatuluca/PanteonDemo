using Core.Scriptables;
using UnityEngine;

namespace UI
{
	public class SoldierUI : MonoBehaviour
	{
		private int _damage;
		
		public int Health { private set; get; }
		public Vector2 GridSize { private set; get; }
		
		public void SetData(SoldierSO soldierSo)
		{
			GridSize = soldierSo.GridSize;
			Health = soldierSo.Health;
			_damage = soldierSo.Damage;
		}
	}
}