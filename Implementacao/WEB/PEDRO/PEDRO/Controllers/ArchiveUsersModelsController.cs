using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PEDRO.Models;

namespace PEDRO.Controllers
{
    public class ArchiveUsersModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ArchiveUsersModels
        public ActionResult Index()
        {
            return View(db.ArchiveUsersModels.ToList());
        }

        // GET: ArchiveUsersModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArchiveUsersModels archiveUsersModels = db.ArchiveUsersModels.Find(id);
            if (archiveUsersModels == null)
            {
                return HttpNotFound();
            }
            return View(archiveUsersModels);
        }

        // GET: ArchiveUsersModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArchiveUsersModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id")] ArchiveUsersModels archiveUsersModels)
        {
            if (ModelState.IsValid)
            {
                db.ArchiveUsersModels.Add(archiveUsersModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(archiveUsersModels);
        }

        // GET: ArchiveUsersModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArchiveUsersModels archiveUsersModels = db.ArchiveUsersModels.Find(id);
            if (archiveUsersModels == null)
            {
                return HttpNotFound();
            }
            return View(archiveUsersModels);
        }

        // POST: ArchiveUsersModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id")] ArchiveUsersModels archiveUsersModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(archiveUsersModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(archiveUsersModels);
        }

        // GET: ArchiveUsersModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArchiveUsersModels archiveUsersModels = db.ArchiveUsersModels.Find(id);
            if (archiveUsersModels == null)
            {
                return HttpNotFound();
            }
            return View(archiveUsersModels);
        }

        // POST: ArchiveUsersModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArchiveUsersModels archiveUsersModels = db.ArchiveUsersModels.Find(id);
            db.ArchiveUsersModels.Remove(archiveUsersModels);
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
