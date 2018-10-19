using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calendario2.Models;


namespace Calendario2.Controllers
{
    public class DownloadsController : Controller
    {
        //private xcursorc_intravoidEntities db = new xcursorc_intravoidEntities();

        // GET: Downloads
        public FileResult Download(string ImageName, string Nombre, int? IdCliente)
        {

            // agrega el log historico
            //var str_desc = "Baja archivo - " + ImageName.ToString() + " - de carpeta -" + Tema + " -";
            //var proc = db.pa_crear_log(2, str_desc, 1, 1, IdCliente, 1, 0, System.DateTime.Now);

            return File(Server.MapPath("~/Views/Shared/Upload/" + Nombre) + "/" + ImageName, System.Net.Mime.MediaTypeNames.Application.Octet, ImageName);
        }
    }
}