using Core.Utilities.Pool_Spawner.Interfaces;

namespace Core.Utilities.Pool_Spawner.Pools
{
    public abstract class PoolBase<T> where T : IPoolMemberBase
    {
        public abstract void Push(T member);
        public abstract T Pull();
        protected abstract void OnEnterPool(T member);
        protected abstract void OnExitPull(T member);

        protected PoolBase()
        {
        }
    }
}