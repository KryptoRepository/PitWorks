﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PitDBEntities : DbContext
    {
        public PitDBEntities()
            : base("name=PitDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Виды_дефектов> Виды_дефектов { get; set; }
        public virtual DbSet<Виды_ремонта> Виды_ремонта { get; set; }
        public virtual DbSet<Внутритрубная_диагностика> Внутритрубная_диагностика { get; set; }
        public virtual DbSet<ДДО> ДДО { get; set; }
        public virtual DbSet<Дефект> Дефект { get; set; }
        public virtual DbSet<ЛПУ> ЛПУ { get; set; }
        public virtual DbSet<Объект> Объект { get; set; }
        public virtual DbSet<Пользователь> Пользователь { get; set; }
        public virtual DbSet<Признак_состояния_работы> Признак_состояния_работы { get; set; }
        public virtual DbSet<Результаты_ДДО> Результаты_ДДО { get; set; }
        public virtual DbSet<Ремонт> Ремонт { get; set; }
        public virtual DbSet<Уровни_доступа> Уровни_доступа { get; set; }
        public virtual DbSet<Участок_газопровода> Участок_газопровода { get; set; }
    }
}
