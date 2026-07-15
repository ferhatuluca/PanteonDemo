using System;
using System.Collections.Generic;
using Core.Utilities.Pool_Spawner.Interfaces;
using Sirenix.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Utilities.Pool_Spawner.PoolLogics
{
    public sealed class PoolLogicSimple<T> : PoolLogicBase<T> where T : MonoBehaviour, IPoolMemberBase
    {
        private readonly Stack<T> _objectStack = new Stack<T>();
        
        private readonly Func<T> _onCreate;

        public int GetObjectsCount() => _objectStack.Count;
        public Stack<T> GetAllObjects() => _objectStack;

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
                _objectStack.Push(newMember);
            }
        }

        public T Pull()
        {
            if (_objectStack.Count > 0)
            {
                var objectT = _objectStack.Pop();
                OnExitPool(objectT);
                return objectT;
            }

            var newMember = _onCreate();
            OnExitPool(newMember);
            return newMember;
        }
        
        public override void Push(T member)
        {
            if (_objectStack.Count < MaxNumberOfObjects)
            {
                _objectStack.Push(member);
                OnEnterPool(member);
            }
            else
            {
                OnEnterPool(member);
                Object.DestroyImmediate(member.gameObject);
            }
        }
        
        public override void ClearPool()
        {
            _objectStack.ForEach(o => Object.DestroyImmediate(o.gameObject));
            _objectStack.Clear();
        }
    }
}