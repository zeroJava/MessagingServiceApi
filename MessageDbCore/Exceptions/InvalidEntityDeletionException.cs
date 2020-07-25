using System;

namespace MessageDbCore.Exceptions
{
    public class InvalidEntityDeletionException<TEntity> : Exception, IBaseEntityException<TEntity>
    {
        public bool IsEntityNull
        {
            get
            {
                return Entity == null;
            }
        }

        public TEntity Entity { get; private set; }

        public InvalidEntityDeletionException(TEntity entity, string message, Exception innerException) : base(message, innerException)
        {
            Entity = entity;
        }
    }
}