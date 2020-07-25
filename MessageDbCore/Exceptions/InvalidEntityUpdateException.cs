using System;

namespace MessageDbCore.Exceptions
{
    public class InvalidEntityUpdateException<TEntity> : Exception, IBaseEntityException<TEntity>
    {
        public bool IsEntityNull
        {
            get
            {
                return Entity == null;
            }
        }

        public TEntity Entity { get; private set; }

        public InvalidEntityUpdateException(TEntity entity, string message, Exception innerException) : base(message, innerException)
        {
            Entity = entity;
        }
    }
}
