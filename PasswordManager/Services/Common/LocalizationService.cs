using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using PasswordManager.Interfaces.Common;
using PasswordManager.Models.Util;
using PasswordManager.Resources;
using Prism.Mvvm;

namespace PasswordManager.Services.Common
{
    public class LocalizationService : BindableBase, ILocalizationService
    {
        public const string TranslationsProperty = "TRANSLATIONS_PROPERTY";

        [IndexerName(TranslationsProperty)]
        public string this[string key]
        {
            get
            {
                var translation = AppResources.ResourceManager.GetString(key, AppResources.Culture);

                if (string.IsNullOrEmpty(translation))
                {
                    if (translation == null)
                    {
                        Debug.WriteLine($"WARNING: Key '{key}' was not found in resources for culture '{AppResources.Culture?.Name}'");
                    }
                    else
                    {
                        Debug.WriteLine($"WARNING: Translation for '{key}' is not set in resources for culture '{AppResources.Culture?.Name}'");
                    }

                    translation = key;
                }

                return translation;
            }
        }

        public event EventHandler<LocaleModel> LanguageChanged;

        public void OnLanguageChanged(LocaleModel locale)
        {
            var culture = CultureInfo.CreateSpecificCulture(locale.Identifier);

            AppResources.Culture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            RaisePropertyChanged(TranslationsProperty);
            LanguageChanged?.Invoke(this, locale);
        }
    }
}
