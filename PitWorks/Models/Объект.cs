//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PitWorks.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Объект
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Объект()
        {
            this.Дефект = new HashSet<Дефект>();
        }
    
        public System.Guid id { get; set; }
        public int id_участка_газопровода { get; set; }
        public string Участок_МГ { get; set; }
        public double Место { get; set; }
        public int Номер_трубы { get; set; }
        public int id_ЛПУ { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Дефект> Дефект { get; set; }
        public virtual ЛПУ ЛПУ { get; set; }
        public virtual Участок_газопровода Участок_газопровода { get; set; }
    }
}
