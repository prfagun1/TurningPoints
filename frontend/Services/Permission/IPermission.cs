namespace frontend.Services.Permission
{
    public interface IPermission
    {
        public string ReadPermission { get; }
        public string WritePermission { get; }
        public string WriteClassDiv { get; }
        public bool AllowsReadAccess { get; }
        public bool AllowsWriteAccess { get; }
    }
}
