using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDMS.Models {
    public class UserRole {
        public const String ADMINISTRATOR = "ADMINISTRATOR";
        public const String MODERATOR = "MODERATOR";
        public const String CLIENT = "CLIENT";

        private static List<String> allRoles;

        private UserRole() { }

        public static List<String> List() {
            if (allRoles == null) {
                allRoles = new List<String> { ADMINISTRATOR, MODERATOR, CLIENT };
            }
            return allRoles;
        }
    }
}