using System;
using PasswordManager.Models.Util;

namespace PasswordManager.Interfaces.Common
{
    /// <summary>
    /// This service acts as synchronous data storage for other services.
    /// </summary>
    public interface IPreferenceService
    {
        /// <summary>
        /// If true, user has set the master password
        /// </summary>
        bool IsMasterPasswordSet {get;set;}

        /// <summary>
        /// Returns the selected language
        /// </summary>
        LocaleModel? SelectedLocale { get; set; }

    }
}
