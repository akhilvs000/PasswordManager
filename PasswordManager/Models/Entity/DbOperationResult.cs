using System;
namespace PasswordManager.Models.Entity
{
    public class DbOperationResult
    {
        public bool IsSuccess { get; set; }
    }

    public class DbOperationResult<TData> : DbOperationResult
    {
        public TData Data { get; set; }
    }
}
