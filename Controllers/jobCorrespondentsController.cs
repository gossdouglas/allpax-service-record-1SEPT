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
    public class jobCorrespondentsController : Controller
    {
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

        // GET: jobCorrespondents
        public ActionResult Index()
        {
            var tbl_jobCorrespondents = db.tbl_jobCorrespondents.Include(t => t.tbl_Jobs);
            return View(tbl_jobCorrespondents.ToList());
        }

        // GET: jobCorrespondents/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_jobCorrespondents tbl_jobCorrespondents = db.tbl_jobCorrespondents.Find(id);
            if (tbl_jobCorrespondents == null)
            {
                return HttpNotFound();
            }
            return View(tbl_jobCorrespondents);
        }

        // GET: jobCorrespondents/Create
        public ActionResult Create()
        {
            ViewBag.jobID = new SelectList(db.tbl_Jobs, "jobID", "description");
            return View();
        }

        // POST: jobCorrespondents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "jobCorrespondentID,jobID,name,email,active")] tbl_jobCorrespondents tbl_jobCorrespondents)
        {
            if (ModelState.IsValid)
            {
                db.tbl_jobCorrespondents.Add(tbl_jobCorrespondents);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.jobID = new SelectList(db.tbl_Jobs, "jobID", "description", tbl_jobCorrespondents.jobID);
            return View(tbl_jobCorrespondents);
        }

        // GET: jobCorrespondents/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_jobCorrespondents tbl_jobCorrespondents = db.tbl_jobCorrespondents.Find(id);
            if (tbl_jobCorrespondents == null)
            {
                return HttpNotFound();
            }
            ViewBag.jobID = new SelectList(db.tbl_Jobs, "jobID", "description", tbl_jobCorrespondents.jobID);
            return View(tbl_jobCorrespondents);
        }

        // POST: jobCorrespondents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "jobCorrespondentID,jobID,name,email,active")] tbl_jobCorrespondents tbl_jobCorrespondents)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_jobCorrespondents).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.jobID = new SelectList(db.tbl_Jobs, "jobID", "description", tbl_jobCorrespondents.jobID);
            return View(tbl_jobCorrespondents);
        }

        // GET: jobCorrespondents/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_jobCorrespondents tbl_jobCorrespondents = db.tbl_jobCorrespondents.Find(id);
            if (tbl_jobCorrespondents == null)
            {
                return HttpNotFound();
            }
            return View(tbl_jobCorrespondents);
        }

        // POST: jobCorrespondents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            tbl_jobCorrespondents tbl_jobCorrespondents = db.tbl_jobCorrespondents.Find(id);
            db.tbl_jobCorrespondents.Remove(tbl_jobCorrespondents);
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
