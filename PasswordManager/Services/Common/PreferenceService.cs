using System;
using PasswordManager.Interfaces.Common;
using PasswordManager.Interfaces.Wrapper;
using PasswordManager.Models.Util;
using static PasswordManager.Constants;

namespace PasswordManager.Services.Common
{
    public class PreferenceService : IPreferenceService
    {
        private const string IsMasterPasswordSetKey = "IS_MASTER_PASSWORD_SET";
        private const string SelectedLocaleKey = "SELECTED_LOCALE";

        private readonly IXamarinEssentialService _xamarinEssentialsService;

        public PreferenceService(IXamarinEssentialService xamarinEssentialService)
        {
            _xamarinEssentialsService = xamarinEssentialService;
        }

        /// <summary>
        /// True if the user set masterpassword
        /// </summary>
        public bool IsMasterPasswordSet
        {
            get => _xamarinEssentialsService.GetPreference(IsMasterPasswordSetKey, false);
            set => _xamarinEssentialsService.SetPreference(IsMasterPasswordSetKey, value);
        }

        /// <summary>
        /// get or set the selected language
        /// </summary>
        public LocaleModel? SelectedLocale
        {
            get
            {
                string locale = _xamarinEssentialsService.GetPreference(SelectedLocaleKey, null);
                try
                {
                    var items = locale.Split(AppConfig.LanguageSeparator);

                    return new LocaleModel(items[0], items.Length > 1 ? items[1] : string.Empty);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set => _xamarinEssentialsService.SetPreference(SelectedLocaleKey, value?.Identifier ?? string.Empty);
        }
    }
}
