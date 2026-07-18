using System;

namespace Core.Utilities.Pool_Spawner.PoolLogics
{
    public abstract class PoolLogicBase<T>
    {
        public abstract void Push(T member);
        
        protected readonly Action<T> OnEnterPool;
        protected readonly Action<T> OnExitPool;
        
        protected int MaxNumberOfObjects;

        public void SetMaxNumberOfObjectsCount(int maxNumberOfObjects) => MaxNumberOfObjects = maxNumberOfObjects;

        protected PoolLogicBase(int maxNumberOfObjects, Action<T> onEnterPool, Action<T> onExitPool)
        {
            MaxNumberOfObjects = maxNumberOfObjects;
            OnEnterPool = onEnterPool;
            OnExitPool = onExitPool;
        }
    }
}