using PasswordManager.Interfaces.Common;
using PasswordManager.Interfaces.Infrastructure;
using PasswordManager.Interfaces.VM;
using PasswordManager.Interfaces.Wrapper;
using PasswordManager.Models.Util;
using PasswordManager.Services.Common;
using PasswordManager.Services.Infrastructure;
using PasswordManager.Services.VM;
using PasswordManager.Services.Wrapper;
using PasswordManager.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using static PasswordManager.Constants;

namespace PasswordManager
{
    public partial class App
    {
        public App() : this(null)
        {
        }

        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }

        protected override void OnInitialized()
        {
            InitializeComponent();
            SetupAppLanguage();
            SetInitialNavigation();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            var preferencesService = Container.Resolve<IPreferenceService>();
            if (preferencesService.IsMasterPasswordSet)
            {
                NavigationService.NavigateAsync($"/{Pages.NavigationPage}/{Pages.LoginPage}");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //pages
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SetMasterPasswordPage>();
            containerRegistry.RegisterForNavigation<LoginPage>();
            containerRegistry.RegisterForNavigation<AddLoginItemsPage>();
            containerRegistry.RegisterForNavigation<AccountListPage>();

            //singleton services
            containerRegistry.RegisterSingleton<IPreferenceService, PreferenceService>();
            containerRegistry.RegisterSingleton<IRealmService, RealmService>();
            containerRegistry.RegisterSingleton<IDatabaseClient, RealmDatabaseClient>();
            containerRegistry.RegisterSingleton<IXamarinEssentialService, XamarinEssentialService>();
            containerRegistry.RegisterSingleton<ILocalizationService, LocalizationService>();

            //common services
            containerRegistry.Register<ICryptoService, CryptoService>();

            //view model services
            containerRegistry.Register<ISetMasterPasswordPageVMService, SetMasterPasswordPageVMService>();
            containerRegistry.Register<ILoginPageVMService, LoginPageVMService>();
            containerRegistry.Register<IAddLoginItemsPageVMService, AddLoginItemsPageVMService>();
            containerRegistry.Register<IAccountListPageVMService, AccountListPageVMService>();

        }

        private void SetupAppLanguage()
        {
            var preferencesService = Container.Resolve<IPreferenceService>();
            var localizationService = Container.Resolve<ILocalizationService>();
            var deviceLocaleService = Container.Resolve<IDeviceLocaleService>();

            if (preferencesService.SelectedLocale is null)
            {
                var systemLocale = deviceLocaleService.GetLocale();

                if (systemLocale is null || !AppConfig.SupportedLanguages.Contains(systemLocale?.Language))
                {
                    preferencesService.SelectedLocale = LocaleModel.GetDefault();
                }
                else
                {
                    preferencesService.SelectedLocale = systemLocale;
                }
            }

            localizationService.OnLanguageChanged(preferencesService.SelectedLocale.Value);
        }

        private void SetInitialNavigation()
        {
            var preferencesService = Container.Resolve<IPreferenceService>();
            if (preferencesService.IsMasterPasswordSet)
            {
                NavigationService.NavigateAsync($"{Pages.NavigationPage}/{Pages.LoginPage}");
            }
            else
            {
                NavigationService.NavigateAsync($"{Pages.NavigationPage}/{Pages.SetMasterPasswordPage}");
            }
        }
    }
}
