namespace Calendario2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contactos")]
    public partial class Contacto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contacto()
        {
            Temas = new HashSet<Tema>();
        }

        public int Id { get; set; }

        public string IdAspNetUsers { get; set; }

        [StringLength(500)]
        public string Nombres { get; set; }

        [StringLength(500)]
        public string Apellidos { get; set; }

        [StringLength(500)]
        public string Telefono { get; set; }

        [StringLength(500)]
        public string Direccion { get; set; }

        [StringLength(500)]
        public string CorreoElectronico { get; set; }

        [StringLength(500)]
        public string Empresa { get; set; }

        [StringLength(2000)]
        public string Notas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tema> Temas { get; set; }
    }
}
