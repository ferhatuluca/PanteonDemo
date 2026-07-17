using UnityEngine;

namespace Core.Utilities.Animation
{
    public static class AnimUtilities
    {
        public static float GetAnimLength(this Animator animator, string animName)
        {
            AnimationClip first = null;
            foreach (AnimationClip ac in animator.runtimeAnimatorController.animationClips)
            {
                if (ac.name == animName)
                {
                    first = ac;
                    break;
                }
            }

            if (first)
            {
                Debug.LogError($"Animation could not be found: {animName}");
                return 0;
            }
            return first.length;  
        }
    }
}