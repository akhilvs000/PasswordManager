using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.Interfaces.Common;
using PasswordManager.Interfaces.VM;
using PasswordManager.Validators;
using PasswordManager.Validators.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using static PasswordManager.Constants;

namespace PasswordManager.ViewModels
{
    public class SetMasterPasswordPageViewModel : ViewModelBase
    {
        #region private fields

        private readonly ISetMasterPasswordPageVMService _vmService;
        private readonly IPageDialogService _pageDialogService;

        #endregion

        #region properties

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private EntryValidator _passwordValidator;
        public EntryValidator PasswordValidator
        {
            get => _passwordValidator;
            set => SetProperty(ref _passwordValidator, value);
        }

        #endregion

        #region commands

        private ICommand _setpasswordCommand;
        public ICommand SetpasswordCommand
            => _setpasswordCommand ??= new DelegateCommand(async () => await OnSetPassword(), () => PasswordValidator.IsValid)
                .ObservesProperty(() => PasswordValidator.IsValid);

        #endregion

        #region ctor
        public SetMasterPasswordPageViewModel
        (
            INavigationService navigationService,
            ILocalizationService localizationService,
            ISetMasterPasswordPageVMService setMasterPasswordPageVMService,
            IPageDialogService pageDialogService
        )
            : base(navigationService, localizationService)
        {
            _vmService = setMasterPasswordPageVMService;
            _pageDialogService = pageDialogService;

            PasswordValidator = new EntryValidator(
                new EmptyTextValidationRule(Localization["master_password_empty_alert"]),
                new ShortTextValidationRule(8, Localization["password_length_validation_error"]));            
        }

        #endregion

        #region private methods

        private async Task OnSetPassword() => await ExecuteIfNotBusy(async () =>
        {
            if (string.IsNullOrEmpty(Password))
            {
                PasswordValidator.Validate();

                await _pageDialogService.DisplayAlertAsync(Localization["alert_text"],
                    Localization["shared_unknown_error_message"], Localization["ok_btn_text"]);
                return;
            }

            _vmService.SetMasterPassword(Password);

            var param = new NavigationParameters
            {
                { Parameters.MasterPassword, Password }
            };
            await NavigationService.NavigateAsync($"/{Pages.NavigationPage}/{Pages.AccountListPage}", param);
        });

        #endregion
    }
}
