using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Calendario2.Models;

namespace INTRANETVOID.Controllers
{
    public class ArchivosController : Controller
    {
        private Model1 db = new Model1();

        private xcursorc_calEntities db2 = new xcursorc_calEntities();

        // GET: Archivos
        public ActionResult Index(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var arc = from t in db.Archivos
                       where t.IdTema.ToString() == Id.ToString()
                       orderby t.IdTema
                       select t;
            var tem = from t2 in db.Temas
                      where t2.Id == Id
                      select t2;
            ViewBag.Tema = tem.SingleOrDefault().Descripcion.ToString();
            return View(arc.ToList());
        }

        // GET: Archivos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archivo archivo = db.Archivos.Find(id);
            if (archivo == null)
            {
                return HttpNotFound();
            }
            return View(archivo);
        }

        // GET: Archivos/Create
        public ActionResult Create()
        {
            //ViewBag.IdCliente = new SelectList(db.Clientes.OrderBy(s => s.Nombres), "Id", "Nombres");
            ViewBag.IdTema = new SelectList(db.Temas.OrderBy(s => s.Descripcion), "Id", "Descripcion");
            return View();
        }

        // POST: Archivos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdTema,Nombre")] Archivo archivo, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string _path = "";
                if (upload.ContentLength > 0)
                {
                    try
                    {

                        string _FileName = Path.GetFileName(upload.FileName);
                        string tema = (from t in db.Temas
                                       where t.Id.ToString() == archivo.IdTema.ToString()
                                       select t.Descripcion).First().ToString();

                        string strPath = "~/Views/Shared/Upload/" + tema;
                        _path = Path.Combine(Server.MapPath(strPath), _FileName);
                        upload.SaveAs(_path);
                        // crea en db
                        db.Archivos.Add(archivo);
                        archivo.Nombre = _FileName;
                        db.SaveChanges();

                        // agrega el log historico
                        var str_desc = "Crea archivo - " + archivo.Nombre.ToString() + " - en carpeta -" + tema + " -";
                        //var proc = db.pa_crear_log(1, str_desc, 1, 1, archivo.IdCliente, 1, 0, System.DateTime.Now);




                        ViewBag.Message = "Archivo cargado exitosamente!!";
                    }
                    catch
                    {
                        ViewBag.Message = "No se pudo cargar el archivo!!";
                    }

                }

            }
            //ViewBag.IdCliente = new SelectList(db.Clientes.OrderBy(s => s.Nombres), "Id", "Nombres", archivo.IdCliente);
            ViewBag.IdTema = new SelectList(db.Temas.OrderBy(s => s.Descripcion), "Id", "Nombre", archivo.IdTema);
            ViewBag.Message = ViewBag.Message + "-->> Y se creo correctamente el registro en la base de datos!!";
            return View(archivo);

        }

        // GET: Archivos/Edit/5 
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archivo archivo = db.Archivos.Find(id);
            if (archivo == null)
            {
                return HttpNotFound();
            }
            //ViewBag.IdCliente = new SelectList(db.Clientes, "Id", "Nombres", archivo.IdCliente);
            ViewBag.IdTema = new SelectList(db.Temas, "Id", "Descripcion", archivo.IdTema);
            return View(archivo);
        }

        // POST: Archivos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdTema,Orden,IdCliente,Nombre,Origen")] Archivo archivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(archivo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.IdCliente = new SelectList(db.Clientes, "Id", "Nombres", archivo.IdCliente);
            ViewBag.IdTema = new SelectList(db.Temas, "Id", "Descripcion", archivo.IdTema);
            return View(archivo);
        }

        // GET: Archivos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archivo archivo = db.Archivos.Find(id);
            if (archivo == null)
            {
                return HttpNotFound();
            }
            return View(archivo);
        }

        // POST: Archivos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Archivo archivo = db.Archivos.Find(id);
            string fullPath = Request.MapPath("~/Views/Shared/Upload/" + archivo.Nombre + "/" + archivo.Nombre.ToString());
            var str_desc = "";
            //string fullPath = Path.Combine(Server.MapPath(strPath), archivo.Nombre.ToString());
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                str_desc = "Elimina archivo - " + archivo.Nombre.ToString() + " - de carpeta -" + archivo.Nombre.ToString() + " -";
            }
            else
            {
                str_desc = "No se encontró el archivo en el sistema.  Se Elimina archivo de la base de datos - " + archivo.Nombre.ToString() + " - de carpeta -" + archivo.Nombre.ToString() + " -";
            }
            // logs
            //var proc = db.pa_crear_log(3, str_desc, 1, 1, archivo.IdCliente, 3, 0, System.DateTime.Now);
            // bprra de bd
            db.Archivos.Remove(archivo);
            db.SaveChanges();
            TempData["Msg"] = str_desc;


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
