using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EDMS.Models;
using WebMatrix.WebData;
using System.Web.Security;

namespace EDMS.Controllers {
    [Authorize(Roles = UserRole.ADMINISTRATOR)]
    public class AdministratorController : Controller {
        private Entities db = new Entities();
        private UsersContext usersDb = new UsersContext();

        public ActionResult Index() {
            return View();
        }

        public ActionResult OrganizationList() {
            return View(db.Organizations.ToList());
        }

        public ActionResult DetailsOrganization(long id = 0) {
            Organization organization = db.Organizations.Find(id);
            if (organization == null) {
                return HttpNotFound();
            }
            return View(organization);
        }

        public ActionResult CreateOrganization() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrganization(Organization organization) {
            if (ModelState.IsValid) {
                db.Organizations.Add(organization);
                db.SaveChanges();
                return RedirectToAction("OrganizationList");
            }

            return View(organization);
        }

        public ActionResult EditOrganization(long id = 0) {
            Organization organization = db.Organizations.Find(id);
            if (organization == null) {
                return HttpNotFound();
            }
            return View(organization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrganization(Organization organization) {
            if (ModelState.IsValid) {
                db.Entry(organization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("OrganizationList");
            }
            return View(organization);
        }

        public ActionResult DeleteOrganization(long id = 0) {
            Organization organization = db.Organizations.Find(id);
            if (organization == null) {
                return HttpNotFound();
            }
            return View(organization);
        }

        [HttpPost, ActionName("DeleteOrganization")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOrganizationConfirmed(long id) {
            Organization organization = db.Organizations.Find(id);
            db.Organizations.Remove(organization);
            db.SaveChanges();
            return RedirectToAction("OrganizationList");
        }

        public ActionResult UserList() {
            return View(db.UsersData.ToList());
        }

        public ActionResult CreateUser() {
            ViewBag.orgList = new SelectList(db.Organizations.ToList(), "ID", "Name", 1);
            ViewBag.roles = UserRole.SelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(RegisterModel user) {
            if (ModelState.IsValid) {
                WebSecurity.CreateUserAndAccount(user.UserName, user.Password);
                Roles.AddUserToRole(user.UserName, user.Role);

                UserData userData = new UserData();
                userData.ProfileID = WebSecurity.GetUserId(user.UserName);
                userData.FIO = user.FIO;
                userData.Email = user.Email;
                userData.PhoneNumber = user.PhoneNumber;
                userData.OrganizationID = user.OrganizationID;
                db.UsersData.Add(userData);
                db.SaveChanges();

                return RedirectToAction("UserList");
            }

            ViewBag.orgList = new SelectList(db.Organizations.ToList(), "ID", "Name", 1);
            ViewBag.roles = UserRole.SelectList();
            return View(user);
        }

        public ActionResult ChangeRole(long id) {
            UserData user = db.UsersData.Find(id);
            if (user == null) {
                return HttpNotFound();
            }
            ViewBag.roles = UserRole.SelectList(); ;
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeRole(FormCollection form) {
            long userID = long.Parse(form["user_id"]);
            UserData user = db.UsersData.Find(userID);
            UserProfile profile = usersDb.UserProfiles.Single(p => p.ID == user.ProfileID);
            String newRole = form["new_role"];
            String currentRole = Roles.GetRolesForUser(profile.LOGIN)[0];
            Roles.RemoveUserFromRole(profile.LOGIN, currentRole);
            Roles.AddUserToRole(profile.LOGIN, newRole);
            return RedirectToAction("UserList");
        }

        public ActionResult DeleteUser(long id) {
            UserData user = db.UsersData.Find(id);
            if (user == null) {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserConfirmed(long id) {
            UserData user = db.UsersData.Find(id);
            UserProfile profile = usersDb.UserProfiles.Single(p => p.ID == user.ProfileID);
            db.UsersData.Remove(user);
            db.SaveChanges();

            Roles.RemoveUserFromRoles(profile.LOGIN, Roles.GetRolesForUser(profile.LOGIN));
            Membership.DeleteUser(profile.LOGIN, true);
            return RedirectToAction("UserList");
        }

        protected override void Dispose(bool disposing) {
            db.Dispose();
            usersDb.Dispose();
            base.Dispose(disposing);
        }
    }
}