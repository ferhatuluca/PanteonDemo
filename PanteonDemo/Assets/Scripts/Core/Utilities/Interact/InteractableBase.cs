using Core.Utilities.Extensions;
using UnityEngine;

namespace Core.Utilities.Interact
{
    public enum ComponentCheck
    {
        ParentObjects,
        ChildObjects
    }
    
    public abstract class InteractableBase<T> : MonoBehaviour
    {
        [SerializeField] private LayerMask _hitObjectLayerMask;

        protected virtual ComponentCheck[] ComponentChecks() => new[] 
            { ComponentCheck.ParentObjects };
        
        protected abstract void OnTriggerInteract(T actor);
        protected abstract void OnTriggerInteractExit(T actor);
        
        protected abstract void OnTriggeredEnterVirtual(T actor, Collider2D other);
        protected abstract void OnTriggerExitVirtual(T actor, Collider2D other);
        
        protected virtual bool PreConditionEnter(Collider2D other) {return true;}
        protected virtual bool ConditionAfterInteractionEnter(T actor) {return true;}
        
        protected virtual bool PreConditionExit(Collider2D other) {return true;}
        protected virtual bool AfterConditionInteractionExit(T actor) {return true;}

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!PreConditionEnter(other))
                return;

            T objectToCheck = default(T);
            foreach (var componentCheck in ComponentChecks())
            {
                if (CheckComponent(ref objectToCheck, other, componentCheck))
                    break;
            }

            if (objectToCheck == null)
                return;

            if (!other.gameObject.layer.LayerMaskLayerCompare(_hitObjectLayerMask))
                return;
            
            if (!ConditionAfterInteractionEnter(objectToCheck))
                return;
            
            OnTriggeredEnterVirtual(objectToCheck, other);
        }
        
        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (!PreConditionExit(other))
                return;
            
            T objectToCheck = default(T);
            foreach (var componentCheck in ComponentChecks())
            {
                if (CheckComponent(ref objectToCheck, other, componentCheck))
                    break;
            }
            
            if (objectToCheck == null)
                return;

            if (!other.gameObject.layer.LayerMaskLayerCompare(_hitObjectLayerMask))
                return;
            
            if (!AfterConditionInteractionExit(objectToCheck))
                return;
            
            OnTriggerExitVirtual(objectToCheck, other);
        }

        private bool CheckComponent(ref T objectToCheck, Collider2D other, ComponentCheck componentCheck)
        {
            if (componentCheck == ComponentCheck.ParentObjects)
                objectToCheck = other.GetComponentInParent<T>(true);
            if (componentCheck == ComponentCheck.ChildObjects)
                objectToCheck = other.GetComponentInChildren<T>(true);

            return objectToCheck != null;
        }
    }
}