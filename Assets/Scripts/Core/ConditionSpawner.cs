using UnityEngine;
using GameDevTV.Utils;
using System.Collections.Generic;

namespace RPG.Core
{
    public class ConditionSpawner : MonoBehaviour
    {
        [SerializeField] Condition conditionToSpawn;
        [SerializeField] GameObject spawner;
        [SerializeField] bool checkOnUpdate;
        IEnumerable<IPredicateEvaluator> evaluators;

        void Awake()
        {
            evaluators = GameObject.FindWithTag("Player").GetComponents<IPredicateEvaluator>();
        }

        void Start()
        {
            spawner.SetActive(conditionToSpawn.Check(evaluators));
        }

        void Update()
        {
            if(checkOnUpdate)
            {
                spawner.SetActive(conditionToSpawn.Check(evaluators));
            }
        }
    }
}