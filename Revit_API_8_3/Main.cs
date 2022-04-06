// Экспортируйте в качестве изображения план 1 этажа из файла упражнений (RevitSheetsExport.rvt).

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit_API_8_3
{
    [Transaction(TransactionMode.Manual)]

    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            #region//косвенное(!!) получение необходимого Вида

            // - по Категории:

            //View theView = new FilteredElementCollector(doc)
            //    .OfCategory(BuiltInCategory.OST_Views)
            //    .WhereElementIsNotElementType()
            //    .Cast<View>()
            //    .FirstOrDefault();

            // - по Классу:

            //View theView = new FilteredElementCollector(doc)
            //    .OfClass(typeof(View))
            //    .WhereElementIsNotElementType()
            //    .Cast<View>()
            //    .FirstOrDefault();

            //TaskDialog.Show("theViewName", theView.Name);
            #endregion

            //Пполучение необходимого Плана(этажа) по Классу,ТипуВида и имени:
            ViewPlan theViewPlan = new FilteredElementCollector(doc)
               .OfClass(typeof(ViewPlan)) // планы (этажа(пола)/потолка/конструктивный)
               .WhereElementIsNotElementType()
               .Cast<ViewPlan>()
               .FirstOrDefault(vp => vp.ViewType == ViewType.FloorPlan && vp.Name.Equals("Level 1"));

            //List <ViewPlan> floorPlans = new FilteredElementCollector(doc)
            //    .OfClass(typeof(ViewPlan))
            //    .WhereElementIsNotElementType()
            //    .Cast<ViewPlan>()
            //    .Where (p => p.ViewType == ViewType.FloorPlan) // excludes ceiling Plans
            //    .ToList();

            //string fpNames = string.Empty;
            //foreach (var fp in floorPlans)
            //{
            //    fpNames += fp.Name +Environment.NewLine;
            //}
            //TaskDialog.Show($"FPs:{floorPlans.Count}", fpNames);

            List<ElementId> viewIDs = new List<ElementId>();

            viewIDs.Add(theViewPlan.Id);

            //TaskDialog.Show($"viewIDs", viewIDs[0].ToString());

            ImageExportOptions imgExpOpts = new ImageExportOptions
            {
                ZoomType = ZoomFitType.FitToPage,
                PixelSize = 2024,
                FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + theViewPlan.Name,
                FitDirection = FitDirectionType.Horizontal,
                HLRandWFViewsFileType = ImageFileType.PNG,
                ImageResolution = ImageResolution.DPI_600,
                ExportRange = ExportRange.SetOfViews,
            };

            imgExpOpts.SetViewsAndSheets(viewIDs);

            doc.ExportImage(imgExpOpts);

            TaskDialog.Show("Выполнено", $"План 1-го этажа проекта {doc.Title} экспортирован в изображение.{Environment.NewLine}(см. \"Рабочий стол\")");

            return Result.Succeeded;
        }
    }
}