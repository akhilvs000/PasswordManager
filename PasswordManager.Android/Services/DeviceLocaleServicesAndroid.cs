using System;
using Java.Util;
using PasswordManager.Interfaces.Common;
using PasswordManager.Models.Util;

namespace PasswordManager.Droid.Services
{
    public class DeviceLocaleServicesAndroid : IDeviceLocaleService
    {
        public LocaleModel? GetLocale()
        {
            LocaleModel locale;

            try
            {
                locale = new LocaleModel(Locale.Default.Language, Locale.Default.Country);
            }
            catch (Exception)
            {
                return null;
            }

            return locale;
        }
    }
}
