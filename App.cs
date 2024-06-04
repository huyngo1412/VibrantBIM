using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace VibrantBIM
{
    public class App : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                application.CreateRibbonTab("VibrantBIMExtension");
                RibbonPanel panelImportEDB = application.CreateRibbonPanel("VibrantBIMExtension", "ImportEDB");
                PushButtonData btnDataImportEDB = new PushButtonData(
                    "ImportETABS",
                    "ImportETABS",
                    Assembly.GetExecutingAssembly().Location, "VibrantBIM.CreateModelRevit")
                {
                   
                };
                PushButton buttonImportEDB = panelImportEDB.AddItem(btnDataImportEDB) as PushButton;
                buttonImportEDB.Enabled = true;
            }
            catch (Exception)
            {

                throw;
            }
            return Result.Succeeded;
        }
    }
}
