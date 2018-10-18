using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Calendario2.Models;
using Microsoft.AspNet.Identity;

namespace Calendario2.Controllers
{
    public class ContactosController : Controller
    {
        private Model1 db = new Model1();

        // GET: Contactos
        [Authorize]
        public ActionResult Index()
        {
            var contactos = db.Contactos;
            if (User.Identity.IsAuthenticated)
            {
                string currentUserId = User.Identity.GetUserId();
                string IdUser = currentUserId;
                var cont = from s in db.Contactos
                           where s.IdAspNetUsers.ToString() == IdUser
                           select s;
                return View(cont.ToList());
            }
            return View();

        }

        // GET: Contactos/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacto contacto = db.Contactos.Find(id);
            if (contacto == null)
            {
                return HttpNotFound();
            }
            return View(contacto);
        }

        // GET: Contactos/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contactos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombres,Apellidos,Telefono,Direccion,CorreoElectronico,Empresa,Notas")] Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                var IdUser = "";
                if (User.Identity.IsAuthenticated)
                {
                    string currentUserId = User.Identity.GetUserId();
                    IdUser = currentUserId;
                    contacto.IdAspNetUsers = IdUser;
                    db.Contactos.Add(contacto);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(contacto);
        }

        // GET: Contactos/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacto contacto = db.Contactos.Find(id);
            if (contacto == null)
            {
                return HttpNotFound();
            }
            return View(contacto);
        }

        // POST: Contactos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombres,Apellidos,Telefono,Direccion,CorreoElectronico,Empresa,Notas")] Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                var IdUser = "";
                if (User.Identity.IsAuthenticated)
                {
                    string currentUserId = User.Identity.GetUserId();
                    IdUser = currentUserId;
                    contacto.IdAspNetUsers = IdUser;
                    db.Entry(contacto).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(contacto);
        }

        // GET: Contactos/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacto contacto = db.Contactos.Find(id);
            if (contacto == null)
            {
                return HttpNotFound();
            }
            return View(contacto);
        }

        // POST: Contactos/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contacto contacto = db.Contactos.Find(id);
            db.Contactos.Remove(contacto);
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
