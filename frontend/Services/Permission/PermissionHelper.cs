using entities.Helpers;
using System.Security.Claims;

namespace frontend.Services.Permission
{
    public class PermissionHelper
    {
        public bool AllowsReadAccess { get; private set; }
        public bool AllowsWriteAccess { get; private set; }
        public string? WriteClassDiv { get; private set; }
        private string WritePermission { get; set; }
        private string ReadPermission { get; set; }
        private ClaimsPrincipal User { get; set; }

        public PermissionHelper(ClaimsPrincipal user, string writePermission, string readPermission)
        {
            WritePermission = writePermission;
            ReadPermission = readPermission;
            User = user;
            GetPermission();
        }

        private InternalPermissions CheckPermission()
        {
            InternalPermissions internalPermissions = new InternalPermissions(false, false, "");

            if (User == null)
                return internalPermissions;

            if (User.IsInRole("Administration"))
            {
                internalPermissions.AllowsWriteAccess = true;
                internalPermissions.AllowsReadAccess = true;
                return internalPermissions;
            }

            foreach (string permission in WritePermission.Split(','))
            {
                if (User.IsInRole(permission))
                {
                    internalPermissions.AllowsWriteAccess = true;
                    internalPermissions.AllowsReadAccess = true;
                    break;
                }
            }

            foreach (string permission in ReadPermission.Split(','))
            {
                if (User.IsInRole(permission))
                {
                    internalPermissions.AllowsReadAccess = true;
                    break;
                }
            }

            return internalPermissions;
        }

        private InternalPermissions GetPermission()
        {
            InternalPermissions internalPermissions = CheckPermission();

            if (internalPermissions.AllowsWriteAccess)
            {
                AllowsReadAccess = true;
                AllowsWriteAccess = true;
                WriteClassDiv = "";
            }
            else if (internalPermissions.AllowsReadAccess)
            {
                AllowsReadAccess = true;
                AllowsWriteAccess = false;
                WriteClassDiv = "hide-div";
            }
            else
            {
                WriteClassDiv = "hide-div";
                AllowsWriteAccess = false;
                AllowsReadAccess = false;
            }

            return internalPermissions;
        }
    }
}
