using UnityEngine;

namespace RPG.Control
{
    [CreateAssetMenu(menuName = "RPG/New Input Reader")]
    public class InputReader : ScriptableObject
    {
        [Header("Actions")]
        [SerializeField] KeyCode movementKey = KeyCode.Mouse0;
        [SerializeField] KeyCode interactionKey = KeyCode.Mouse0;
        [SerializeField] KeyCode cancelKey = KeyCode.Mouse1;       
        [SerializeField] KeyCode firstAbilityKey = KeyCode.Alpha1;
        [SerializeField] KeyCode secondAbilityKey = KeyCode.Alpha2;
        [SerializeField] KeyCode thirdAbilityKey = KeyCode.Alpha3;
        [SerializeField] KeyCode fourthAbilityKey = KeyCode.Alpha4;
        [SerializeField] KeyCode fifthAbilityKey = KeyCode.Alpha5;
        [SerializeField] KeyCode sixthAbilityKey = KeyCode.Alpha6;

        [Header("UI")]
        [SerializeField] KeyCode inventoryKey = KeyCode.I; 
        [SerializeField] KeyCode questsKey = KeyCode.Q;
        [SerializeField] KeyCode traitsKey = KeyCode.T;
        [SerializeField] KeyCode pauseKey = KeyCode.Escape;

        public KeyCode GetMovementKey() => movementKey;
        public KeyCode GetInteractionKey() => interactionKey;
        public KeyCode GetCancelKey() => cancelKey;
        public KeyCode GetInventoryKey() => inventoryKey;
        public KeyCode GetQuestsKey() => questsKey;
        public KeyCode GetTraitsKey() => traitsKey;
        public KeyCode GetPauseKey() => pauseKey;
        public KeyCode GetFirstAbilityKey() => firstAbilityKey;
        public KeyCode GetSecondAbilityKey() => secondAbilityKey;
        public KeyCode GetThirdAbilityKey() => thirdAbilityKey;
        public KeyCode GetFourthAbilityKey() => fourthAbilityKey;
        public KeyCode GetFifthAbilityKey() => fifthAbilityKey;
        public KeyCode GetSixthAbilityKey() => sixthAbilityKey;
    }
}