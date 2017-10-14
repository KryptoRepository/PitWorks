using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Проверить класс на необходимость 
namespace PitWorks.Models
{
    public class TableDefectModel
    {
        public Guid id;
        public Внутритрубная_диагностика vtd;
        public ДДО ddo;
        public Ремонт repair;
        public Виды_дефектов defKind;
        public Результаты_ДДО ddoResult;
        public Виды_ремонта repKind;
        public int id_признак_состояния;
    }
}