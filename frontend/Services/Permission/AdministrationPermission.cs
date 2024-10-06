using System.Security.Claims;

namespace frontend.Services.Permission
{
    public class AdministrationPermission : IPermission
    {
        private ClaimsPrincipal? _user;

        public string ReadPermission { get; private set; }
        public string WritePermission { get; private set; }
        public bool AllowsReadAccess { get; private set; }
        public bool AllowsWriteAccess { get; private set; }
        public string WriteClassDiv { get; private set; }

        public AdministrationPermission(ClaimsPrincipal? user = null)
        {
            _user = user;
            if (_user != null && _user.IsInRole("Admin"))
            {
                AllowsReadAccess = true;
                AllowsWriteAccess = true;
                WriteClassDiv = "";
            }
            else
            {
                AllowsReadAccess = false;
                WriteClassDiv = "hide-div";
            }

            ReadPermission = "Admin";
            WritePermission = "Admin";
        }
    }
}
