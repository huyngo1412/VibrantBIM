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
                RibbonPanel panelEtabs_SapModel = application.CreateRibbonPanel("VibrantBIMExtension", "Etabs/Sap2000 Model");
                PushButtonData btnImportEDB = new PushButtonData(
                    "Import Model ETABS",
                    "Import Model ETABS",
                    Assembly.GetExecutingAssembly().Location, "VibrantBIM.CreateModelRevit")
                {
                   
                };
                PushButton pushbtnImportEDB = panelEtabs_SapModel.AddItem(btnImportEDB) as PushButton;
                pushbtnImportEDB.Enabled = true;

                PushButtonData btnFrameForces = new PushButtonData(
                    "Frame Force",
                    "Frame Force",
                    Assembly.GetExecutingAssembly().Location, "VibrantBIM.FrameForceTable")
                {

                };
                PushButton pushbtnFrameForces = panelEtabs_SapModel.AddItem(btnFrameForces) as PushButton;

            }
            catch (Exception)
            {

                throw;
            }
            return Result.Succeeded;
        }
    }
}
