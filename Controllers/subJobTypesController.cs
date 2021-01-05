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
    public class subJobTypesController : Controller
    {
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

        // GET: subJobTypes
        public ActionResult Index()
        {
            return View(db.tbl_subJobTypes.ToList());
        }

        // GET: subJobTypes/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_subJobTypes tbl_subJobTypes = db.tbl_subJobTypes.Find(id);
            if (tbl_subJobTypes == null)
            {
                return HttpNotFound();
            }
            return View(tbl_subJobTypes);
        }

        // GET: subJobTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: subJobTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "subJobID,description")] tbl_subJobTypes tbl_subJobTypes)
        {
            if (ModelState.IsValid)
            {
                db.tbl_subJobTypes.Add(tbl_subJobTypes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_subJobTypes);
        }

        // GET: subJobTypes/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_subJobTypes tbl_subJobTypes = db.tbl_subJobTypes.Find(id);
            if (tbl_subJobTypes == null)
            {
                return HttpNotFound();
            }
            return View(tbl_subJobTypes);
        }

        // POST: subJobTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "subJobID,description")] tbl_subJobTypes tbl_subJobTypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_subJobTypes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_subJobTypes);
        }

        // GET: subJobTypes/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_subJobTypes tbl_subJobTypes = db.tbl_subJobTypes.Find(id);
            if (tbl_subJobTypes == null)
            {
                return HttpNotFound();
            }
            return View(tbl_subJobTypes);
        }

        // POST: subJobTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            tbl_subJobTypes tbl_subJobTypes = db.tbl_subJobTypes.Find(id);
            db.tbl_subJobTypes.Remove(tbl_subJobTypes);
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
