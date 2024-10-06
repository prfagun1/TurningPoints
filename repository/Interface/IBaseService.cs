using entities.Helpers;

namespace repository.Interface
{
    public interface IBaseService<T>
    {
        public T? Get(Guid id);
        public T Add(T entidade);
        public T Update(T entidade);
        public void Delete(Guid id);
        public void Delete(T entidade);
        public PaginationList<T> GetAll(SearchFilter filtro);
        public void SaveChanges();
    }
}
