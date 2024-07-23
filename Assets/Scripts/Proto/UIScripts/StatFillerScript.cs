// StatFillerScript v0.1 Proto

/* 
 *   v0.1
 * - automatic get components added
 * - basic fields are implemented
 * - Lerping function implemented
 */

using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class StatFillerScript : MonoBehaviour
    {
        [SerializeField] private Character character;
        [SerializeField] private Image filler;
        [SerializeField] private Text fillerText;
        [SerializeField] private float smothenLerp;

        private void OnValidate()
        {
            GetNecessaryComponents();
        }

        private void GetNecessaryComponents()
        {
            if (character == null)
                character = GetComponent<Character>();

            if (filler == null)
                filler = gameObject.GetComponent<Image>();

            if (fillerText == null)
                fillerText = gameObject.GetComponentInChildren<Text>();
        }

        private void Update()
        {
            UpdateFiller();
        }

        private void UpdateFiller()
        {
            float _statFillAmount = character.Vitality.FillAmount;
            if (_statFillAmount != filler.fillAmount)
            {
                filler.fillAmount = Mathf.Lerp(filler.fillAmount, _statFillAmount, smothenLerp);
            }
        }
    }
}
