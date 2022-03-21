using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Realms;

namespace PasswordManager.Interfaces.Wrapper
{
    public interface IRealm
    {
        void AddAll<T>(IEnumerable<T> list) where T : RealmObject;
        IQueryable<T> All<T>() where T : RealmObject;
        void Write(Action action);
        T Add<T>(T obj, bool update = false) where T : RealmObject;
        void RemoveRange<T>(IQueryable<T> range) where T : RealmObject;
        T Find<T>(long? primaryKey) where T : RealmObject;
        T Find<T>(string primaryKey) where T : RealmObject;
        Task WriteAsync(Action<IRealm> action);
        void Remove(RealmObject obj);
        void RemoveAll<T>() where T : RealmObject;
        void RemoveAll();
    }
}
