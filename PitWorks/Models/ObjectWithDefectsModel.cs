using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PitWorks.Models
{
    public class ObjectWithDefectsModel
    {
        public Объект shurfObject = new Объект();
        public List<DefectModel> defects = new List<DefectModel>();

        public ObjectWithDefectsModel() { }

        public ObjectWithDefectsModel(Объект shurfObj)
        {
            this.shurfObject = shurfObj;
        }

        public ObjectWithDefectsModel(Объект shurfObj, List<DefectModel> defects)
        {
            this.shurfObject = shurfObj;
            this.defects = defects;
        }
    }
}