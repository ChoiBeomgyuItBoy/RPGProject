using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Animations
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimations : MonoBehaviour
    {
        private Animator animator;

        private readonly int ForwardSpeedHash = Animator.StringToHash("forwardSpeed");
        private readonly int AttackHash = Animator.StringToHash("attack");
        private readonly int CancelAttackHash = Animator.StringToHash("cancelAttack");
        private readonly int DeadHash = Animator.StringToHash("die");

        private void OnEnable()
        {
            GetComponent<Mover>().OnLocomotion += UpdateLocomotion;

            GetComponent<Fighter>().OnAttack += PlayAttack;

            GetComponent<Fighter>().OnCancel += CancelAttack;

            GetComponent<Health>().OnDead += PlayDead;
        }

        private void OnDisable()
        {
            GetComponent<Mover>().OnLocomotion -= UpdateLocomotion;

            GetComponent<Fighter>().OnAttack -= PlayAttack;

            GetComponent<Fighter>().OnCancel += CancelAttack;

            GetComponent<Health>().OnDead -= PlayDead;
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void UpdateLocomotion(float speed)
        {
            animator.SetFloat(ForwardSpeedHash, speed);
        }

        private void PlayAttack()
        {
            animator.ResetTrigger(CancelAttackHash);
            animator.SetTrigger(AttackHash);
        }

        private void CancelAttack()
        {
            animator.ResetTrigger(AttackHash);
            animator.SetTrigger(CancelAttackHash);
        }   

        private void PlayDead()
        {
            animator.SetTrigger(DeadHash);
        }
    }
}