using System;
using PasswordManager.Validators.Base;
using Xamarin.Forms;

namespace PasswordManager.Controls
{
    public class ValidationEntry : Entry, IDisposable
    {
        #region Bindable properties

        public static readonly BindableProperty ValidatorProperty =
            BindableProperty.Create(nameof(Validator),
            typeof(EntryValidator), typeof(ValidationEntry),
            new EntryValidator(), BindingMode.Default, null,
            propertyChanged: ValidatorValueChanged);

        public EntryValidator Validator
        {
            get => (EntryValidator)GetValue(ValidatorProperty);
            set => SetValue(ValidatorProperty, value);
        }

        #endregion

        #region ctor

        public ValidationEntry()
        {
            AddEntryTextChanged();
        }

        #endregion

        #region public methods

        public void Dispose()
        {
            RemoveEntryTextChanged();
        }

        #endregion

        #region private methods

        private void AddEntryTextChanged()
        {
            RemoveEntryTextChanged();
            TextChanged += EntryOnTextChanged;
        }

        private void RemoveEntryTextChanged() => TextChanged -= EntryOnTextChanged;

        private void EntryOnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Validator != null && Validator.Value != e.NewTextValue)
            {
                Validator.Value = e.NewTextValue;
                Validator.Validate();
            }
        }

        private static void ValidatorValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ValidationEntry entry && oldValue != newValue && newValue is EntryValidator validator)
            {
                validator.Value = entry?.Text;
            }
        }

        #endregion
    }
}
