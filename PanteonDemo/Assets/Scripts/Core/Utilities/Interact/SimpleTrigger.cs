using Core.Utilities.Extensions;
using UnityEngine;

namespace Core.Utilities.Interact
{
    public class SimpleTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask _hitObjectLayerMask;

        protected virtual void OnTriggeredEnterVirtual(Collider other){}
        protected virtual void OnTriggerExitVirtual(Collider other){}

        protected virtual bool PreConditionOnTrigger() {return true;}
        protected virtual bool PreConditionOnTrigger(Collider other) {return true;}
        protected virtual bool PreConditionOnTriggerExit() {return true;}

        private void OnTriggerEnter(Collider other)
        {
            if (!PreConditionOnTrigger())
                return;
            
            if (!PreConditionOnTrigger(other))
                return;

            
            if (!other.gameObject.layer.LayerMaskLayerCompare(_hitObjectLayerMask))
                return;

            OnTriggeredEnterVirtual(other);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!PreConditionOnTriggerExit())
                return;

            if (!other.gameObject.layer.LayerMaskLayerCompare(_hitObjectLayerMask))
                return;
            
            OnTriggerExitVirtual(other);
        }
    }
}