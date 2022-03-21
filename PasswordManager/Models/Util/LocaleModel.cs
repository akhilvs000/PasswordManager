using static PasswordManager.Constants;

namespace PasswordManager.Models.Util
{
    public struct LocaleModel
    {
        public static LocaleModel GetDefault()
            => new(AppConfig.Lang_EN, string.Empty);

        public LocaleModel(string language, string country)
        {
            Language = language.ToLower();
            Country = country.ToLower();

            var localeString = $"{Language}{AppConfig.LanguageSeparator}{Country}";

            var supportedLocale = !string.IsNullOrEmpty(Country)
                               && AppConfig.SupportedLanguages.Contains(localeString);

            Identifier = supportedLocale ? localeString : Language;
        }

        public string Language { get; }
        public string Country { get; }
        public string Identifier { get; }
    }
}
