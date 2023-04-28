using System;
using System.Collections;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(menuName = "RPG/Abilities/Effects/Trigger Animation Effect")]
    public class TriggerAnimationEffect : EffectStrategy
    {
        [SerializeField] string animationTrigger;
        [SerializeField] string animationTag = "effect";
        [SerializeField] [Range(0,1)] float timeToEnableControl = 0.9f;

        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(WaitForAnimationFinished(data));
            finished();
        }

        private IEnumerator WaitForAnimationFinished(AbilityData data)
        {
            Animator animator = data.GetUser().GetComponent<Animator>();
            PlayerController playerController = data.GetUser().GetComponent<PlayerController>();

            animator.ResetTrigger("cancelAbility");
            animator.SetTrigger(animationTrigger);

            while(!FinishedPlaying(animator) && !data.IsCancelled())
            {
                playerController.enabled = false;
                yield return null;
            }

            animator.SetTrigger("cancelAbility");
            playerController.enabled = true;
        }

        private bool FinishedPlaying(Animator animator)
        {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            return !animator.IsInTransition(0) && 
                    stateInfo.IsTag(animationTag) && 
                        stateInfo.normalizedTime >= timeToEnableControl;
        }
    }
}
