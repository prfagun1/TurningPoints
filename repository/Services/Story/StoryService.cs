using entities;
using entities.Helpers;
using Microsoft.EntityFrameworkCore;
using repositorio.Helpers;
using repository.Context;
using repository.Extensions;
using repository.Interface;
using System.Linq;
using System.Linq.Expressions;


namespace repository.Services
{
    public class StoryService : IStoryService
    {
        private TurningPointsContext _context;

        public StoryService(TurningPointsContext context)
        {
            _context = context;
        }

        public Story? Get(Guid id)
        {
            var entity = _context.Story.FirstOrDefault(x => x.Id == id);
            if(entity != null)
                entity.Chapter = null;

            return entity;
        }

        public Story Add(Story entity)
        {
            _context.Story.Add(entity);
            return entity;
        }

        public Story Update(Story entity)
        {

            //var entityTemp = _context.Story.AsNoTracking().FirstOrDefault(x => x.Id == entity.Id);

            _context.Story.Update(entity);
            return entity;
        }

        public PaginationList<Story> GetAll(SearchFilter filter)
        {
            List<Expression<Func<Story, bool>>> searchFilters = FilterConfig.GetStoryFilter(filter.Entity);
            int page = filter.PageIndex == 0 ? 1 : filter.PageIndex;

            if (string.IsNullOrEmpty(filter.SortField))
            {
                filter.SortField = "Title";
            }

            IQueryable<Story> query = _context.Story.AsQueryable();
            if (searchFilters != null)
            {
                foreach (var filterValue in searchFilters)
                {
                    query = query.Where(filterValue);
                }
            }

            return PaginationList<Story>.Create(query.OrderByDynamic(filter.SortField, filter.SortType), page, filter.PageSize);
        }

        public PaginationList<Story> GetAllOpen(SearchFilter filter)
        {

            //List<Expression<Func<Story, bool>>> searchFilters = FilterConfig.GetStoryFilterOpen(filter.Entity);
            int page = filter.PageIndex == 0 ? 1 : filter.PageIndex;

            if (string.IsNullOrEmpty(filter.SortField))
            {
                filter.SortField = "Title";
            }

            IQueryable<Story> query = _context.Story.AsQueryable();
            /*
            if (searchFilters != null)
            {
                foreach (var filterValue in searchFilters)
                {
                    query = query.Where(filterValue);
                }
            }*/

            if(filter.Entity != null)
            {
                filter.Entity.TryGetValue("Description", out object filterObject);
                if (filterObject != null)
                {
                    query = query.Where(x => 
                        x.Descricao.Contains(filterObject.ToString()) ||
                        x.Tags.Contains(filterObject.ToString()) ||
                        x.Description.Contains(filterObject.ToString()) ||
                        x.Titulo.Contains(filterObject.ToString()) ||
                        x.Title.Contains(filterObject.ToString())
                    );
                }
              
            }




            return PaginationList<Story>.Create(query.OrderByDynamic(filter.SortField, filter.SortType), page, filter.PageSize);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var entity = Get(id);
            ArgumentNullException.ThrowIfNull(entity);
            Delete(entity);
        }

        public void Delete(Story entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Remove(entity);
        }

    }
}
