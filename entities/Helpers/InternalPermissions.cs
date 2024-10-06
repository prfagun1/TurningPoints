namespace entities.Helpers
{
    public class InternalPermissions
    {
        public InternalPermissions()
        {
            Permission = "";
            AllowsWriteAccess = false;
            AllowsReadAccess = false;
        }

        public InternalPermissions(bool allowsWriteAccess, bool allowsReadAccess, string permission)
        {
            Permission = permission;
            AllowsWriteAccess = allowsWriteAccess;
            AllowsReadAccess = allowsReadAccess;
        }

        public bool AllowsWriteAccess { get; set; }
        public bool AllowsReadAccess { get; set; }
        public string Permission { get; set; }
    }
}
