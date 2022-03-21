using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PasswordManager.Interfaces.Wrapper;
using PasswordManager.Models.Entity;
using Realms;

namespace PasswordManager.Interfaces.Infrastructure
{
    public interface IDatabaseClient
    {
        //void ConfigureRealm(RealmConfiguration configuration);
        IRealm GetRealmInstance();

        DbOperationResult<IQueryable<TData>> GetAll<TData>() where TData : RealmObject;
        DbOperationResult<TData> Find<TData>(string primaryKey) where TData : RealmObject;
        DbOperationResult<TData> Find<TData>(long? primaryKey) where TData : RealmObject;
        DbOperationResult AddOrUpdate<TData>(TData entry) where TData : RealmObject;
        DbOperationResult Update<TData>(TData entry, Action<TData> updateTransaction) where TData : RealmObject;
        DbOperationResult Write(Action transaction);
        DbOperationResult Write(IRealm realm, Action transaction);

        Task<DbOperationResult> DeleteAsync<TData>(TData entry) where TData : RealmObject;
        Task<DbOperationResult> DeleteRangeAsync<TData>(Expression<Func<TData, bool>> predicate) where TData : RealmObject;
        Task<DbOperationResult> WriteAsync(Action<IRealm> transaction);
    }
}
