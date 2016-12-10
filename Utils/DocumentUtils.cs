using EDMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace EDMS.Utils {
    public class DocumentUtils {
        private UsersUtils userUtils;
        private Entities db;

        public DocumentUtils(Entities db) {
            this.db = db;
            this.userUtils = new UsersUtils(db);
        }

        public List<Document> GetCurrentClientDocuments() {
            return GetClientDocuments(WebSecurity.CurrentUserId);
        }

        public List<Document> GetClientDocuments(long clientID) {
            UserData currentUser = db.UsersData.Where(u => u.ProfileID == clientID).Single();
            List<Document> documents = new List<Document>();
            documents.AddRange(currentUser.Documents);
            documents.AddRange(currentUser.ClientDocuments.Select(cd => cd.Docuent));
            return documents;
        }

        public bool IsCurrentClientDocument(Document document) {
            List<Document> documents = GetCurrentClientDocuments();
            return documents.Contains(document);
        }

        public bool IsMayEdit(Document document) {
            string status = document.Status;
            return DocumentSatus.CREATED.Equals(status) || DocumentSatus.REJECTED.Equals(status);
        }

        public bool IsMayDelete(Document document) {
            return DocumentSatus.CONFIRMED.Equals(document.Status);
        }
    }
}