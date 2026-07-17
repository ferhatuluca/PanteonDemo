using Core.Enums;
using Core.Other;
using Core.Utilities.Pool_Spawner.Spawner.SpawnerWithPool;

namespace Core.Managers
{
	public class EffectSpawnerManager : SpawnerMonoWithPoolWithType<Effect, EffectType>
	{
		public Effect SpawnEffect(EffectType effectType)
		{
			SetSpawnType(effectType);
			
			//It gets object from pool, if there is no object poolmanager spawns it, if there is then pops it
			Effect effect = GetObjectFromPool();
			
			return effect;
		}
	}
}