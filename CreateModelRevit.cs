using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ETABSv1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibrantBIM.ViewModels;

namespace VibrantBIM
{
    [TransactionAttribute(TransactionMode.Manual)]
    internal class CreateModelRevit : Autodesk.Revit.UI.IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument _uidoc = commandData.Application.ActiveUIDocument;
            Document _document = _uidoc.Document;
            try
            {
                var vm = new ImportEDBViewModel(_uidoc, _document);
                vm.ImportEDBView.ShowDialog();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                return Result.Cancelled;
                TaskDialog.Show("Thông báo", "Đã xảy ra lỗi: " + ex.Message);
            };
        }
    }
}
