using RPG.Attributes;
using UnityEngine;

public class ManaBar : MonoBehaviour
{
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Mana mana = null;
        [SerializeField] Canvas rootCanvas = null;

        void Update()
        {
            if(Mathf.Approximately(mana.GetFraction(), 0) || Mathf.Approximately(mana.GetFraction(), 1))
            {
                rootCanvas.enabled = false;
                return;
            }

            rootCanvas.enabled = true;
            foreground.localScale = GetValue();
        }

        Vector3 GetValue()
        {
            return new Vector3(mana.GetFraction(), 1f, 1f);
        }
}
