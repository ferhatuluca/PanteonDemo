using System;
using System.Collections.Generic;
using Core.Utilities.Pool_Spawner.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;
 
namespace Core.Utilities.Pool_Spawner.PoolLogics
{
    public sealed class PoolLogicSimple<T> : PoolLogicBase<T> where T : MonoBehaviour, IPoolMemberBase
    {
        private readonly Stack<T> _deActiveObjectStack = new Stack<T>();
        private readonly HashSet<T> _activeObjects = new HashSet<T>();
 
        private readonly Func<T> _onCreate;
        
        public IReadOnlyCollection<T> GetAllObjects() => _deActiveObjectStack;
        public IReadOnlyCollection<T> GetActiveObjects() => _activeObjects;
 
        public PoolLogicSimple(int maxNumberOfObjects, Func<T> onCreate, Action<T> onEnterPool, Action<T> onExitPool)
            : base(maxNumberOfObjects, onEnterPool, onExitPool)
        {
            _onCreate = onCreate;
        }
 
        public void CreateMultiple(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var newMember = _onCreate();
                _deActiveObjectStack.Push(newMember);
            }
        }
 
        public T Pull()
        {
            var objectT = _deActiveObjectStack.Count > 0 ? _deActiveObjectStack.Pop() : _onCreate();
 
            if (!_activeObjects.Add(objectT))
            {
                Debug.LogError($"{objectT.name} is already active in this pool.");
            }
 
            OnExitPool(objectT);
            return objectT;
        }
 
        public override void Push(T member)
        {
            if (!_activeObjects.Remove(member))
            {
                Debug.LogError($"{member.name} is not active in this pool, ignoring duplicate push.");
                return;
            }
 
            if (_deActiveObjectStack.Count + _activeObjects.Count < MaxNumberOfObjects)
            {
                _deActiveObjectStack.Push(member);
                OnEnterPool(member);
            }
            else
            {
                OnEnterPool(member);
                Object.Destroy(member.gameObject);
            }
        }
    }
}