using EDMS.Models;
using EDMS.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EDMS.Controllers {
    [Authorize(Roles = UserRole.MODERATOR)]
    public class ModeratorController : Controller {
        private Entities db = new Entities();
        private UsersUtils usersUtils;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext) {
            base.Initialize(requestContext);
            usersUtils = new UsersUtils(db);
        }

        public ActionResult Index() {
            UserData currentUser = usersUtils.GetCurrentUser();
            List<Document> documents = db.Documents.Where(d => d.Moderator.ID == currentUser.ID).ToList();
            return View(documents);
        }

        public ActionResult Details(long id) {
            Document document = db.Documents.Find(id);
            if (document == null) {
                return HttpNotFound();
            }
            return View(document);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(FormCollection form) {
            long documentId = long.Parse(form["document_id"]);
            Document document = db.Documents.Find(documentId);
            document.Status = DocumentSatus.CONFIRMED;
            document.ModeratorID = null;
            db.Entry(document).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reject(FormCollection form) {
            long documentId = long.Parse(form["document_id"]);
            Document document = db.Documents.Find(documentId);
            document.Status = DocumentSatus.REJECTED;
            document.ModeratorID = null;
            db.Entry(document).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
