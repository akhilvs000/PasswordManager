using System;
using Foundation;
using PasswordManager.Interfaces.Common;
using PasswordManager.Models.Util;

namespace PasswordManager.iOS.Services
{
    public class DeviceLocaleServicesiOS : IDeviceLocaleService
    {
        public LocaleModel? GetLocale()
        {
            if (NSLocale.PreferredLanguages.Length > 0)
            {
                var tmp = NSLocale.PreferredLanguages[0]; //en, en-US, de-US
                var items = tmp.Split("-");

                return new LocaleModel(items.Length > 0 ? items[0] : string.Empty,
                                       items.Length > 1 ? items[1] : string.Empty);
            }

            return null;
        }
    }
}
