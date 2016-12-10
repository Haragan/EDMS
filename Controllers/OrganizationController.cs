using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EDMS.Models;

namespace EDMS.Controllers {
    public class OrganizationController : Controller {
        private Entities db = new Entities();

        public ActionResult Index() {
            return View(db.Organizations.ToList());
        }

        public ActionResult Details(long id = 0) {
            Organization organization = db.Organizations.Find(id);
            if (organization == null) {
                return HttpNotFound();
            }
            return View(organization);
        }

        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Organization organization) {
            if (ModelState.IsValid) {
                db.Organizations.Add(organization);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(organization);
        }

        public ActionResult Edit(long id = 0) {
            Organization organization = db.Organizations.Find(id);
            if (organization == null) {
                return HttpNotFound();
            }
            return View(organization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Organization organization) {
            if (ModelState.IsValid) {
                db.Entry(organization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(organization);
        }

        public ActionResult Delete(long id = 0) {
            Organization organization = db.Organizations.Find(id);
            if (organization == null) {
                return HttpNotFound();
            }
            return View(organization);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id) {
            Organization organization = db.Organizations.Find(id);
            db.Organizations.Remove(organization);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}