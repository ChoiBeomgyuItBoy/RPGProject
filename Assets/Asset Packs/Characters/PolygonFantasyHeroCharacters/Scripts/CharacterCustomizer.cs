using System.Collections.Generic;
using UnityEngine;

// Code provided by @darkrainbowsprinkles

namespace PsychoticLab
{
    public class CharacterCustomizer : MonoBehaviour
    {
        [SerializeField] string[] defaultCharacterParts;
        Dictionary<string, Transform> characterPartLookup = null;

        public void SetCharacterPart(string partName, bool state)
        {
            if(characterPartLookup == null) 
            {
                BuildLookup();
            }

            if(!characterPartLookup.ContainsKey(partName))
            {
                Debug.LogError($"Character part {partName} not found");
                return;
            }

            Transform selectedPart = characterPartLookup[partName];

            selectedPart.gameObject.SetActive(state);
        }

        void Awake()
        {
            foreach(var part in defaultCharacterParts)
            {
                SetCharacterPart(part, true);
            }
        }

        void BuildLookup()
        {
            characterPartLookup = new Dictionary<string, Transform>();

            // Male Parts
            AddToLookup("Male_Head_All_Elements");
            AddToLookup("Male_Head_No_Elements");
            AddToLookup("Male_01_Eyebrows");
            AddToLookup("Male_02_FacialHair");
            AddToLookup("Male_03_Torso");
            AddToLookup("Male_04_Arm_Upper_Right");
            AddToLookup("Male_05_Arm_Upper_Left");
            AddToLookup("Male_06_Arm_Lower_Right");
            AddToLookup("Male_07_Arm_Lower_Left");
            AddToLookup("Male_08_Hand_Right");
            AddToLookup("Male_09_Hand_Left");
            AddToLookup("Male_10_Hips");
            AddToLookup("Male_11_Leg_Right");
            AddToLookup("Male_12_Leg_Left");

            // Female Parts
            AddToLookup("Female_Head_All_Elements");
            AddToLookup("Female_Head_No_Elements");
            AddToLookup("Female_01_Eyebrows");
            AddToLookup("Female_02_FacialHair");
            AddToLookup("Female_03_Torso");
            AddToLookup("Female_04_Arm_Upper_Right");
            AddToLookup("Female_05_Arm_Upper_Left");
            AddToLookup("Female_06_Arm_Lower_Right");
            AddToLookup("Female_07_Arm_Lower_Left");
            AddToLookup("Female_08_Hand_Right");
            AddToLookup("Female_09_Hand_Left");
            AddToLookup("Female_10_Hips");
            AddToLookup("Female_11_Leg_Right");
            AddToLookup("Female_12_Leg_Left");

            // All Gender Parts
            AddToLookup("All_01_Hair");
            AddToLookup("All_02_Head_Attachment");
            AddToLookup("HeadCoverings_Base_Hair");
            AddToLookup("HeadCoverings_No_FacialHair");
            AddToLookup("HeadCoverings_No_Hair");
            AddToLookup("All_03_Chest_Attachment");
            AddToLookup("All_04_Back_Attachment");
            AddToLookup("All_05_Shoulder_Attachment_Right");
            AddToLookup("All_06_Shoulder_Attachment_Left");
            AddToLookup("All_07_Elbow_Attachment_Right");
            AddToLookup("All_08_Elbow_Attachment_Left");
            AddToLookup("All_09_Hips_Attachment");
            AddToLookup("All_10_Knee_Attachement_Right");
            AddToLookup("All_11_Knee_Attachement_Left");
            AddToLookup("Elf_Ear");

            // Other
            AddToLookup("All_Shields");
        }

        void AddToLookup(string rootName)
        {
            foreach(Transform root in GetComponentsInChildren<Transform>())
            {
                if(root.name != rootName) continue;

                foreach(Transform child in root)
                {
                    if(characterPartLookup.ContainsKey(child.name))
                    {
                        Debug.LogError($"Duplicated character part name: {child.name}");
                        return;
                    }

                    characterPartLookup[child.name] = child;
                    child.gameObject.SetActive(false);

                    if(rootName == "Shield")
                    {
                        print(characterPartLookup[child.name]);
                    }
                }
            }
        }
    }
}