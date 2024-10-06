
using entities;
using entities.Helpers;

namespace repository.Interface
{
    public interface ILogService
    {
        public PaginationList<Log> GetAll(SearchFilter filtro);
    }

}
