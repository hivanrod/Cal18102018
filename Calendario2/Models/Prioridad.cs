namespace Calendario2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Prioridad")]
    public partial class Prioridad
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Prioridad()
        {
            Temas = new HashSet<Tema>();
        }

        public int Id { get; set; }

        public string UserId { get; set; }

        [StringLength(2000)]
        public string Nombre { get; set; }

        public short? Orden { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tema> Temas { get; set; }
    }
}
