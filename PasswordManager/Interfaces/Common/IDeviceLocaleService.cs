using PasswordManager.Models.Util;

namespace PasswordManager.Interfaces.Common
{
    public interface IDeviceLocaleService
    {
        /// <summary>
        /// gets the current device locale
        /// </summary>
        /// <returns></returns>
        LocaleModel? GetLocale();
    }
}
