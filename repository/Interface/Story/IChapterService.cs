using entities;
using entities.Helpers;

namespace repository.Interface
{
    public interface IChapterService : IBaseService<Chapter>  {
        public List<Chapter> GetStoryChapter(Guid storyId);
    }
}
