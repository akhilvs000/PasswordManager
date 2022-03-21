using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.Interfaces.Common;
using PasswordManager.Interfaces.VM;
using PasswordManager.Models.UI;
using Prism.Commands;
using Prism.Navigation;
using static PasswordManager.Constants;

namespace PasswordManager.ViewModels
{
    public class AccountListPageViewModel : ViewModelBase
    {
        #region fields

        private readonly IAccountListPageVMService _vmService;

        private string _masterPassword;
        #endregion

        #region properties

        private ObservableCollection<AccountDetails> _accountDetailList;
        public ObservableCollection<AccountDetails> AccountDetailList
        {
            get => _accountDetailList;
            set => SetProperty(ref _accountDetailList, value);
        }

        #endregion

        #region commands

        private ICommand _accountItemTappedCommand;
        public ICommand AccountItemTappedCommand
            => _accountItemTappedCommand ??= new DelegateCommand<AccountDetails>(async (obj) => await OnAccountItemTapped(obj));

        private ICommand _addLoginItemCommand;
        public ICommand AddLoginItemCommand
            => _addLoginItemCommand ??= new DelegateCommand(async () => await OnAddLoginItemTapped());

        #endregion

        #region ctor
        public AccountListPageViewModel
        (
            INavigationService navigationService,
            ILocalizationService localizationService,
            IAccountListPageVMService accountListPageVMService
        )
            : base(navigationService, localizationService)
        {
            _vmService = accountListPageVMService;
        }

        #endregion

        #region overridings

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if(parameters.TryGetValue(Parameters.MasterPassword, out _masterPassword))
            {
                GetAccountLists(_masterPassword);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.GetNavigationMode() == NavigationMode.Back &&
                parameters.TryGetValue(Parameters.Reload, out bool reload) && reload)
            {
                GetAccountLists(_masterPassword);
            }
        }

        #endregion

        #region private  methods

        private void GetAccountLists(string password)
        {
            AccountDetailList = _vmService.GetAccountDetailsList(password);
        }

        private async Task OnAddLoginItemTapped() => await ExecuteIfNotBusy(async () =>
        {
            var param = new NavigationParameters
            {
                { Parameters.MasterPassword, _masterPassword }
            };

            await NavigationService.NavigateAsync(Pages.AddLoginItemPage, param);
        });

        private async Task OnAccountItemTapped(AccountDetails obj) => await ExecuteIfNotBusy(async () =>
        {
            var param = new NavigationParameters
            {
                { Parameters.MasterPassword, _masterPassword },
                { Parameters.AccountDetails, obj },
            };

            await NavigationService.NavigateAsync(Pages.AddLoginItemPage, param);
        });
        #endregion
    }
}
