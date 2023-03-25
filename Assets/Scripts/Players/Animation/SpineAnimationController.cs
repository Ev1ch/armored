using UnityEngine;
using Spine.Unity;
using Spine;

namespace Players.Animation
{
    [RequireComponent(typeof(SkeletonAnimation))]
    public class SpineAnimationController : AnimationController
    {
        [SpineAnimation, SerializeField] private string _idleAnimationKey;
        [SpineAnimation, SerializeField] private string _runAnimationKey;
        [SpineAnimation, SerializeField] private string _jumpAnimationKey;
        [SpineAnimation, SerializeField] private string _attackAnimationKey;

        private SkeletonAnimation _skeletonAnimation;

        private void Start() => _skeletonAnimation = GetComponent<SkeletonAnimation>();

        protected override void PlayAnimation(AnimationType animationType)
        {
            var animationName = GetAnimationName(animationType);
            var currentAnimationName = _skeletonAnimation.AnimationState.GetCurrent(0).Animation.Name;

            if (currentAnimationName == animationName)
            {
                return;
            }

            TrackEntry trackEntry = _skeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
            trackEntry.Complete += (te) =>
            {
                OnActionRequested();
                OnAnimationEnded();
            };
        }

        private string GetAnimationName(AnimationType animationType)
        {
            return animationType switch
            {
                AnimationType.Idle => _idleAnimationKey,
                AnimationType.Run => _runAnimationKey,
                AnimationType.Jump => _jumpAnimationKey,
                AnimationType.Attack => _attackAnimationKey,
                _ => _idleAnimationKey
            };
        }
    }
}
