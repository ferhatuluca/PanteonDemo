using Core.Enums;
using Core.Utilities.Pool_Spawner.Spawner.SpawnerWithPool;

namespace Core.Other
{
	public class EffectSpawner : SpawnerMonoWithPoolWithType<Effect, EffectType>
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