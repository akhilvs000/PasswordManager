using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.Interfaces.Common;
using PasswordManager.Interfaces.VM;
using PasswordManager.Models.UI;
using PasswordManager.Validators;
using PasswordManager.Validators.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using static PasswordManager.Constants;

namespace PasswordManager.ViewModels
{
    public class AddLoginItemsPageViewModel : ViewModelBase
    {
        #region private fields

        private readonly IAddLoginItemsPageVMService _vmService;
        private readonly IPageDialogService _pageDialogService;
        private string _masterPassword;

        #endregion

        #region properties
        
        private AccountDetails _accountDetails = new AccountDetails();
        public AccountDetails AccountDetails
        {
            get => _accountDetails;
            set => SetProperty(ref _accountDetails, value);
        }

        private EntryValidator _websiteNameValidator;
        public EntryValidator WebsiteNameValidator
        {
            get => _websiteNameValidator;
            set => SetProperty(ref _websiteNameValidator, value);
        }

        private EntryValidator _websiteUrlValidator;
        public EntryValidator WebsiteUrlValidator
        {
            get => _websiteUrlValidator;
            set => SetProperty(ref _websiteUrlValidator, value);
        }
        
        private EntryValidator _usernameValidator;
        public EntryValidator UsernameValidator
        {
            get => _usernameValidator;
            set => SetProperty(ref _usernameValidator, value);
        }
        
        private EntryValidator _passwordValidator;
        public EntryValidator PasswordValidator
        {
            get => _passwordValidator;
            set => SetProperty(ref _passwordValidator, value);
        }

        private bool _isReadonlyMode;
        public bool IsReadonlyMode
        {
            get => _isReadonlyMode;
            set => SetProperty(ref _isReadonlyMode, value);
        }

        private bool _isPassword = true;
        public bool IsPassword
        {
            get => _isPassword;
            set => SetProperty(ref _isPassword, value);
        }

        #endregion

        #region commands

        private ICommand _saveAccountCommand;
        public ICommand SaveAccountCommand
            => _saveAccountCommand ??= new DelegateCommand(async () => await OnSaveAccount(), () => WebsiteNameValidator.IsValid &&
            WebsiteUrlValidator.IsValid && UsernameValidator.IsValid && PasswordValidator.IsValid)
            .ObservesProperty(() => WebsiteNameValidator.IsValid)
            .ObservesProperty(() => WebsiteUrlValidator.IsValid)
            .ObservesProperty(() => UsernameValidator.IsValid)
            .ObservesProperty(() => PasswordValidator.IsValid);

        private ICommand _editCommand;
        public ICommand EditCommand
            => _editCommand ??= new DelegateCommand(OnEditTapped);

        private ICommand _showorHidePasswordCommand;
        public ICommand ShoworHidePasswordCommand
            => _showorHidePasswordCommand ??= new DelegateCommand(OnShowOrHidePassword);

        private ICommand _deleteAccountDetailsCommand;
        public ICommand DeleteAccountDetailsCommand
            => _deleteAccountDetailsCommand ??= new DelegateCommand(async () => await OnDeleteAccountDetails());

        #endregion

        #region ctor
        public AddLoginItemsPageViewModel
        (
            INavigationService navigationService,
            ILocalizationService localizationService,
            IAddLoginItemsPageVMService addLoginItemsPageVMService,
            IPageDialogService pageDialogService
        )
            : base(navigationService, localizationService)
        {
            _vmService = _ = addLoginItemsPageVMService;
            _pageDialogService = pageDialogService;
            InitializeValidators();
        }
        #endregion

        #region overrides

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            parameters.TryGetValue(Parameters.MasterPassword, out _masterPassword);

            if(parameters.TryGetValue(Parameters.AccountDetails, out AccountDetails account))
            {
                AccountDetails = account;
                IsReadonlyMode = true;
            }
        }

        #endregion

        #region private methods

        private async Task OnSaveAccount() => await ExecuteIfNotBusy(async () =>
        {
            Validate();

            if (!WebsiteNameValidator.IsValid || !WebsiteUrlValidator.IsValid ||
                !UsernameValidator.IsValid || !PasswordValidator.IsValid)
                return;

            var isSuccess = _vmService.SaveAccountDetails(AccountDetails, _masterPassword);

            if(!isSuccess)
            {
                await _pageDialogService.DisplayAlertAsync(Localization["alert_text"],
                    Localization["shared_unknown_error_message"], Localization["ok_btn_text"]);
                return;
            }

            var param = new NavigationParameters
            {
                { Parameters.Reload, true }
            };
            await NavigationService.GoBackAsync(param);
        });

        private void OnEditTapped() => ExecuteIfNotBusy(() =>
        {
            IsReadonlyMode = false;
        });

        private void OnShowOrHidePassword() => ExecuteIfNotBusy(() =>
        {
            IsPassword = !IsPassword;
        });

        private async Task OnDeleteAccountDetails() => await ExecuteIfNotBusy(async () =>
        {
            _vmService.DeleteAccountDetails(AccountDetails);

            var param = new NavigationParameters
            {
                { Parameters.Reload, true }
            };

            await NavigationService.GoBackAsync(param);
        });

        private void InitializeValidators()
        {
            WebsiteNameValidator = new EntryValidator(
                new EmptyTextValidationRule(Localization["website_name_empty_alert"]));

            WebsiteUrlValidator = new EntryValidator(
                new EmptyTextValidationRule(Localization["website_url_empty_alert"]));

            UsernameValidator = new EntryValidator(
                new EmptyTextValidationRule(Localization["username_empty_alert"]));

            PasswordValidator = new EntryValidator(
                new EmptyTextValidationRule(Localization["password_empty_alert"]));
        }

        private void Validate()
        {
            WebsiteNameValidator.Validate();
            WebsiteUrlValidator.Validate();
            UsernameValidator.Validate();
            PasswordValidator.Validate();
        }

        #endregion
    }
}
