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
    
    public partial class Внутритрубная_диагностика
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Внутритрубная_диагностика()
        {
            this.Дефект = new HashSet<Дефект>();
        }
    
        public System.Guid id { get; set; }
        public Nullable<int> id_виды_дефектов { get; set; }
        public Nullable<int> Ориент__дефекта { get; set; }
        public Nullable<int> Длина_дефекта { get; set; }
        public Nullable<int> Ширина_дефекта { get; set; }
        public Nullable<int> Глубина_дефекта { get; set; }
        public Nullable<double> Срок_НО_по_ВТД { get; set; }
        public Nullable<System.DateTime> Дата_проведения_ВТД { get; set; }
    
        public virtual Виды_дефектов Виды_дефектов { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Дефект> Дефект { get; set; }
    }
}
