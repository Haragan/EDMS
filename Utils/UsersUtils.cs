using EDMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace EDMS.Utils {
    public class UsersUtils {

        private Entities db;

        public UsersUtils(Entities db) {
            this.db = db;
        }

        public UserData GetCurrentUser() {
            int currentUserId = WebSecurity.CurrentUserId;
            if (currentUserId == -1) return null;
            return db.UsersData.Where(u => u.ProfileID == currentUserId).Single();
        }
    }
}