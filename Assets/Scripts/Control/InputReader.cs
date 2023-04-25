using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    [CreateAssetMenu(menuName = "RPG/New Input Reader")]
    public class InputReader : ScriptableObject
    {
        [SerializeField] InputAction[] inputActions;
        Dictionary<PlayerAction, KeyCode> keyLookup;
        public event Action onChange;
        bool isChoosing = false;

        [System.Serializable]
        class InputAction
        {
            public PlayerAction action = default;
            public KeyCode keyCode = default;
        }

        public IEnumerable<KeyValuePair<PlayerAction, KeyCode>> GetActionPairs()
        {
            if(keyLookup == null)
            {
                BuildLookup();
            }

            return keyLookup;
        }

        public KeyCode GetKeyCode(PlayerAction action)
        {
            if(isChoosing)
            {
                return KeyCode.None;
            }

            if(keyLookup == null)
            {
                BuildLookup();
            }

            if(!keyLookup.ContainsKey(action)) 
            {
                Debug.LogError($"Keybind for Input Action: {action} not found");
                return default;
            }

            return keyLookup[action];
        }

        public void SetKeyCode(PlayerAction action, KeyCode keyCode)
        {   
            if(keyLookup == null)
            {
                BuildLookup();
            }

            if(!keyLookup.ContainsKey(action)) 
            {
                Debug.LogError($"Keybind for Input Action: {action} not found");
                return;
            }
            
            keyLookup[action] = keyCode;
            SaveChanges();
            onChange?.Invoke();
        }

        public bool HasRepeatedKeyCode(KeyCode keyCode)
        {
            if(keyLookup == null)
            {
                BuildLookup();
            }

            HashSet<KeyCode> usedKeyCodes = new HashSet<KeyCode>();
            foreach(KeyCode candidate in keyLookup.Values)
            {
                if(candidate != KeyCode.None && !usedKeyCodes.Add(candidate))
                {
                    if(candidate == keyCode)
                    {
                        return true;
                    }
                }
            }
            return false;
        }       

        public void SetChoosing(bool isChoosing)
        {
            this.isChoosing = isChoosing;
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        private void BuildLookup()
        {
            keyLookup = new Dictionary<PlayerAction, KeyCode>();
            foreach(var inputAction in inputActions)
            {
                if(keyLookup.ContainsKey(inputAction.action))
                {
                    Debug.LogError($"Duplicated Input Action for {inputAction.action} & {inputAction.keyCode}");
                }

                keyLookup[inputAction.action] = inputAction.keyCode;
            }
        }

        private void SaveChanges()
        {
            for (int i = 0; i < inputActions.Length; i++)
            {
                InputAction selectedAction = inputActions[i];
                selectedAction.keyCode = keyLookup[selectedAction.action];
            }
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            onChange?.Invoke();
        }
#endif
    }
}