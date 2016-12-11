using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EDMS.Models {
    public class UserRole {
        public const String ADMINISTRATOR = "ADMINISTRATOR";
        public const String MODERATOR = "MODERATOR";
        public const String CLIENT = "CLIENT";

        private static SelectList allRoles;

        private UserRole() { }

        public static SelectList SelectList() {
            if (allRoles == null) {

                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem() { Value = ADMINISTRATOR, Text = "Администратор" });
                items.Add(new SelectListItem() { Value = MODERATOR, Text = "Модератор" });
                items.Add(new SelectListItem() { Value = CLIENT, Text = "Клиент", Selected = true });
                allRoles = new SelectList(items, "Value", "Text");
            }
            return allRoles;
        }
    }
}