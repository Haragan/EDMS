using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EDMS.Models;
using WebMatrix.WebData;
using EDMS.Utils;
using EDMS.Filters;

namespace EDMS.Controllers {
    [Authorize(Roles = "CLIENT")]
    public class ClientDocumentController : Controller {
        private Entities db = new Entities();
        private DocumentUtils documentUtils;
        private UsersUtils usersUtils; 

        protected override void Initialize(System.Web.Routing.RequestContext requestContext) {
            base.Initialize(requestContext);
            this.documentUtils = new DocumentUtils(db);
            this.usersUtils = new UsersUtils(db);
        }

        public ActionResult Index() {
            return View(documentUtils.GetCurrentClientDocuments());
        }

        public ActionResult Details(long id = 0) {
            Document document = db.Documents.Find(id);
            if (document == null) {
                return HttpNotFound();
            }
            if (!documentUtils.IsCurrentClientDocument(document)) {
                TempData["message"] = "Вам не доступен этот документ";
                return RedirectToAction("ActionDeny");
            }
            return View(document);
        }

        public ActionResult Create() {
            Document document = new Document();
            UserData currentUser = db.UsersData.Where(u => u.ProfileID == WebSecurity.CurrentUserId).Single();
            document.CreateDate = DateTime.Now;
            document.CreatorID = currentUser.ID;
            document.OrganizationID = currentUser.Organization.ID;
            document.Status = DocumentSatus.INIT;
            return View(document);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Document document) {
            if (ModelState.IsValid) {
                document.Status = DocumentSatus.CREATED;
                db.Documents.Add(document);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(document);
        }

        public ActionResult Edit(long id = 0) {
            Document document = db.Documents.Find(id);
            if (document == null) {
                return HttpNotFound();
            }
            if (!documentUtils.IsCurrentClientDocument(document)) {
                TempData["mesage"] = "Вам не доступен этот документ";
                return RedirectToAction("ActionDeny");
            }
            if (!documentUtils.IsMayEdit(document)) {
                TempData["message"] = "Вы не можете редактировать данный документ, документ должен находться в соотвествующем состоянии";
                return RedirectToAction("ActionDeny");
            }
            document.Status = DocumentSatus.EDITING;
            db.Entry(document).State = EntityState.Modified;
            db.SaveChanges();
            return View(document);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Document document) {
            if (ModelState.IsValid) {
                document.Status = DocumentSatus.CREATED;
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(document);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelEdit(Document document) {
            document.Status = DocumentSatus.CREATED;
            db.Entry(document).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(long id = 0) {
            Document document = db.Documents.Find(id);
            if (document == null) {
                return HttpNotFound();
            }
            if (!documentUtils.IsCurrentClientDocument(document)) {
                TempData["message"] = "Вам не доступен этот документ";
                return RedirectToAction("ActionDeny");
            }
            if (!documentUtils.IsMayDelete(document)) {
                TempData["message"] = "Вы не можете удалить данный документ, он находится не в том состоянии";
                return RedirectToAction("ActionDeny");
            }
            return View(document);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id) {
            Document document = db.Documents.Find(id);
            db.Documents.Remove(document);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SendToModerator(long id) {
            Document document = db.Documents.Find(id);
            if (document == null) {
                return HttpNotFound();
            }
            if (!documentUtils.IsCurrentClientDocument(document)) {
                TempData["message"] = "Вам не доступен этот документ";
                return RedirectToAction("ActionDeny");
            }
            if (!documentUtils.IsMaySendToModerator(document)) {
                TempData["message"] = "Вы не можете отправить документ модератору, он должен находиться в состоянии: " + DocumentSatus.CREATED;
                return RedirectToAction("ActionDeny");
            }
            ViewBag.moderators = new SelectList(usersUtils.GetAllModerators(), "ID", "FIO", 1);
            return View(document);
        }

        [HttpPost, ActionName("SendToModerator")]
        [ValidateAntiForgeryToken]
        public ActionResult SendToModeratorConfirm(FormCollection form) {
            long documentId = long.Parse(form["document_id"]);
            Document document = db.Documents.Single(d => d.ID == documentId);
            document.Status = DocumentSatus.SEND_TO_MODERATOR;
            long moderatorId = long.Parse(form["moderator_id"]);
            ModeratorDocument md = new ModeratorDocument();
            md.Document = document;
            md.Moderator = db.UsersData.Where(u => u.ID == moderatorId).Single();
            document.Moderators.Add(md);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ActionDeny(string message) {
            return View(TempData["message"]);
        }

        public ActionResult Workflow() {
            return View();
        }

        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}