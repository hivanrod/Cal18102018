namespace Calendario2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tema")]
    public partial class Tema
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int? IdPrioridad { get; set; }

        [StringLength(2000)]
        public string Descripcion { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? FechaHora { get; set; }

        public bool? Verificado { get; set; }

        public int? IdUsuario { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? VerificaFechaHora { get; set; }

        [StringLength(2000)]
        public string Notas { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public int? Ingreso { get; set; }


        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public int? Presupuesto { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public int? Compras { get; set; }


        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public int? Pagos { get; set; }

        public int? IdContacto { get; set; }

        public int? Total { get; set; } = 0;

        public int? Pasadas { get; set; } = 0;

        public int? Hoy { get; set; } = 0;

        public int? Futuras { get; set; } = 0;
        public int? Archivos { get; set; } = 0;

        public virtual Contacto Contacto { get; set; }

        public virtual Prioridad Prioridad { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
