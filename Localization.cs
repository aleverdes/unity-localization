using System;
using UnityEngine;
namespace AffenCode
{
    public class Localization : MonoBehaviour
    {
        [SerializeField] private TextAsset _localizationCsv;
        [SerializeField] private string _overrideLanguageInEditor;
        [SerializeField] private bool _onlyEnglishAndRussian;
        
        private static LocalizationTable LocalizationTable { get; set; }
        private static string OverrideLanguageInEditor { get; set; }
        private static bool OnlyEnglishAndRussian { get; set; }

        private void Awake()
        {
            Parse();
        }
        
        private void Parse()
        {
            if (LocalizationTable != null && LocalizationTable.Elements.Count > 0)
            {
                return;
            }

            LocalizationTable = new();
            OverrideLanguageInEditor = _overrideLanguageInEditor;
            OnlyEnglishAndRussian = _onlyEnglishAndRussian;

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

                var localizedElement = new LocalizationTable.Element();
                LocalizationTable.Elements.Add(lineElements[0], localizedElement);
                localizedElement.Key = lineElements[0];

                var k = 0;
                for (var j = 1; j < languages.Length; ++j)
                {
                    k++;
                    try
                    {
                        var value = lineElements[k];
                        if (value.StartsWith("\"") && !value.StartsWith("\"\""))
                        {
                            do
                            {
                                k++;
                                value += "," + lineElements[k];
                            }
                            while (!value.EndsWith("\"") && !value.StartsWith("\"\""));
                        }
                        
                        localizedElement.Values.Add(languages[j], value);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Can't add {languages[j]} = {lineElements[j]}.\n{e}");
                    }
                }
            }
        }
        
        public static string Localize(string key)
        {
            return LocalizationTable.Localize(key);
        }

        public static string GetCurrentLocalizationKey()
        {
            if (Application.isEditor && !string.IsNullOrEmpty(OverrideLanguageInEditor))
            {
                return OverrideLanguageInEditor;
            }

            if (OnlyEnglishAndRussian)
            {
                return Application.systemLanguage is SystemLanguage.Russian or SystemLanguage.Belarusian
                    ? "ru"
                    : "en";
            }
            
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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetLocalization()
        {
            LocalizationTable = null;
            OverrideLanguageInEditor = null;
            OnlyEnglishAndRussian = false;
        }
    }
}
