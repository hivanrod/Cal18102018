using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Calendario2.Models;

namespace Calendario.Controllers
{
    public class Temas1Controller : Controller
    {
        private Model1 db = new Model1();

        // GET: Temas1
        public ActionResult Index()
        {
            var temas = db.Temas.Include(t => t.Contacto).Include(t => t.Prioridad).Include(t => t.Usuario);
            return View(temas.ToList());
        }

        // GET: Temas1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema tema = db.Temas.Find(id);
            if (tema == null)
            {
                return HttpNotFound();
            }
            return View(tema);
        }

        // GET: Temas1/Create
        public ActionResult Create()
        {
            ViewBag.IdContacto = new SelectList(db.Contactos, "Id", "Nombres");
            ViewBag.IdPrioridad = new SelectList(db.Prioridads, "Id", "Nombre");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Nombre");
            return View();
        }

        // POST: Temas1/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdPrioridad,Descripcion,FechaHora,Verificado,IdUsuario,VerificaFechaHora,Notas,Compras,Pagos,IdContacto")] Tema tema)
        {
            if (ModelState.IsValid)
            {
                db.Temas.Add(tema);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdContacto = new SelectList(db.Contactos, "Id", "Nombres", tema.IdContacto);
            ViewBag.IdPrioridad = new SelectList(db.Prioridads, "Id", "Nombre", tema.IdPrioridad);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Nombre", tema.IdUsuario);
            return View(tema);
        }

        // GET: Temas1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema tema = db.Temas.Find(id);
            if (tema == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdContacto = new SelectList(db.Contactos, "Id", "Nombres", tema.IdContacto);
            ViewBag.IdPrioridad = new SelectList(db.Prioridads, "Id", "Nombre", tema.IdPrioridad);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Nombre", tema.IdUsuario);
            return View(tema);
        }

        // POST: Temas1/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdPrioridad,Descripcion,FechaHora,Verificado,IdUsuario,VerificaFechaHora,Notas,Compras,Pagos,IdContacto")] Tema tema)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tema).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdContacto = new SelectList(db.Contactos, "Id", "Nombres", tema.IdContacto);
            ViewBag.IdPrioridad = new SelectList(db.Prioridads, "Id", "Nombre", tema.IdPrioridad);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "Id", "Nombre", tema.IdUsuario);
            return View(tema);
        }

        // GET: Temas1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema tema = db.Temas.Find(id);
            if (tema == null)
            {
                return HttpNotFound();
            }
            return View(tema);
        }

        // POST: Temas1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tema tema = db.Temas.Find(id);
            db.Temas.Remove(tema);
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
