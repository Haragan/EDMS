using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDMS.Models {
    public class UserRole {
        private String code;
        private String name;

        public String Name { get { return name; } }
        public String Code { get { return code; } }

        public static readonly UserRole ADMINISTRATOR = new UserRole("ADMINISTRATOR", "Администратор");
        public static readonly UserRole MODERATOR = new UserRole("MODERATOR", "Модератор");
        public static readonly UserRole CLIENT = new UserRole("CLIENT", "Клиент");

        private UserRole(String code, String name) {
            this.code = code;
            this.name = name;
        }
    }
}