using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VibrantBIM.ViewModels;

namespace VibrantBIM
{

    [TransactionAttribute(TransactionMode.Manual)]
    internal class FrameForceTable : Autodesk.Revit.UI.IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
           
            try
            {
                UIDocument _uidoc = commandData.Application.ActiveUIDocument;
                Document _document = _uidoc.Document;
                var vm = new FrameForceVM();
                vm.FrameForceDataView.ShowDialog();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                return Result.Cancelled;
            };
        }
    }
}
