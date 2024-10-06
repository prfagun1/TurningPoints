
using System.Linq.Expressions;
using entities;
using entities.Helpers;
using lib.Cryptography;
using repositorio.Helpers;
using repository.Context;
using repository.Extensions;
using repository.Interface;


namespace repository.Services
{
    public class ParameterInternalService : IParameterInternalService
    {
        private TurningPointsContext _context;
        List<string> _encryptedParameters = new List<string> { "AdminUser", "AdminPassword", "EmailUser", "EmailPassword" };

        public ParameterInternalService(TurningPointsContext context)
        {
            _context = context;
        }

        public ParameterInternal? Get(Guid id)
        {
            var entity = _context.ParameterInternal.FirstOrDefault(x => x.Id == id);
            if (entity == null || string.IsNullOrEmpty(entity.Name))
                return null;

            if (_encryptedParameters.FirstOrDefault(x => x.Contains(entity.Name)) != null)
            {
                if (!string.IsNullOrEmpty(entity.Value) && entity.RecordDate.HasValue)
                {
                    var date = entity.RecordDate?.ToString("dd/MM/yyyy HH:mm:ss");
                    if (!string.IsNullOrEmpty(date))
                        entity.Value = Cipher.Decrypt(entity.Value, date);
                }
            }

            return entity;
        }

        public ParameterInternal? Get(string name)
        {
            var entity = _context.ParameterInternal.FirstOrDefault(x => x.Name == name);
            if (entity == null || string.IsNullOrEmpty(entity.Name))
                return null;

            if (_encryptedParameters.FirstOrDefault(x => x.Contains(entity.Name)) != null)
            {
                if (!string.IsNullOrEmpty(entity.Value) && entity.RecordDate != null)
                {
                    var date = entity.RecordDate?.ToString("dd/MM/yyyy HH:mm:ss");
                    if (!string.IsNullOrEmpty(date))
                        entity.Value = Cipher.Decrypt(entity.Value, date);
                }
            }

            return entity;
        }

        public int GetItemsPerPage()
        {
            var itemsPerPage = Get("ItemsPerPage");
            if (itemsPerPage != null)
                return Convert.ToInt32(itemsPerPage.Value);

            return 100;
        }

        public ParameterInternal Add(ParameterInternal entity)
        {
            _context.ParameterInternal.Add(entity);
            return entity;
        }

        public ParameterInternal Update(ParameterInternal entity)
        {
            if (string.IsNullOrEmpty(entity.Name))
                throw new Exception("The parameter name is required.");

            if (_encryptedParameters.FirstOrDefault(x => x.Contains(entity.Name)) != null)
            {
                if (!string.IsNullOrEmpty(entity.Value) && entity.RecordDate.HasValue)
                {
                    var date = entity.RecordDate?.ToString("dd/MM/yyyy HH:mm:ss");
                    if (!string.IsNullOrEmpty(date))
                        entity.Value = Cipher.Encrypt(entity.Value, date);
                }
            }

            _context.ParameterInternal.Update(entity);
            return entity;
        }

        public PaginationList<ParameterInternal> GetAll(SearchFilter filter)
        {
            List<Expression<Func<ParameterInternal, bool>>> searchFilters = FilterConfig.GetParameterInternalFilter(filter.Entity);
            int page = filter.PageIndex == 0 ? 1 : filter.PageIndex;

            if (string.IsNullOrEmpty(filter.SortField))
            {
                filter.SortField = "Name";
            }

            IQueryable<ParameterInternal> query = _context.ParameterInternal.AsQueryable();
            if (searchFilters != null)
            {
                foreach (var filterValue in searchFilters)
                {
                    query = query.Where(filterValue);
                }
            }

            foreach (var value in query.ToList())
            {
                if (!string.IsNullOrEmpty(value.Name))
                    if (_encryptedParameters.FirstOrDefault(x => x.Contains(value.Name)) != null)
                    {
                        value.Value = "Encrypted data";
                    }
            }

            return PaginationList<ParameterInternal>.Create(query.OrderByDynamic(filter.SortField, filter.SortType), page, filter.PageSize);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
