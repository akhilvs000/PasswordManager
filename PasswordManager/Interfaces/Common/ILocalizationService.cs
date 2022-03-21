using System;
using PasswordManager.Models.Util;

namespace PasswordManager.Interfaces.Common
{
    public interface ILocalizationService
    {
        /// <summary>
        /// returns the localized string for the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string this[string key] { get; }

        event EventHandler<LocaleModel> LanguageChanged;
        void OnLanguageChanged(LocaleModel locale);
    }
}
