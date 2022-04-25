using System;
using System.Collections.Generic;
using System.Linq;
using KuzbassEnergo.Helper;
using KuzbassEnergo.Model;
using Spire.Xls;

namespace KuzbassEnergo
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var context = new Context()) { }
            Workbook wb = new Workbook();
            wb.LoadFromFile("TestData.xlsx");
            Worksheet ws = wb.ActiveSheet;

            List<TableModel> collection = new();


            List<Repository> rcollection = new();
            List<Process> pcollection = new();
            List<SubDivision> scollection = new();

            #region Парсинг таблицы
            for (int row = 5; row < ws.Rows.Length + 1; row++)
            {
                TableModel tm = new();
                tm.Process = ws[row, 2].DisplayedText;
                tm.Name = ws[row, 3].DisplayedText;
                //Проверка Подразделения Владельца процесса
                if(!String.IsNullOrEmpty(ws[row, 4].DisplayedText)){
                    //Если в коллекции отсутствует, добавляем
                    if (!scollection.Exists(x => x.Name == ws[row, 4].DisplayedText)){
                        scollection.Add(new(ws[row, 4].DisplayedText));
                    }
                    //Добавление к табличным данным
                    tm.ProcessOwner.Add(scollection.Find(x => x.Name == ws[row, 4].DisplayedText));
                }
                    
                //Проверка объединения ячейки для подразделений владельцев процесса 
                if (String.IsNullOrEmpty(tm.Process) && String.IsNullOrEmpty(tm.Name)) {
                    if(tm.ProcessOwner.Count > 0)
                        collection[^1].ProcessOwner.Add(tm.ProcessOwner[0]);
                }
                else { collection.Add(tm); }
            }
            #endregion

            #region Распределение по моделям и построение иерархии
            for (int i = 0; i < collection.Count; i++)
            {
                //Проверяется код подразделения и подразделение владельцев процесса
                if (String.IsNullOrEmpty(collection[i].Process) && collection[i].ProcessOwner.Count == 0)
                {
                    Repository r = new(collection[i].Name);
                    if (rcollection.Count != 0)
                    {
                        r.AddOwnerRepository(rcollection[0]);
                    }
                    rcollection.Add(r);
                }
                else
                {
                    Repository r = new(collection[i].Name, collection[i].ProcessOwner);
                    Process p = new(collection[i].Process, r);

                    if (pcollection.Count == 0)
                    {
                        pcollection.Add(p);
                        r.AddOwnerRepository(rcollection[^1]);
                    }
                    else
                    {
                        //Поиск родителя для процесса и репозитория
                        if (p.Name.Length != 2)
                        {
                            var splitName = p.Name.Split(".");
                            var temp = splitName[splitName.Length - 1].Length + 1;
                            Process result = pcollection.Find(x => x.Name == p.Name.Remove(p.Name.Length - temp));
                            p.SetOwnerProcess(result);
                            r.AddOwnerRepository(result.Repository);
                        }
                        else
                        {
                            var result = rcollection.FindLast(x => x.HasProcess() == false && (x.ProcessOwner == null || x.ProcessOwner.Count == 0));
                            r.AddOwnerRepository(result);
                        }
                        pcollection.Add(p);
                    }
                    rcollection.Add(r);
                }
                foreach (var item in rcollection)
                {
                    item.ConverProcessOwnerList();
                }

            }

            #endregion

            AddDataToDataBase();


            void AddDataToDataBase()
            {
                using(Context db = new())
                {
                    foreach(var item in rcollection)
                    {
                        db.Repositories.Add(item);
                    }
                    foreach(var item in pcollection)
                    {
                        db.Processes.Add(item);
                    }
                    foreach(var item in scollection)
                    {
                        db.SubDivisions.Add(item);
                    }
                    db.SaveChanges();
                }
            }
        }
        
    }

    public class TableModel
    {
        public string Process;
        public string Name;
        public List<SubDivision> ProcessOwner = new();
    }
}
