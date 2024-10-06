using entities;
using entities.Helpers;
using Microsoft.EntityFrameworkCore;
using repositorio.Helpers;
using repository.Context;
using repository.Extensions;
using repository.Interface;
using System.Linq.Expressions;


namespace repository.Services
{
    public class ChapterService : IChapterService
    {
        private TurningPointsContext _context;

        public ChapterService(TurningPointsContext context)
        {
            _context = context;
        }

        public Chapter? Get(Guid id)
        {
            var entity = _context.Chapter.FirstOrDefault(x => x.Id == id);

            return entity;
        }

        public Chapter Add(Chapter entity)
        {
            _context.Chapter.Add(entity);
            return entity;
        }

        public Chapter Update(Chapter entity)
        {
            _context.Chapter.Update(entity);
            return entity;
        }

        public PaginationList<Chapter> GetAll(SearchFilter filter)
        {
            List<Expression<Func<Chapter, bool>>> searchFilters = FilterConfig.GetChapterFilter(filter.Entity);
            int page = filter.PageIndex == 0 ? 1 : filter.PageIndex;

            if (string.IsNullOrEmpty(filter.SortField))
            {
                filter.SortField = "StoryId";
            }

            IQueryable<Chapter> query = _context.Chapter.AsQueryable().Include(x => x.Story);

            if (searchFilters != null)
            {
                foreach (var filterValue in searchFilters)
                {
                    query = query.Where(filterValue);
                }
            }

            foreach (var item in query)
            {
                item.Story.Chapter = null;
            }

            return PaginationList<Chapter>.Create(query.OrderByDynamic(filter.SortField, filter.SortType), page, filter.PageSize);
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

        public void Delete(Chapter entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Remove(entity);
        }

        public List<Chapter> GetStoryChapter(Guid storyId)
        {
            var chapterList = _context.Chapter.Where(x => x.StoryId.ToString() == storyId.ToString());
            return chapterList.ToList();
        }
    }
}
