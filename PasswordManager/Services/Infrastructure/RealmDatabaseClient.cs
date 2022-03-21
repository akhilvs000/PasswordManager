using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PasswordManager.Interfaces.Infrastructure;
using PasswordManager.Interfaces.Wrapper;
using PasswordManager.Models.Entity;
using Realms;

namespace PasswordManager.Services.Infrastructure
{
    public class RealmDatabaseClient : IDatabaseClient
    {
        private static readonly DbOperationResult ResultSuccess = new DbOperationResult { IsSuccess = true };
        private static readonly DbOperationResult ResultError = new DbOperationResult { IsSuccess = false };

        private IRealmService _realmService;

        public RealmDatabaseClient(IRealmService realmService)
        {
            _realmService = realmService;
        }

        public IRealm GetRealmInstance()
        {
            return _realmService.GetInstance();
        }

        public DbOperationResult<IQueryable<TData>> GetAll<TData>()
            where TData : RealmObject
        {
            try
            {
                return new DbOperationResult<IQueryable<TData>>
                {
                    IsSuccess = true,
                    Data = GetRealmInstance().All<TData>()
                };
            }
            catch
            {
                return new DbOperationResult<IQueryable<TData>>
                {
                    IsSuccess = false
                };
            }
        }

        public DbOperationResult<TData> Find<TData>(string primaryKey)
            where TData : RealmObject
        {
            var result = GetRealmInstance().Find<TData>(primaryKey);

            return new DbOperationResult<TData>
            {
                IsSuccess = result != null,
                Data = result
            };
        }

        public DbOperationResult<TData> Find<TData>(long? primaryKey)
            where TData : RealmObject
        {
            var result = GetRealmInstance().Find<TData>(primaryKey);

            return new DbOperationResult<TData>
            {
                IsSuccess = result != null,
                Data = result
            };
        }

        public DbOperationResult AddOrUpdate<TData>(TData entry)
            where TData : RealmObject
        {
            try
            {
                var realm = GetRealmInstance();
                realm.Write(() => realm.Add(entry, update: true));

                return ResultSuccess;
            }
            catch
            {
                return ResultError;
            }
        }

        public DbOperationResult Update<TData>(TData entry, Action<TData> updateTransaction)
            where TData : RealmObject
        {
            try
            {
                var realm = GetRealmInstance();

                realm.Write(() =>
                {
                    updateTransaction(entry);
                    realm.Add(entry, update: true);
                });

                return ResultSuccess;
            }
            catch
            {
                return ResultError;
            }
        }

        public DbOperationResult Write(Action transaction)
        {
            try
            {
                GetRealmInstance().Write(transaction);
                return ResultSuccess;
            }
            catch
            {
                return ResultError;
            }
        }

        public DbOperationResult Write(IRealm realm, Action transaction)
        {
            try
            {
                realm.Write(transaction);
                return ResultSuccess;
            }
            catch
            {
                return ResultError;
            }
        }


        public async Task<DbOperationResult> DeleteAsync<TData>(TData entry)
            where TData : RealmObject
        {
            try
            {
                await GetRealmInstance().WriteAsync(realm => realm.Remove(entry));
                return ResultSuccess;
            }
            catch
            {
                return ResultError;
            }
        }

        public async Task<DbOperationResult> DeleteRangeAsync<TData>(Expression<Func<TData, bool>> predicate)
            where TData : RealmObject
        {
            try
            {
                await GetRealmInstance().WriteAsync(realm =>
                {
                    var entriesToDelete = realm.All<TData>().Where(predicate);
                    realm.RemoveRange(entriesToDelete);
                });

                return ResultSuccess;
            }
            catch
            {
                return ResultError;
            }
        }

        public async Task<DbOperationResult> WriteAsync(Action<IRealm> transaction)
        {
            try
            {
                await GetRealmInstance().WriteAsync(transaction);
                return ResultSuccess;
            }
            catch
            {
                return ResultError;
            }
        }
    }
}
