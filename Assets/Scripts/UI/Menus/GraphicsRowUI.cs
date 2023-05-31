using System;
using System.Text.RegularExpressions;
using RPG.Graphics;
using TMPro;
using UnityEngine;

namespace RPG.UI.Menus
{
    public class GraphicsRowUI : MonoBehaviour
    {
        [SerializeField] TMP_Dropdown dropdown;
        [SerializeField] TMP_Text graphicsNameText;
        GraphicsManager graphicsManager;
        GraphicSetting graphicSetting;
        Type type;

        public void Setup(GraphicsManager graphicsManager, GraphicSetting graphicSetting, Type type)
        {
            this.graphicsManager = graphicsManager;
            this.graphicSetting = graphicSetting;
            this.type = type;

            FillUI();
        }

        void Start()
        {
            dropdown.onValueChanged.AddListener((int call) => graphicsManager.SetGraphicSetting(graphicSetting, call));
        }

        void FillUI()
        {
            dropdown.ClearOptions();
        
            foreach(var value in Enum.GetValues(type))
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(value.ToString()));
            }

            dropdown.value = graphicsManager.GetSelectedOption(graphicSetting);
            graphicsNameText.text = Regex.Replace(graphicSetting.ToString(), @"([a-z])([A-Z0-9])", "$1 $2");
        }
    }
}
