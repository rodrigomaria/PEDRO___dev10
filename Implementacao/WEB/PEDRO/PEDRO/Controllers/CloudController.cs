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
    [Authorize]
    public class CloudController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cloud
        public ActionResult Index()
        {
            string id = User.Identity.GetUserId();
            return View(db.CloudModels.Where(cm => cm.user.Id == id).ToList());
        }

        // GET: Cloud/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CloudModel cloudModel = db.CloudModels.Find(id);
            if (cloudModel == null)
            {
                return HttpNotFound();
            }
            return View(cloudModel);
        }

        // GET: Cloud/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cloud/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nome,email,pass,confirmPass")] CloudModel cloudModel)
        {
            if (ModelState.IsValid)
            {
                cloudModel.user = db.Users.Find(User.Identity.GetUserId());
                db.CloudModels.Add(cloudModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cloudModel);
        }

        // GET: Cloud/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CloudModel cloudModel = db.CloudModels.Find(id);
            if (cloudModel == null)
            {
                return HttpNotFound();
            }
            return View(cloudModel);
        }

        // POST: Cloud/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nome,email,pass,confirmPass")] CloudModel cloudModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cloudModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cloudModel);
        }

        // GET: Cloud/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CloudModel cloudModel = db.CloudModels.Find(id);
            if (cloudModel == null)
            {
                return HttpNotFound();
            }
            return View(cloudModel);
        }

        // POST: Cloud/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CloudModel cloudModel = db.CloudModels.Find(id);
            db.CloudModels.Remove(cloudModel);
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
