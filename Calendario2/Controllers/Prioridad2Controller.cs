using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Calendario2.Models;

namespace Calendario2.Controllers
{
    public class Prioridad2Controller : Controller
    {
        private Model1 db = new Model1();

        // GET: Prioridad2
        public ActionResult Index()
        {
            return View(db.Prioridads.ToList());
        }

        // GET: Prioridad2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prioridad prioridad = db.Prioridads.Find(id);
            if (prioridad == null)
            {
                return HttpNotFound();
            }
            return View(prioridad);
        }

        // GET: Prioridad2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Prioridad2/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,Nombre,Orden")] Prioridad prioridad)
        {
            if (ModelState.IsValid)
            {
                db.Prioridads.Add(prioridad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(prioridad);
        }

        // GET: Prioridad2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prioridad prioridad = db.Prioridads.Find(id);
            if (prioridad == null)
            {
                return HttpNotFound();
            }
            return View(prioridad);
        }

        // POST: Prioridad2/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Nombre,Orden")] Prioridad prioridad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prioridad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prioridad);
        }

        // GET: Prioridad2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prioridad prioridad = db.Prioridads.Find(id);
            if (prioridad == null)
            {
                return HttpNotFound();
            }
            return View(prioridad);
        }

        // POST: Prioridad2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prioridad prioridad = db.Prioridads.Find(id);
            db.Prioridads.Remove(prioridad);
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
