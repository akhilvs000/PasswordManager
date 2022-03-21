using System;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.Interfaces.Common;
using PasswordManager.Models.Util;
using Prism.AppModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace PasswordManager.ViewModels
{
    public class ViewModelBase : BindableBase, INavigatedAware, IDestructible, IInitialize, IPageLifecycleAware
    {
        #region properties

        /// <summary>
        /// Provides page based navigation
        /// </summary>
        public INavigationService NavigationService { get; }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// Used to prevent multiple clicks
        /// </summary>
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ILocalizationService Localization { get; }

        #endregion
       
        #region ctor

        public ViewModelBase
        (
            INavigationService navigationService,
            ILocalizationService localizationService
        )
        {
            NavigationService = navigationService;
            Localization = localizationService;

            Localization.LanguageChanged += HandleLanguageChanged;
        }

        #endregion

        public virtual void Initialize(INavigationParameters parameters) { }

        public virtual void OnNavigatedFrom(INavigationParameters parameters) { }

        public virtual void OnNavigatedTo(INavigationParameters parameters) { }

        public virtual void OnAppearing() { }

        public virtual void OnDisappearing() { }        

        public virtual void Destroy()
        {
            Localization.LanguageChanged -= HandleLanguageChanged;
        }

        protected virtual void HandleLanguageChanged(object sender, LocaleModel locale) { }

        /// <summary>
        /// to prevent multiple actions at the same time
        /// </summary>
        /// <param name="actionToExecute"></param>
        protected void ExecuteIfNotBusy(Action actionToExecute)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                actionToExecute();
            }
            catch (Exception)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// to prevent multiple actions at the same time
        /// </summary>
        /// <param name="actionToExecute"></param>
        /// <returns></returns>
        protected async Task ExecuteIfNotBusy(Func<Task> actionToExecute)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await actionToExecute();
            }
            catch (Exception)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }        
    }
}
