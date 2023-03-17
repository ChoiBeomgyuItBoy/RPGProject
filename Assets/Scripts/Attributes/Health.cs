using UnityEngine;
using GameDevTV.Saving;
using RPG.Stats;
using RPG.Core;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float regenerationPercentage = 100f;
        [SerializeField] private UnityEvent<float> onDamageTaken;
        [SerializeField] public UnityEvent onDie;
        private LazyValue<float> health;
        private readonly int DeadHash = Animator.StringToHash("die");
        private bool wasDeadLastFrame = false;
        public bool IsDead => health.value <= 0;

        public float GetCurrentHealth()
        {
            return health.value;
        }

        public float GetMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * (GetFraction());
        }

        public float GetFraction()
        {
            return health.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        
        public void TakeDamage(GameObject instigator, float damage)
        {
            if(IsDead) return;

            health.value = Mathf.Max(0f, health.value - damage);

            if(IsDead) 
            {
                onDie.Invoke();
                AwardExperience(instigator);
            }
            else
            {
                onDamageTaken.Invoke(damage);
            }

            UpdateState();
        }

        public void Heal(float amount)
        {
            health.value = Mathf.Min(health.value + amount, GetMaxHealth());
            UpdateState();
        }

        private void Awake()
        {
            health = new LazyValue<float>(GetInitialHealth);
        }

        private void Start()
        {
            health.ForceInit();
        }


        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        private void UpdateState()
        {
            Animator animator = GetComponent<Animator>();

            if(!wasDeadLastFrame && IsDead)
            {           
                GetComponent<ActionScheduler>().CancelCurrentAction();
                animator.SetTrigger(DeadHash);
            }

            if(wasDeadLastFrame && !IsDead)
            {
                animator.Rebind();
            }

            wasDeadLastFrame = IsDead;
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);

            health.value = Mathf.Max(health.value, regenHealthPoints);
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();

            if(experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        object ISaveable.CaptureState()
        {
            return health.value;
        }

        void ISaveable.RestoreState(object state)
        {
            health.value = (float) state;

            UpdateState();
        }
    }
}
