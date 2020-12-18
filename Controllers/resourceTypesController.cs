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
    public class resourceTypesController : Controller
    {
        private allpaxServiceRecordEntities db = new allpaxServiceRecordEntities();

        // GET: resourceTypes
        public ActionResult Index()
        {
            return View(db.tbl_resourceTypes.ToList());
        }

        // GET: resourceTypes/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_resourceTypes tbl_resourceTypes = db.tbl_resourceTypes.Find(id);
            if (tbl_resourceTypes == null)
            {
                return HttpNotFound();
            }
            return View(tbl_resourceTypes);
        }

        // GET: resourceTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: resourceTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "resourceTypeID,resourceType,description,rate")] tbl_resourceTypes tbl_resourceTypes)
        {
            if (ModelState.IsValid)
            {
                db.tbl_resourceTypes.Add(tbl_resourceTypes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_resourceTypes);
        }

        // GET: resourceTypes/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_resourceTypes tbl_resourceTypes = db.tbl_resourceTypes.Find(id);
            if (tbl_resourceTypes == null)
            {
                return HttpNotFound();
            }
            return View(tbl_resourceTypes);
        }

        // POST: resourceTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "resourceTypeID,resourceType,description,rate")] tbl_resourceTypes tbl_resourceTypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_resourceTypes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_resourceTypes);
        }

        // GET: resourceTypes/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_resourceTypes tbl_resourceTypes = db.tbl_resourceTypes.Find(id);
            if (tbl_resourceTypes == null)
            {
                return HttpNotFound();
            }
            return View(tbl_resourceTypes);
        }

        // POST: resourceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            tbl_resourceTypes tbl_resourceTypes = db.tbl_resourceTypes.Find(id);
            db.tbl_resourceTypes.Remove(tbl_resourceTypes);
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
