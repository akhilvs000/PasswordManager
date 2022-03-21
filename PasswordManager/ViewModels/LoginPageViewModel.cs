using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.Interfaces.Common;
using PasswordManager.Interfaces.VM;
using PasswordManager.Models.Util;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using static PasswordManager.Constants;

namespace PasswordManager.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        #region private fields

        private readonly ILoginPageVMService _vmService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IPreferenceService _preferenceService;

        #endregion

        #region properties

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value,
                () => IsWrongPasswordAlertVisible = false);
        }

        private string _languageText;
        public string LanguageText
        {
            get => _languageText;
            set => SetProperty(ref _languageText, value);
        }

        private bool _isWrongPasswordAlertVisible;
        public bool IsWrongPasswordAlertVisible
        {
            get => _isWrongPasswordAlertVisible;
            set => SetProperty(ref _isWrongPasswordAlertVisible, value);
        }

        #endregion

        #region commands

        private ICommand _loginCommand;
        public ICommand LoginCommand
            => _loginCommand ??= new DelegateCommand(async () => await OnLogin(), () => !string.IsNullOrEmpty(Password))
                .ObservesProperty(() => Password);

        private ICommand _changeLanguageCommand;
        public ICommand ChangeLanguageCommand
            => _changeLanguageCommand ??= new DelegateCommand(async () => await OnChangeLanguageTapped());

        #endregion

        #region ctor
        public LoginPageViewModel
        (
            INavigationService navigationService,
            ILocalizationService localizationService,
            ILoginPageVMService loginPageVMService,
            IPreferenceService preferenceService,
            IPageDialogService pageDialogService,
            IPreferenceService preference)
            : base(navigationService, localizationService)
        {
            _vmService = loginPageVMService;
            _pageDialogService = pageDialogService;
            _preferenceService = preference;

            UpdateLanguageText();
        }
        #endregion

        #region overrides

        protected override void HandleLanguageChanged(object sender, LocaleModel locale)
        {
            UpdateLanguageText();
        }

        #endregion

        #region private methods

        private async Task OnLogin() => await ExecuteIfNotBusy(async () =>
        {
            if (string.IsNullOrEmpty(Password))
                IsWrongPasswordAlertVisible = true;

            var loginResult = _vmService.Login(Password);

            IsWrongPasswordAlertVisible = !loginResult;

            if (!loginResult)
                return;

            var param = new NavigationParameters
            {
                { Parameters.MasterPassword, Password }
            };

            await NavigationService.NavigateAsync($"/{Pages.NavigationPage}/{Pages.AccountListPage}", param);
        });

        private async Task OnChangeLanguageTapped() => await ExecuteIfNotBusy(async () =>
        {
            var result = await _pageDialogService.DisplayActionSheetAsync(
                Localization["language_change_text"], Localization["cancel_text"],
                null, AppConfig.Lang_EN, AppConfig.Lang_DE);

            if(result != null && AppConfig.SupportedLanguages.Any(x => x == result))
            {
                UpdateLanguage(result);
            }
        });

        private void UpdateLanguage(string language)
        {
            var locale = new LocaleModel(language, string.Empty);
            _preferenceService.SelectedLocale = locale;
            Localization.OnLanguageChanged(locale);
            RaisePropertyChanged(nameof(Localization));
        }

        private void UpdateLanguageText()
        {
            LanguageText = $"{Localization["language_change_text"]}:{_preferenceService.SelectedLocale?.Language}";
        }

        #endregion
    }
}
