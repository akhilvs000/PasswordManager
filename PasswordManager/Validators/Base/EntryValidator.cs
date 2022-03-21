using System;
namespace PasswordManager.Validators.Base
{
    public class EntryValidator : ValidatableObject<string>
    {
        public EntryValidator(params IValidationRule<string>[] rules)
            : base(rules) { }
    }
}
