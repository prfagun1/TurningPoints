
using entities;
using entities.Helpers;

namespace repository.Interface
{
    public interface IParameterInternalService 
    {
        public ParameterInternal? Get(Guid id);
        public ParameterInternal? Get(string nome);
        public ParameterInternal Add(ParameterInternal entidade);
        public ParameterInternal Update(ParameterInternal entidade);
        public void SaveChanges();
        public int GetItemsPerPage();
        public PaginationList<ParameterInternal> GetAll(SearchFilter filtro);
    }
}
