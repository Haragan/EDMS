using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDMS.Models {
    public class UserRole {
        public const String ADMINISTRATOR = "ADMINISTRATOR";
        public const String MODERATOR = "MODERATOR";
        public const String CLIENT = "CLIENT";

        private UserRole() { }
    }
}