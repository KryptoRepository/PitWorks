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
    
    public partial class ДДО
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ДДО()
        {
            this.Дефект = new HashSet<Дефект>();
        }
    
        public System.Guid id { get; set; }
        public Nullable<int> Номер_МПР { get; set; }
        public Nullable<int> Номер_акта_НО { get; set; }
        public Nullable<System.DateTime> Дата_начала_ДДО { get; set; }
        public Nullable<System.DateTime> Дата_окончания_ДДО { get; set; }
        public Nullable<double> Разр__давление { get; set; }
        public Nullable<double> Срок_ремонта { get; set; }
        public Nullable<int> id_результаты_ДДО { get; set; }
    
        public virtual Результаты_ДДО Результаты_ДДО { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Дефект> Дефект { get; set; }
    }
}
