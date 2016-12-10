﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EDMS.Models;
using WebMatrix.WebData;
using EDMS.Utils;

namespace EDMS.Controllers {
    [Authorize(Roles = "CLIENT")]
    public class ClientDocumentController : Controller {
        private Entities db = new Entities();
        private DocumentUtils documentUtils;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext) {
            base.Initialize(requestContext);
            this.documentUtils = new DocumentUtils(db);
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