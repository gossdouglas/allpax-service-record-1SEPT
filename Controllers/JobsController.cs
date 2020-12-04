using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using allpax_service_record.Models;

namespace allpax_service_record.Controllers
{
    public class JobsController : Controller
    {
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

        // GET: Jobs
        public ActionResult Index()
        {
            return View(db.tbl_Jobs.ToList());
        }

        // GET: Jobs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Jobs tbl_Jobs = db.tbl_Jobs.Find(id);
            if (tbl_Jobs == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Jobs);
        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "jobID,description,customerCode,customerContact,active,location,id")] tbl_Jobs tbl_Jobs)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Jobs.Add(tbl_Jobs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Jobs);
        }

        // GET: Jobs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Jobs tbl_Jobs = db.tbl_Jobs.Find(id);
            if (tbl_Jobs == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Jobs);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "jobID,description,customerCode,customerContact,active,location,id")] tbl_Jobs tbl_Jobs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Jobs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Jobs);
        }

        // GET: Jobs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Jobs tbl_Jobs = db.tbl_Jobs.Find(id);
            if (tbl_Jobs == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Jobs);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            tbl_Jobs tbl_Jobs = db.tbl_Jobs.Find(id);
            db.tbl_Jobs.Remove(tbl_Jobs);
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
