using entities;
using entities.Helpers;

namespace repository.Interface
{
    public  interface IStoryService : IBaseService<Story>  {

        public PaginationList<Story> GetAllOpen(SearchFilter filtro);
    }

}
