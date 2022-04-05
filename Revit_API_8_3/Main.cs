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

            // косвенное(!!) получение необходимого вида:
            View theView = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Views)
                .WhereElementIsNotElementType()
                .Cast<View>()                
                .FirstOrDefault(); 

            //TaskDialog.Show("theViewName", theView.Name);

            List<ElementId> viewIDs = new List<ElementId>();

            viewIDs.Add(theView.Id);

            //TaskDialog.Show($"viewIDs", viewIDs[0].ToString());

            ImageExportOptions imgExpOpts = new ImageExportOptions
            {
                ZoomType = ZoomFitType.FitToPage,
                PixelSize = 2024,
                FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + theView.Name,
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