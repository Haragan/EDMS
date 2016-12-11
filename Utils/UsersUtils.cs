using EDMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
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

        public List<UserData> GetAllClients() {
            UsersContext uCtx = new UsersContext();
            List<UserData> clients = new List<UserData>();
            int currentUserId = WebSecurity.CurrentUserId;
            uCtx.UserProfiles.ToList()
                .ForEach(p => {
                    if (p.ID != currentUserId &&
                        Roles.IsUserInRole(p.LOGIN, UserRole.CLIENT)) {
                        clients.Add(db.UsersData.Where(c => c.ProfileID == p.ID).Single());
                    }
                });
            uCtx.Dispose();
            return clients;
        }

        public List<UserData> GetAvailableModeratorsForDocument(Document document) {
            UsersContext uCtx = new UsersContext();
            List<UserData> moderators = new List<UserData>();
            uCtx.UserProfiles.ToList()
                .ForEach(p => {
                    if (Roles.IsUserInRole(p.LOGIN, UserRole.MODERATOR)) {
                        UserData moderator = db.UsersData.Single(c => c.ProfileID == p.ID);
                        if (moderator.Organization.Equals(document.Organization)) {
                            moderators.Add(moderator);
                        }
                    }
                });
            uCtx.Dispose();
            return moderators;
        }

        public List<UserData> GetAvailableClientsForDocument(Document document) {
            List<UserData> allClients = GetAllClients();
            allClients.Remove(GetCurrentUser());
            List<ClientDocument> documentClients = document.ClientDocuments.ToList();
            documentClients.ForEach(cd => {
                allClients.Remove(cd.Client);
            });
            return allClients;
        }
    }
}