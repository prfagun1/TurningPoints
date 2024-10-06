namespace repository.Interface
{
    public interface IUnitOfWork
    {
        public IParameterInternalService ParameterInternal { get; }
        public ILogService Log { get; }
        public ILoginService Login { get; }


        public IStoryService Story { get; }
        public IChapterService Chapter { get; }
    }
}
