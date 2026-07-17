using System.Collections.Generic;
using UnityEngine;

namespace Core.Utilities.Interact
{
    public abstract class MultipleInteractable<T> : InteractableBase<T>
    {
        protected List<T> Interacts = new List<T>();
        
        protected sealed override void OnTriggeredEnterVirtual(T actor, Collider2D other)
        {
            if (Interacts == null)
                Interacts = new List<T>();
            
            if (Interacts.Contains(actor))
                return;
            
            Interacts.Add(actor);
            OnTriggerInteract(actor);
        }

        protected sealed override void OnTriggerExitVirtual(T actor, Collider2D other)
        {
            if(Interacts.Count == 0)
                return;
            
            if (!Interacts.Contains(actor))
                return;
            
            Interacts.Remove(actor);
            OnTriggerInteractExit(actor);
        }
        
        protected void ClearInteracts()
        {
            Interacts.Clear();
        }
    }
}