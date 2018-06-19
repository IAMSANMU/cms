using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace Imow.Framework.Db
{
    public class DbTransactionWrapper: IDisposable
    {
        public DbTransactionWrapper(DbTransaction transaction)
        {
            this.DbTransaction = transaction;
        }
        public DbTransaction DbTransaction { get; private set; }
        public bool IsRollBack { get; set; }
        public void Rollback()
        {
            if (!this.IsRollBack)
            {
                this.DbTransaction.Rollback();
            }
        }
        public void Commit()
        {
            this.DbTransaction.Commit();
        }
        public void Dispose()
        {
         
            this.DbTransaction.Dispose();
        }
    }

    public abstract class Transaction : IDisposable
    {
        [ThreadStatic]
        private static Transaction current;

        public bool Completed { get; private set; }
        public DbTransactionWrapper DbTransactionWrapper { get; protected set; }
        protected Transaction() { }
        public void Rollback()
        {
            this.DbTransactionWrapper.Rollback();
        }
        public DependentTransaction DependentClone()
        {
            return new DependentTransaction(this);
        }
        public void Dispose()
        {
            this.DbTransactionWrapper.Dispose();
        }
        public static Transaction Current
        {
            get { return current; }
            set { current = value; }
        }
    }

    public class CommittableTransaction : Transaction
    {
        public CommittableTransaction(DbTransaction dbTransaction)
        {
            this.DbTransactionWrapper = new DbTransactionWrapper(dbTransaction);
        }
        public void Commit()
        {
            this.DbTransactionWrapper.Commit();
        }
    }
    public class DependentTransaction : Transaction
    {
        public Transaction InnerTransaction { get; private set; }
        internal DependentTransaction(Transaction innerTransaction)
        {
            this.InnerTransaction = innerTransaction;
            this.DbTransactionWrapper = this.InnerTransaction.DbTransactionWrapper;
        }
    }
}