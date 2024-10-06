using entities;
using entities.Helpers;
using lib.Cryptography;
using Microsoft.EntityFrameworkCore;
using repositorio.Helpers;
using repository.Context;
using repository.Extensions;
using repository.Interface;
using System.Linq.Expressions;

namespace repository.Services
{
    public class LoginService : ILoginService
    {

        private TurningPointsContext _context;
        public LoginService(TurningPointsContext context)
        {
            _context = context;
        }

        public Login? Get(Guid id)
        {
            var entity = _context.Login.FirstOrDefault(x => x.Id == id);
            
            if (entity == null)
                return null;

            entity.Password = string.Empty;

            return entity;
        }

        public Login? Get(string email)
        {
            var entity = _context.Login.FirstOrDefault(x => x.Email == email);
            
            if (entity == null)
                return null;

            entity.Password = string.Empty;

            return entity;
        }

        private Login GetWithPassword(string email)
        {
            var entity = _context.Login.AsNoTracking().FirstOrDefault(x => x.Email == email);

            if (entity == null)
                return null;

            var date = entity.RecordDate?.ToString("dd/MM/yyyy HH:mm:ss");
            var teste = Cipher.Decrypt(entity.Password, date);

            entity.Password = Cipher.Decrypt(entity.Password, entity.RecordDate?.ToString("dd/MM/yyyy HH:mm:ss"));

            return entity;
        }

        public Login Add(Login entity)
        {

            var dateNow = Convert.ToDateTime(DateTime.Now.ToString());
            entity.RecordDate = dateNow;

            if (!string.IsNullOrEmpty(entity.Password) && entity.RecordDate.HasValue)
            {
                var date = entity.RecordDate?.ToString("dd/MM/yyyy HH:mm:ss");
                if (!string.IsNullOrEmpty(date))
                    entity.Password = Cipher.Encrypt(entity.Password, date);
            }

            entity.Permission = Enumerators.Permission.Standard;

            _context.Login.Add(entity);
            SaveChanges();

            //entity.Password = string.Empty;
            return entity;
        }

        public Login Update(Login entity)
        {
            entity.Password = GetWithPassword(entity.Email)?.Password;
            _context.Login.Update(entity);
            return entity;
        }

        public PaginationList<Login> GetAll(SearchFilter filter)
        {
            List<Expression<Func<Login, bool>>> searchFilters = FilterConfig.GetLoginInternalFilter(filter.Entity);
            int page = filter.PageIndex == 0 ? 1 : filter.PageIndex;

            if (string.IsNullOrEmpty(filter.SortField))
            {
                filter.SortField = "Name";
            }

            IQueryable<Login> query = _context.Login.AsQueryable();
            if (searchFilters != null)
            {
                foreach (var filterValue in searchFilters)
                {
                    query = query.Where(filterValue);
                }
            }

            foreach (var login in query.ToList())
            {
                login.Password = string.Empty;
            }

            return PaginationList<Login>.Create(query.OrderByDynamic(filter.SortField, filter.SortType), page, filter.PageSize);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public bool GetValidLogin(string email, string password)
        {
            var entity = GetWithPassword(email);
            
            if (entity == null || !entity.Password.Equals(password))
                return false;

            return true;

        }

        public bool GetIfTokenIsValid(string token)
        {
            var entity = _context.Login.AsNoTracking().FirstOrDefault(x => x.TokenExpiration < DateTime.Now && x.Token == token);

            if (entity == null)
                return false;

            return true;
        }

        public Login UpdatePassword(Login entity)
        {

            if (!string.IsNullOrEmpty(entity.Password) && entity.RecordDate.HasValue)
            {
                var date = entity.RecordDate?.ToString("dd/MM/yyyy HH:mm:ss");
                if (!string.IsNullOrEmpty(date))
                    entity.Password = Cipher.Encrypt(entity.Password, date);
            }

            _context.Login.Update(entity);
            return entity;
           
        }

        public Enumerators.Permission GetPermission(string email)
        {
            return Get(email).Permission;
        }

    }
}
