using UnityEngine;
using TMPro;
using GameDevTV.Inventories;
using RPG.Combat;
using RPG.Inventories;
using UnityEngine.UI;
using RPG.Abilities;

namespace GameDevTV.UI.Inventories
{
    /// <summary>
    /// Root of the tooltip prefab to expose properties to other classes.
    /// </summary>
    public class ItemTooltip : MonoBehaviour
    {
        // CONFIG DATA
        [SerializeField] TextMeshProUGUI itemNameText = null;
        [SerializeField] Image itemIcon = null;
        [SerializeField] TextMeshProUGUI itemDescriptionText = null;
        [SerializeField] TextMeshProUGUI itemInfoText = null;

        // PUBLIC

        public void Setup(InventoryItem item)
        {
            itemNameText.text = item.GetDisplayName();
            itemIcon.sprite = item.GetIcon();
            itemDescriptionText.text = item.GetDescription();
            itemInfoText.text = "";

            CheckIfWeapon(item);
            CheckIfStatsEquipableItem(item);
            CheckIfAbility(item);
        }

        void CheckIfWeapon(InventoryItem item)
        {
            var weapon = item as WeaponConfig;

            if (weapon != null)
            {
                itemInfoText.text = $"Weapon Stats:";
                itemInfoText.text += $"\n   - Base Damage: {weapon.GetDamage()} HP.";
                itemInfoText.text += $"\n   - Bonus Damage: {weapon.GetPercentageBonus()}%";
                itemInfoText.text += $"\n   - Range: {weapon.GetRange()}m.";
            }
        }

        void CheckIfStatsEquipableItem(InventoryItem item)
        {
            var statsEquipableItem = item as StatsEquipableItem;

            if (statsEquipableItem != null)
            {
                itemInfoText.text = $"Modifiers: ";

                if(statsEquipableItem.GetAdditiveModifiers() != null)
                {
                    foreach (var additiveModifier in statsEquipableItem.GetAdditiveModifiers())
                    {
                        itemInfoText.text += $"\n   +{additiveModifier.value} {additiveModifier.stat}";
                    }
                }

                if(statsEquipableItem.GetPercentageModifiers() != null)
                {
                    foreach (var percentageModifier in statsEquipableItem.GetPercentageModifiers())
                    {
                        itemInfoText.text += $"\n   +{percentageModifier.value}% {percentageModifier.stat}";
                    }
                }
            }
        }

        void CheckIfAbility(InventoryItem item)
        {
            Ability ability = item as Ability;

            if(ability != null)
            {
                itemInfoText.text = $"Effects:";

                foreach(var effect in ability.GetEffects())
                {
                    if(effect.GetEffectInfo() == null) continue;

                    foreach(var info in effect.GetEffectInfo())
                    {
                        itemInfoText.text += $"\n   - {info}";
                    }
                }
            }
        }
    }
}
