
namespace PasswordManager.Validators.Base
{
    public interface IValidationRule<T>
    {
        string ValidationMessageKey { get; set; }
        bool Check(T value);
    }
}