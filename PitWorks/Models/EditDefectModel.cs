using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PitWorks.Models
{
    public class EditDefectModel
    {
        public Guid objectID { get; set; }
        public Дефект defect { get; set; }
        public Внутритрубная_диагностика vtd { get; set; }
        public ДДО ddo { get; set; }
        public Ремонт repair { get; set; }
        public SelectList defKind { get; set; }
        public SelectList resultDDO { get; set; }
        public SelectList repairKind { get; set; }
        public bool status { get; set; }

        public EditDefectModel()
        {

        }

        public EditDefectModel(Дефект _defect, Внутритрубная_диагностика _vtd, ДДО _ddo, Ремонт _repair, 
            IEnumerable<Виды_дефектов> _defKind, IEnumerable<Результаты_ДДО> _resultDDO, IEnumerable<Виды_ремонта> _repairKind)
        {
            defect = _defect;
            vtd = _vtd;
            ddo = _ddo;
            repair = _repair;
            defKind = new SelectList(_defKind, "id", "Наименование", vtd.id_виды_дефектов);
            repairKind = new SelectList(_repairKind, "id", "Наименование", repair.id_виды_ремонта);
            resultDDO = new SelectList(_resultDDO, "id", "Наименование", ddo.id_результаты_ДДО);
            status = Convert.ToBoolean(defect.id_признак_состояния);
            objectID = (Guid)defect.id_объекта;
            
        }
    }
}