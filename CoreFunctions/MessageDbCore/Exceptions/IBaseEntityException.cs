namespace MessageDbCore.Exceptions
{
   public interface IBaseEntityException<TEntity>
   {
      bool IsEntityNull { get; }
      TEntity Entity { get; }
   }
}