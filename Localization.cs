using System.Collections.Generic;
using System.Linq;
namespace AffenCode
{
    public class Localization
    {
        public readonly Dictionary<string, Element> Elements = new();

        public string Localize(string localizationKey)
        {
            localizationKey = localizationKey.Trim();
            return Elements.ContainsKey(localizationKey) ? Elements[localizationKey].GetValue(LocalizationManager.GetCurrentLocalizationKey()) : $"<{localizationKey}>";
        }

        public class Element
        {
            public readonly Dictionary<string, string> Values = new();
            public string Key;

            public string GetValue(string languageCode)
            {
                if (Values.ContainsKey(languageCode))
                {
                    return Values[languageCode];
                }

                if (Values.ContainsKey("en"))
                {
                    return Values["en"];
                }

                if (Values.ContainsKey("ru"))
                {
                    return Values["ru"];
                }

                return Values.Count > 0 ? Values.FirstOrDefault().Value : $"<{Key ?? "EMPTY"}>";
            }
        }
    }
}
