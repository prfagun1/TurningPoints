using entities;
using entities.Helpers;

namespace repository.Interface
{
    public interface ILoginService
    {
        public Login? Get(Guid id);
        public Login? Get(string email);
        public Login Add(Login entity);
        public Login Update(Login entity);

        public Login UpdatePassword(Login entity);
        public void SaveChanges();
        public PaginationList<Login> GetAll(SearchFilter filtro);
        public bool GetValidLogin(string email, string password);
        public bool GetIfTokenIsValid(string token);

        public Enumerators.Permission GetPermission(string email);

    }
}
