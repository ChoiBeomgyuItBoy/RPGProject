using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Animations
{
    [RequireComponent(typeof(Animator), typeof(Mover))]
    public class CharacterAnimations : MonoBehaviour
    {
        private Animator animator;

        private readonly int ForwardSpeedHash = Animator.StringToHash("forwardSpeed");
        private readonly int AttackHash = Animator.StringToHash("attack");

        private void OnEnable()
        {
            GetComponent<Mover>().OnLocomotion += UpdateLocomotion;

            if(TryGetComponent<Fighter>(out Fighter fighter)) fighter.OnAttack += PlayAttack;
        }

        private void OnDisable()
        {
            GetComponent<Mover>().OnLocomotion -= UpdateLocomotion;

            if(TryGetComponent<Fighter>(out Fighter fighter)) fighter.OnAttack += PlayAttack;
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
            animator.SetTrigger(AttackHash);
        }
    }
}