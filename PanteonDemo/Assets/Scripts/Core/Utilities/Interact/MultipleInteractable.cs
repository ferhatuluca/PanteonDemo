using System.Collections.Generic;
using UnityEngine;

namespace Core.Utilities.Interact
{
    public abstract class MultipleInteractable<T> : InteractableBase<T>
    {
        protected HashSet<T> Interacts;
        
        protected sealed override void OnTriggeredEnterVirtual(T actor, Collider2D other)
        {
            if (Interacts == null)
                Interacts = new HashSet<T>();
            
            if (!Interacts.Add(actor))
                return;
            
            OnTriggerInteract(actor);
        }

        protected sealed override void OnTriggerExitVirtual(T actor, Collider2D other)
        {
            if(Interacts.Count == 0)
                return;
            
            if (!Interacts.Remove(actor))
                return;
            
            OnTriggerInteractExit(actor);
        }
        
        protected void ClearInteracts()
        {
            Interacts.Clear();
        }
    }
}