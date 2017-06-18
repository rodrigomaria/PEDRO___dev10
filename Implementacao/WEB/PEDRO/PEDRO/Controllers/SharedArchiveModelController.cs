using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PEDRO.Models;
using Microsoft.AspNet.Identity;

namespace PEDRO.Controllers
{
    public class SharedArchiveModelController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,usuario_id")] SharedArchiveModel sharedArchive)
        {
            if (ModelState.IsValid)
            {
                sharedArchive.proprietario_id = db.Users.Find(User.Identity.GetUserId()).Id;
                db.SharedArchiveModels.Add(sharedArchive);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sharedArchive);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SharedArchiveModel sAM = db.SharedArchiveModels.Find(id);
            if (sAM == null)
            {
                return HttpNotFound();
            }
            return View(sAM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SharedArchiveModel sAM = db.SharedArchiveModels.Find(id);
            db.SharedArchiveModels.Remove(sAM);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
