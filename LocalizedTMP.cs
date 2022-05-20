using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
namespace AffenCode
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedTMP : MonoBehaviour
    {
        public TMP_Text Label;
        public string[] LocalizationKeys = new string[1];
        public string StringFormat = "{0}";

        protected virtual void Reset()
        {
            Label = GetComponent<TMP_Text>();
        }

        protected virtual void Start()
        {
            Localize();
        }

        public virtual void Localize()
        {
            var localized = new object[LocalizationKeys.Length];
            for (var i = 0; i < LocalizationKeys.Length; i++)
            {
                localized[i] = Localization.Localize(LocalizationKeys[i]);
            }
            Label.text = string.Format(string.IsNullOrEmpty(StringFormat) ? "{0}" : StringFormat, localized);
        }
    }
}
