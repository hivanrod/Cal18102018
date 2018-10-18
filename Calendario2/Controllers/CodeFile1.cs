//var idcontacto = temae.IdContacto;
//var entra = false;
//if (!String.IsNullOrEmpty(submit))
//{
//    if (submit == "Ok2")
//    {
//        entra = true;
//    }
//}
//// buscas el usuario del tema y si es diferente a lresponsable, envía correo de verificación 
//if (temae.IdUsuario.ToString() != idcontacto.ToString() && (entra))
//    {
//        //envia correo de verificacion al usuario de contacto.
//        var Email = "";
//        if (!String.IsNullOrEmpty(temae.IdContacto.ToString()))
//        {
//            Email = (from s in db.Contactos where (s.Id.ToString() == idcontacto.ToString()) select new { Correo = s.CorreoElectronico }).SingleOrDefault().Correo.ToString();
//            //// ****
//            MailMessage mailMsg = new MailMessage();
//            //// To
//            mailMsg.To.Add(new MailAddress(Email.ToString(), Email.ToString()));
//            ////// From
//            mailMsg.From = new MailAddress("info@xcursor.com", "Sistema Automatico de Correo CALENDARIO XcursoR");
//            ////// Subject and multipart/alternative Body
//            mailMsg.Subject = "Alerta Calendario XcursoR - > " + temae.Descripcion;
//            //////string text = "text body";
//            string html = "<BR><BR>Se hace verificacion del tema " + temae.Descripcion + " en fecha y hora " + temae.VerificaFechaHora + "<br><br>Notas:" + temae.Notas.ToString() + "<br><br>Gracias por su atención cal.xcursor.com";
//            //////mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
//            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
//            ////// Init SmtpClient and send
//            SmtpClient smtpClient = new SmtpClient("mail.xcursor.com", Convert.ToInt32(587));
//            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("info@xcursor.com", "124mateoH!!");
//            smtpClient.Credentials = credentials;
//            smtpClient.Send(mailMsg);
//        }
//        else
//        {
//            Email = "No hay idcontacto" + idcontacto.ToString();
//        }
//        temae.Notas = temae.Notas.ToString() + " de contacto enviado " + temae.FechaHora + " " + Email.ToString() + " Notas: " + temae.Notas.ToString();
//    }
