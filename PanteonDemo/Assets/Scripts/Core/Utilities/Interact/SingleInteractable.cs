using UnityEngine;

namespace Core.Utilities.Interact
{
    public abstract class SingleInteractable<T> : InteractableBase<T> where T: class
    {
        protected T Interact;

        protected sealed override void OnTriggeredEnterVirtual(T actor, Collider2D other)
        {
            if(Interact != null)
                return;
            
            Interact = actor;
            OnTriggerInteract(Interact);
        }

        protected sealed override void OnTriggerExitVirtual(T actor, Collider2D other)
        {
            if (Interact != actor) 
                return;
            
            OnTriggerInteractExit(Interact);
            Interact = null;
        }
    }
}