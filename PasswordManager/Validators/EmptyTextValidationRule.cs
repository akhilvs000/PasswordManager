using PasswordManager.Validators.Base;

namespace PasswordManager.Validators
{
    public class EmptyTextValidationRule : IValidationRule<string>
    {
        public string ValidationMessageKey { get; set; }

        public EmptyTextValidationRule(string validationMessageKey)
        {
            ValidationMessageKey = validationMessageKey;
        }

        public bool Check(string value) => !string.IsNullOrWhiteSpace(value);
    }
}