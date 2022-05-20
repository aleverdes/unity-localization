using System;
using UnityEngine;
namespace AffenCode
{
    public class LocalizationManager : MonoBehaviour
    {
        [SerializeField] private TextAsset _localizationCsv;
        public static Localization Localization { get;  private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetLocalization()
        {
            Localization = null;
        }

        private void Awake()
        {
            Parse();
        }
        
        private void Parse()
        {
            if (Localization != null && Localization.Elements.Count > 0)
            {
                return;
            }

            Localization = new();

            if (!_localizationCsv)
            {
                return;
            }
            
            var csv = _localizationCsv.text;
            var csvLines = csv.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            var languages = csvLines[0].Split(",", StringSplitOptions.RemoveEmptyEntries);

            for (var i = 1; i < csvLines.Length; ++i)
            {
                var lineElements = csvLines[i].Split(",");

                if (string.IsNullOrEmpty(lineElements[0]))
                {
                    continue;
                }

                var localizedElement = new Localization.Element();
                Localization.Elements.Add(lineElements[0], localizedElement);
                localizedElement.Key = lineElements[0];

                for (var j = 1; j < languages.Length; ++j)
                {
                    try
                    {
                        localizedElement.Values.Add(languages[j], lineElements[j]);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Can't add {languages[j]} = {lineElements[j]}.\n{e}");
                    }
                }
            }
        }

        public static string GetCurrentLocalizationKey()
        {
            var languageCode = GetLocalizationKey(Application.systemLanguage);

            if (string.IsNullOrEmpty(languageCode))
            {
                languageCode = GetAndroidLanguageKey();
            }

            if (string.IsNullOrEmpty(languageCode))
            {
                languageCode = "en";
            }

            return languageCode;
        }

        public static string GetAndroidLanguageKey()
        {
#if UNITY_ANDROID
            try
            {
                using var locale = new AndroidJavaClass("java.util.Locale");
                using var localeInst = locale.CallStatic<AndroidJavaObject>("getDefault");
                var name = localeInst.Call<string>("getLanguage");
                return name;
            }
            catch (Exception e)
            {
                return null;
            }
#else
            return null;
#endif
        }

        public static string GetLocalizationKey(SystemLanguage language)
        {
            return language switch
            {
                SystemLanguage.Afrikaans => "af",
                SystemLanguage.Arabic => "ar",
                SystemLanguage.Basque => "eu",
                SystemLanguage.Belarusian => "be",
                SystemLanguage.Bulgarian => "bg",
                SystemLanguage.Catalan => "ca",
                SystemLanguage.Chinese => "zh-CN",
                SystemLanguage.ChineseSimplified => "zh-CN",
                SystemLanguage.ChineseTraditional => "zh-CN",
                SystemLanguage.Czech => "cs",
                SystemLanguage.Danish => "da",
                SystemLanguage.Dutch => "nl",
                SystemLanguage.English => "en",
                SystemLanguage.Estonian => "et",
                SystemLanguage.Faroese => "is",
                SystemLanguage.Finnish => "fi",
                SystemLanguage.French => "fr",
                SystemLanguage.German => "de",
                SystemLanguage.Greek => "el",
                SystemLanguage.Hebrew => "iw",
                SystemLanguage.Hungarian => "hu",
                SystemLanguage.Icelandic => "is",
                SystemLanguage.Indonesian => "id",
                SystemLanguage.Italian => "it",
                SystemLanguage.Japanese => "ja",
                SystemLanguage.Korean => "ko",
                SystemLanguage.Latvian => "lv",
                SystemLanguage.Lithuanian => "lt",
                SystemLanguage.Norwegian => "no",
                SystemLanguage.Polish => "pl",
                SystemLanguage.Portuguese => "pt",
                SystemLanguage.Romanian => "ro",
                SystemLanguage.Russian => "ru",
                SystemLanguage.SerboCroatian => "sr",
                SystemLanguage.Slovak => "sk",
                SystemLanguage.Slovenian => "sl",
                SystemLanguage.Spanish => "es",
                SystemLanguage.Swedish => "sv",
                SystemLanguage.Thai => "th",
                SystemLanguage.Turkish => "tr",
                SystemLanguage.Ukrainian => "uk",
                SystemLanguage.Vietnamese => "vi",
                SystemLanguage.Unknown => null,
                _ => null
            };
        }
    }
}
