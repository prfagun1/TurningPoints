using System.Linq.Expressions;
using entities.Helpers;
using entities;
using repositorio.Helpers;
using repository.Context;
using repository.Extensions;
using repository.Interface;

namespace repository.Services
{
    public class LogService : ILogService
    {
        private TurningPointsContext _context;

        public LogService(TurningPointsContext context)
        {
            _context = context;
        }

        public PaginationList<Log> GetAll(SearchFilter filter)
        {
            List<Expression<Func<Log, bool>>> searchFilter = FilterConfig.GetLogFilter(filter.Entity);
            int page = filter.PageIndex == 0 ? 1 : filter.PageIndex;

            if (string.IsNullOrEmpty(filter.SortField))
            {
                filter.SortField = "LoggedDate";
                filter.SortType = entities.Enumerators.SortOrder.Descending;
            }

            IQueryable<Log> query = _context.Log.AsQueryable();
            if (searchFilter != null)
            {
                foreach (var filterValue in searchFilter)
                {
                    query = query.Where(filterValue);
                }
            }

            return PaginationList<Log>.Create(query.OrderByDynamic(filter.SortField, filter.SortType), page, filter.PageSize);
        }

    }

}
