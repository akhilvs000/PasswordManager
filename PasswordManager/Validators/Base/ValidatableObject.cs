using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Prism.Mvvm;

namespace PasswordManager.Validators.Base
{
    public class ValidatableObject<T> : BindableBase
    {
        private readonly List<IValidationRule<T>> _rules;

        public event PropertyChangedEventHandler Validated;

        private string _currentError = "default";

        public List<IValidationRule<T>> Rules => _rules;

        private List<string> _errors = new List<string>();
        public List<string> Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }

        private T _value;
        public T Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        private bool _isValid = true;
        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid, value);
        }

        public ValidatableObject(params IValidationRule<T>[] rules)
        {
            _rules = rules.ToList();
        }

        public void SetError(string errorText)
        {
            Errors = new List<string> { errorText };
            IsValid = false;
            Validated?.Invoke(null, null);
        }

        public bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = _rules.Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessageKey);

            Errors = errors.ToList();
            IsValid = !Errors.Any();

            if (_currentError != Errors.FirstOrDefault())
            {
                _currentError = Errors.FirstOrDefault();
                RaisePropertyChanged(nameof(IsValid));
            }

            Validated?.Invoke(null, null);
            return IsValid;
        }
    }
}