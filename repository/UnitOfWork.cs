
using repository.Context;
using repository.Interface;
using repository.Services;

namespace repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private TurningPointsContext _context;
        private bool disposed = false;

        public UnitOfWork(TurningPointsContext context)
        {
            _context = context;
        }

        public IParameterInternalService ParameterInternal { get { return new ParameterInternalService(_context); } }
        public ILogService Log { get { return new LogService(_context); } }

        public ILoginService Login { get { return new LoginService(_context); } }


        public IStoryService Story { get { return new StoryService(_context); } }
        public IChapterService Chapter { get { return new ChapterService(_context); } }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
