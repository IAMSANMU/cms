using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using imow.Core.config;
using imow.Framework.Config;
using Imow.Framework.Engine;

namespace Imow.Framework.Db
{
    public class TransactionScope : IDisposable
    {
        private Transaction transaction = Transaction.Current;
        public bool Completed { get; private set; }

        public TransactionScope(IsolationLevel isolationLevel = IsolationLevel.Unspecified,
            Func<string, DbProviderFactory> getFactory = null)
        {
            string connStr = ImowEngineContext.Current.ResolveConfig<imow.Core.config.DBConfig>().ConnectionString;
            if (null == transaction)
            {
                if (null == getFactory)
                {
                    getFactory = cnnstringName => DbConfig.GetDbProviderFactory(ref connStr);
                }
                DbProviderFactory factory = getFactory(connStr);
                DbConnection connection = factory.CreateConnection();
                connection.ConnectionString = connStr;
                connection.Open();
                DbTransaction dbTransaction = connection.BeginTransaction(isolationLevel);
                Transaction.Current = new CommittableTransaction(dbTransaction);
            }
            else
            {
                Transaction.Current = transaction.DependentClone();
            }
        }

        public void Complete()
        {
            this.Completed = true;
        }
        public void Dispose()
        {
            Transaction current = Transaction.Current;
            Transaction.Current = transaction;
            if (!this.Completed)
            {
                current.Rollback();
            }
            CommittableTransaction committableTransaction = current as CommittableTransaction;
            if (null != committableTransaction)
            {
                if (this.Completed)
                {
                    committableTransaction.Commit();
                }
                committableTransaction.Dispose();
            }
        }
    }
}
