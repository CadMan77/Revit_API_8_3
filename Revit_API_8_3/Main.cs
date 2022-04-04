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

            List<View> views = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Views)
                .WhereElementIsNotElementType()
                .Cast<View>()
                .ToList();

            //string msgBody = string.Empty;
            //foreach (var view in views)
            //{
            //    msgBody += view.Name + $"{Environment.NewLine}";
            //    msgBody += view.Id.ToString() + $"{Environment.NewLine}";
            //}
            //TaskDialog.Show($"{views.Count}", msgBody);

            List<ElementId> viewsId = new List<ElementId>();

            viewsId.Add(views[0].Id);

            //TaskDialog.Show("views[0].Name", views[0].Name);
            //TaskDialog.Show("viewsId.Count", viewsId.Count.ToString());

            ImageExportOptions imgExpOpts = new ImageExportOptions
            {
                ZoomType = ZoomFitType.FitToPage,
                PixelSize = 2024,
                FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + views[0].Name,
                FitDirection = FitDirectionType.Horizontal,
                HLRandWFViewsFileType = ImageFileType.PNG,
                ImageResolution = ImageResolution.DPI_600,
                ExportRange = ExportRange.SetOfViews,
            };

            imgExpOpts.SetViewsAndSheets(viewsId);

            //using (Transaction ts = new Transaction(doc, "Export Transaction"))
            //{
            //    ts.Start();

            doc.ExportImage(imgExpOpts);

            //    ts.Commit();
            //}

            TaskDialog.Show("Выполнено", $"План 1-го этажа проекта {doc.Title} экспортирован в изображение.{Environment.NewLine}(см. \"Рабочий стол\")");

            return Result.Succeeded;
        }
    }
}