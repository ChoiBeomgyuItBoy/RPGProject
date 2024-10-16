using UnityEngine;
using GameDevTV.Utils;
using System.Collections.Generic;

namespace RPG.Core
{
    public class ConditionEnabler : MonoBehaviour
    {
        [SerializeField] EnableCondition[] enableConditions;
        [SerializeField] bool checkOnUpdate = false;
        IEnumerable<IPredicateEvaluator> evaluators;

        [System.Serializable]
        struct EnableCondition
        {
            public GameObject target;
            public Condition condition;
        }

        void Awake()
        {
            evaluators = GameObject.FindWithTag("Player").GetComponents<IPredicateEvaluator>();
        }

        void Start()
        {
            Check();
        }

        void Update()
        {
            if(checkOnUpdate)
            {
                Check();
            }
        }

        void Check()
        {
            foreach(var enableCondition in enableConditions)
            {
                enableCondition.target.SetActive(enableCondition.condition.Check(evaluators));
            }
        }
    }
}