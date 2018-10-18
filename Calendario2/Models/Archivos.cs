namespace Calendario2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Archivos")]
    public partial class Archivo
    {

        public int Id { get; set; }

        public string UserId { get; set; }

        public int? IdTema { get; set; }

        public string Nombre { get; set; }

        //public virtual ICollection<Tema> IdTema { get; set; }


    }
}