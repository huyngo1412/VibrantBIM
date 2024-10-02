using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ETABSv1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using VibrantBIM.Extensions;
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
            UIApplication _uiApp = commandData.Application;
           
            try
            {
                var vm = new ImportModelETABSVM(_uidoc, _document);
                Thread newWindowThread = new Thread(() =>
                {                   
                    vm.ImportModelETABSView.Dispatcher.Invoke(() =>
                    {
                        vm.ImportModelETABSView.ShowDialog();
                    });
                 
                    Dispatcher.Run();
                });
                newWindowThread.SetApartmentState(ApartmentState.STA);
                newWindowThread.Start();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                return Result.Cancelled;
            };
        }
    }
}
