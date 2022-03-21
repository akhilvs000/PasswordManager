using PasswordManager.Validators.Base;

namespace PasswordManager.Validators
{
    public class ShortTextValidationRule : IValidationRule<string>
    {
        private int _validLength;
        public string ValidationMessageKey { get; set; }

        public ShortTextValidationRule(int validLength, string validationMessageKey)
        {
            _validLength = validLength;
            ValidationMessageKey = validationMessageKey;
        }

        public bool Check(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length < _validLength)
                return false;

            return true;
        }
    }
}