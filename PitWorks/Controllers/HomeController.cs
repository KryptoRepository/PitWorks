using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PitWorks.Models;
using Newtonsoft.Json;
using System.Data.Entity;

namespace PitWorks.Controllers
{
    public class HomeController : Controller
    {
        PitDBEntities db = new PitDBEntities();

        //public ActionResult View()
        //{
        //    return View(db.Внутритрубная_диагностика.First());
        //}

        public ActionResult Index()
        {
            //смотрим роль пользователя с логином из куки
            if (User.Identity.IsAuthenticated)
            {
                var userFromDb = db.Пользователь.FirstOrDefault(u => u.Логин == User.Identity.Name);
                if (userFromDb.id_уровня_доступа == 0)
                    return RedirectToAction("HomePagePDS", "Home");
                else if (userFromDb.id_уровня_доступа == 1)
                    return RedirectToAction("HomePageLPU", "Home");
                else
                    return HttpNotFound();
            }
            else
                return RedirectToAction("Login", "Account");
        }

        //чекбоксы слетят при перезагрузке страницы
        [Authorize]
        public ActionResult HomePagePDS()
        {
            IEnumerable<ЛПУ> lpuFromDB = db.ЛПУ;
            List<LPUModel> mylpuList = new List<LPUModel>();
         
            foreach (var item in lpuFromDB)
            {
                LPUModel lpu = new LPUModel();
                lpu.id = item.id;
                lpu.Name = item.Наименование;
                mylpuList.Add(lpu);
            }
        
            return View(mylpuList);
        }

        [Authorize]
        public ActionResult HomePageLPU()
        {
            var userFromDb = db.Пользователь.FirstOrDefault(u => u.Логин == User.Identity.Name);
            ViewBag.lpuID = userFromDb.id_ЛПУ;

            var lpu = db.ЛПУ.Find(userFromDb.id_ЛПУ);
            ViewBag.lpuName = lpu.Наименование;
            return View();
        }

 
        public PartialViewResult ListObjectsPartial(string lpuIDListJSON)
        {
            //    var ObjectsByLPU = db.Объекты.Where(o => o.id_ЛПУ == id_LPU);  //ФИльтр по лпу
            

            List<Объект> objects = new List<Объект>();
            if (lpuIDListJSON == "" || lpuIDListJSON == "[]")
            {
                objects = db.Объект.ToList();
            }
            else
            {
                try
                {
                    List<int> lpuIDList = JsonConvert.DeserializeObject<List<int>>(lpuIDListJSON);

                    foreach (int id in lpuIDList)
                    {
                        var tempObjectsList = db.Объект.Where(o => o.id_ЛПУ == id);
                        foreach (var obj in tempObjectsList)
                        {
                            objects.Add(obj);
                        }

                    }
                }
                catch(Exception ex)
                {

                }

            }

            return PartialView(objects);
        }

        [HttpGet]
        public ActionResult AddObjectPartial()
        {
            try
            {
                //Модель для передачи двух моделей(ЛПУ и Газопроводы) в одно представление
                AddObjectModel myModel = new AddObjectModel();

                myModel.lpuList = db.ЛПУ;
                myModel.tubeList = db.Участок_газопровода;

                return PartialView(myModel);
            }
            catch (Exception e)
            {
                return PartialView();
            }
        }

        //через ajax передавались только строки, поэтоу у tubePlace tubeNumber тип string
        //почему бы не попробовать изменить AddObjectModel, как в Login. Добавить атрибуты в модель, во вьюхе кнопку submit
        [HttpPost]
        public ActionResult AddObject(string lpuName, string tubeName, string tubePart, string tubePlace, string tubeNumber)
        {
            try
            {
                ЛПУ lpu = new ЛПУ();
                lpu = db.ЛПУ.FirstOrDefault(l => l.Наименование == lpuName);
                Участок_газопровода tube = db.Участок_газопровода.FirstOrDefault(t => t.Наименование == tubeName);

                Объект obj = new Объект();
                obj.id = Guid.NewGuid();
                obj.id_ЛПУ = lpu.id;
                obj.id_участка_газопровода = tube.id;
                obj.Участок_МГ = tubePart;
                string tmp = tubePlace.Replace('.', ',');
                obj.Место = Convert.ToDouble(tmp);
                obj.Номер_трубы = int.Parse(tubeNumber);


                db.Объект.Add(obj);
                db.SaveChanges();
                ViewBag.IsAdd = true;
            }
            catch
            {
                ViewBag.IsAdd = false;
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ListDefectsPartial(Guid id)
        {
            try
            {
                Объект obj = db.Объект.FirstOrDefault(o => o.id == id);
                List<TableDefectModel> defectItems = new List<TableDefectModel>();

                var lpu = db.ЛПУ.FirstOrDefault(l => l.id == obj.id_ЛПУ);
                var tube = db.Участок_газопровода.FirstOrDefault(t => t.id == obj.id_участка_газопровода);
                
                //во вьюбаг инфа об объекте
                ViewBag.objectID = obj.id;
                ViewBag.lpuName = lpu.Наименование;
                ViewBag.tubeName = tube.Наименование;
                ViewBag.tubePart = obj.Участок_МГ;
                ViewBag.tubePlace = obj.Место;
                ViewBag.tubeNumber = obj.Номер_трубы;

                var defects = db.Дефект.Where(d => d.id_объекта == obj.id);

                foreach (var def in defects)
                {
                    // инфа о дефекте: втд, ддо, ремонте
                    TableDefectModel item = new TableDefectModel();
                    item.id = def.id;

                    item.vtd = db.Внутритрубная_диагностика.FirstOrDefault(v => v.id == def.id_ВТД);
                    item.ddo = db.ДДО.FirstOrDefault(d => d.id == def.id_ДДО);
                    item.repair = db.Ремонт.FirstOrDefault(r => r.id == def.id_Ремонта);

                    item.defKind = db.Виды_дефектов.FirstOrDefault(i => i.id == item.vtd.id_виды_дефектов);
                    item.ddoResult = db.Результаты_ДДО.FirstOrDefault(i => i.id == item.ddo.id_результаты_ДДО);
                    item.repKind = db.Виды_ремонта.FirstOrDefault(i => i.id == item.repair.id_виды_ремонта);

                    defectItems.Add(item);
                }

                ViewBag.defects = defectItems;
                return PartialView();
            }
            catch (Exception ex)
            {
                return PartialView();
            }
        }

        public ActionResult CatalogDetailedDefects()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddDefectPartial()
        {
            return PartialView();
        }

        [HttpPost]
        public string AddDefect(string objectID, string[] defectInfo)
        {
            try  //передать id объекта
            {
                //   var obj = db.Объект.FirstOrDefault(o => o.id == currentObject.id);

                string tmpString;
                DateTime tmpDate;
              
                Внутритрубная_диагностика tubeTest = new Внутритрубная_диагностика();
                tubeTest.id = Guid.NewGuid();
                tubeTest.id_виды_дефектов = int.Parse(defectInfo[0]);          //1
                tubeTest.Ориент__дефекта = int.Parse(defectInfo[1]);           //2
                tubeTest.Длина_дефекта = int.Parse(defectInfo[2]);             //3
                tubeTest.Ширина_дефекта = int.Parse(defectInfo[3]);            //4
                tubeTest.Глубина_дефекта = int.Parse(defectInfo[4]);           //5
                tmpString = defectInfo[5].Replace('.', ',');
                tubeTest.Срок_НО_по_ВТД = Convert.ToDouble(tmpString);         //6
                tmpDate = DateTime.Parse(defectInfo[6]);
                tubeTest.Дата_проведения_ВТД = tmpDate;                        //7


                ДДО extraDiagnostic = new ДДО();
                extraDiagnostic.id = Guid.NewGuid();
                extraDiagnostic.Номер_МПР = int.Parse(defectInfo[7]);                //8 
                extraDiagnostic.Номер_акта_НО = int.Parse(defectInfo[8]);            //9
                extraDiagnostic.Дата_начала_ДДО = DateTime.Parse(defectInfo[9]);     //10
                extraDiagnostic.Дата_окончания_ДДО = DateTime.Parse(defectInfo[10]); //11

                tmpString = defectInfo[11].Replace('.', ',');
                extraDiagnostic.Разр__давление = Convert.ToDouble(tmpString);        //12

                tmpString = defectInfo[12].Replace('.', ',');
                extraDiagnostic.Срок_ремонта = Convert.ToDouble(tmpString);          //13
                extraDiagnostic.id_результаты_ДДО = int.Parse(defectInfo[13]);       //14


                Ремонт repair = new Ремонт();
                repair.id = Guid.NewGuid();
                repair.Дата_начала = DateTime.Parse(defectInfo[14]);
                repair.Дата_окончания = DateTime.Parse(defectInfo[15]);
                repair.id_виды_ремонта = int.Parse(defectInfo[16]);
                repair.Примечание = defectInfo[17];


                Дефект defect = new Дефект();
                defect.id = Guid.NewGuid();
                defect.id_признак_состояния = 0;
                defect.id_ВТД = tubeTest.id;
                defect.id_ДДО = extraDiagnostic.id;
                defect.id_Ремонта = repair.id;

                try
                {
                    defect.id_объекта = Guid.Parse(objectID);
                }
                catch (Exception ex)
                {

                }

                db.Внутритрубная_диагностика.Add(tubeTest);
                db.ДДО.Add(extraDiagnostic);
                db.Ремонт.Add(repair);
                db.Дефект.Add(defect);
               

                db.SaveChanges();
            }
            catch
            {

            }

            return objectID;
        }

        [HttpGet]
        public ActionResult EditDefectPartial(Guid defecctID) //
        {
            var def = db.Дефект.Find(defecctID);
            var vtd = db.Внутритрубная_диагностика.Find(def.id_ВТД);
            var ddo = db.ДДО.Find(def.id_ДДО);
            var repair = db.Ремонт.Find(def.id_Ремонта);
            var defKind = db.Виды_дефектов;
            var resultDDO = db.Результаты_ДДО;
            var repairKind = db.Виды_ремонта;

            EditDefectModel defect = new EditDefectModel(def, vtd, ddo, repair, defKind, resultDDO, repairKind);

            return PartialView(defect);
        }

        [HttpPost]
        public void EditDefectPartial(EditDefectModel editModel)
        {
            Дефект def = new Дефект();
            editModel.defect.id_признак_состояния = Convert.ToInt32(editModel.status);
            //db.Entry(editModel.vtd).State = EntityState.Modified;
            //db.Entry(editModel.ddo).State = EntityState.Modified;
            //db.Entry(editModel.repair).State = EntityState.Modified;
            //db.Entry(editModel.defect).State = EntityState.Modified;

            //db.Entry(book).State = EntityState.Modified;
        }

        //[HttpGet]
        //public ActionResult TableDefectsPartial(Guid id)
        //{
        //    //вытаскивать данные 
        //    try
        //    {
        //        //    currentObject = db.Объект.FirstOrDefault(o => o.id == id);
        //        List<TableDefectModel> defectItems = new List<TableDefectModel>();
        //        var obj = db.Объект.FirstOrDefault(o => o.id == id);
        //        var lpu = db.ЛПУ.FirstOrDefault(l => l.id == obj.id_ЛПУ);
        //        var tube = db.Участок_газопровода.FirstOrDefault(t => t.id == obj.id_участка_газопровода);

        //        //во вьюбаг инфа о объекте
        //        ViewBag.objectID = id;
        //        ViewBag.lpuName = lpu.Наименование;
        //        ViewBag.tubeName = tube.Наименование;
        //        ViewBag.tubePart = obj.Участок_МГ;
        //        ViewBag.tubePlace = obj.Место;
        //        ViewBag.tubeNumber = obj.Номер_трубы;

        //        var defects = db.Дефект.Where(d => d.id_объекта == id);

        //        foreach (var def in defects)
        //        {
        //            //в моделе инфа о втд, ддо, ремонте
        //            TableDefectModel item = new TableDefectModel();
        //            item.id_признак_состояния = (int)def.id_признак_состояния;

        //            item.vtd = db.Внутритрубная_диагностика.FirstOrDefault(v => v.id == def.id_ВТД);
        //            item.ddo = db.ДДО.FirstOrDefault(d => d.id == def.id_ДДО);
        //            item.repair = db.Ремонт.FirstOrDefault(r => r.id == def.id_Ремонта);


        //            item.defKind = db.Виды_дефектов.FirstOrDefault(i => i.id == item.vtd.id_виды_дефектов);
        //            item.ddoResult = db.Результаты_ДДО.FirstOrDefault(i => i.id == item.ddo.id_результаты_ДДО);
        //            item.repKind = db.Виды_ремонта.FirstOrDefault(i => i.id == item.repair.id_виды_ремонта);

        //            defectItems.Add(item);
        //        }


        //        ViewBag.defects = defectItems;
        //        return PartialView();
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }   
        //}

        public ActionResult CreateReport()
        {
           
            return View();
        }

        public string GetObjectsToReportJSON()
        {
            //List<ObjectWithDefectsModel> objectsToView = new List<ObjectWithDefectsModel>();
            //List<DefectModel> defectItems = new List<DefectModel>();
            //var objects = db.Объект;

            //foreach (var obj in objects)
            //{
            //    var defects = db.Дефект.Where(d => d.id_объекта == obj.id);

            //    foreach (var def in defects)
            //    {
            //        // инфа о дефекте: втд, ддо, ремонте
            //        DefectModel item = new DefectModel();

            //        item.vtd = db.Внутритрубная_диагностика.FirstOrDefault(v => v.id == def.id_ВТД);
            //        item.ddo = db.ДДО.FirstOrDefault(d => d.id == def.id_ДДО);
            //        item.repair = db.Ремонт.FirstOrDefault(r => r.id == def.id_Ремонта);

            //        item.defKind = db.Виды_дефектов.FirstOrDefault(i => i.id == item.vtd.id_виды_дефектов);
            //        item.ddoResult = db.Результаты_ДДО.FirstOrDefault(i => i.id == item.ddo.id_результаты_ДДО);
            //        item.repKind = db.Виды_ремонта.FirstOrDefault(i => i.id == item.repair.id_виды_ремонта);
            //        item.id_признак_состояния = (int)def.id_признак_состояния;

            //        defectItems.Add(item);
            //    }

            //    objectsToView.Add(new ObjectWithDefectsModel(obj, defectItems));
            //}

            var objects = db.Объект;
            List<ReportCheckBoxItem> checkBoxItems = new List<ReportCheckBoxItem>();

            foreach (var obj in objects)
            {
                ReportCheckBoxItem objectCheckBox = new ReportCheckBoxItem(obj.id.ToString(), "Участок МГ: " + obj.Участок_МГ);
                objectCheckBox.children = new List<ReportCheckBoxItem>();

                var defects = db.Дефект.Where(d => d.id_объекта == obj.id);

                foreach (var def in defects)
                {
                    var vtd = db.Внутритрубная_диагностика.FirstOrDefault(v => v.id == def.id_ВТД);
                    ReportCheckBoxItem defectCheckBox = new ReportCheckBoxItem(def.id.ToString(), "Дата ВТД: " + vtd.Дата_проведения_ВТД.ToString());
                    
                    objectCheckBox.children.Add(defectCheckBox);
                }
                
                checkBoxItems.Add(objectCheckBox);
            }

            return JsonConvert.SerializeObject(checkBoxItems);
        }

        
        public ActionResult ReportTablePartial(string keysJSON)
        {
            //keysJSON - json строка с id объектов и дефектов, выбранных в чекбоксах. В строке подряд идут id объекта, затем id его дефектов
            var keys = JsonConvert.DeserializeObject<Guid[]>(keysJSON);
            List<ReportObjectModel> reportObjects = new List<ReportObjectModel>(); //модель для вьюхи

            foreach (var key in keys)  
            {
                Объект pitObject = db.Объект.FirstOrDefault(o => o.id == key);
                //если id из массива принадлежит Объекту
                if (pitObject != null)
                {
                    //добавляем в модель объект, участок газопровода, имя ЛПУ
                    ReportObjectModel tempObj = new ReportObjectModel();
                    tempObj.pitObject = pitObject;
                    tempObj.sectionTube = db.Участок_газопровода.FirstOrDefault(f => f.id == pitObject.id_участка_газопровода).Наименование;
                    tempObj.lpuName = db.ЛПУ.FirstOrDefault(f => f.id == pitObject.id_ЛПУ).Наименование;
                    reportObjects.Add(tempObj);
                }
                else // //если id из массива принадлежит Дефекту - добавляем дефекты к ОБъекту
                {
                    ReportDefectModel tempDef = new ReportDefectModel();
                    Дефект def = db.Дефект.FirstOrDefault(d => d.id == key);
                    tempDef.defectID = def.id;

                    tempDef.vtd = db.Внутритрубная_диагностика.FirstOrDefault(v => v.id == def.id_ВТД);
                    tempDef.ddo = db.ДДО.FirstOrDefault(d => d.id == def.id_ДДО);
                    tempDef.repair = db.Ремонт.FirstOrDefault(r => r.id == def.id_Ремонта);

                    tempDef.defKind = db.Виды_дефектов.FirstOrDefault(i => i.id == tempDef.vtd.id_виды_дефектов);
                    tempDef.ddoResult = db.Результаты_ДДО.FirstOrDefault(i => i.id == tempDef.ddo.id_результаты_ДДО);
                    tempDef.repKind = db.Виды_ремонта.FirstOrDefault(i => i.id == tempDef.repair.id_виды_ремонта);
                    tempDef.id_признак_состояния = (int)def.id_признак_состояния;

                    reportObjects.Last().defects.Add(tempDef); //добавляем к последнему добавленному объекту дефект
                }
            }
            //ViewBag.reportObjects = reportObjects;
            return PartialView(reportObjects); //reportObjects экспортировать в EXEL
        }

    }
}