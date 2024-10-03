using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using VibrantBIM.Abtract;
using VibrantBIM.ViewModels;

namespace VibrantBIM.Extensions
{
    public class CreateBeamEventHandle : IExternalEventHandler, ISetData
    {
        private Document _document;
        private DataContainer _container;
        private static TaskCompletionSource<bool> _taskCompletionSource = new TaskCompletionSource<bool>();

        public static Task<bool> Task => _taskCompletionSource.Task;
        public CreateBeamEventHandle(Document document)
        {
            _document = document;
        }
        public void SetDataContainer(DataContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }
        public void Execute(UIApplication app)
        {
            string Message = "";
            try
            {
                XmlDocument _xmlDocument = new XmlDocument();
                _xmlDocument.Load(XMLCRUID.FilePathXML);
                Level level = _document.ActiveView.GenLevel;
                FilteredElementCollector collector = new FilteredElementCollector(_document);
                collector.OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructuralFraming);
                using (Transaction transaction = new Transaction(_document, "Create Beam"))
                {
                    transaction.Start();
                    for (int i = 0; i < _container.Beams.Count; i++)
                    {
                        XYZ Start = new XYZ(_container.Beams[i].FirstPoint.X, _container.Beams[i].FirstPoint.Y, _container.Beams[i].FirstPoint.Z);
                        XYZ End = new XYZ(_container.Beams[i].LastPoint.X, _container.Beams[i].LastPoint.Y, _container.Beams[i].LastPoint.Z);
                        Autodesk.Revit.DB.Curve beamLine = Line.CreateBound(ConvertUnit.MmToFoot(Start), ConvertUnit.MmToFoot(End));
                        try
                        {
                            FamilySymbol gotSymbol = collector.Where(x => x.Name == _container.Beams[i].RevitFamily).FirstOrDefault() as FamilySymbol;
                            gotSymbol.Activate();
                            if(gotSymbol == null)
                            {
                                throw new Exception("Create a new beam (" + _container.Beams[i].Name+ ") false ");
                            }    
                            FamilyInstance instance = _document.Create.NewFamilyInstance(beamLine, gotSymbol,
                                                                                        level, StructuralType.Beam);
                            //XMLCRUID.UpdateFile(ref _xmlDocument, "//Beams/Beam", _container.Beams[i].Name, "Name", "ElementID", instance.Id.ToString());
                        }
                        catch (Autodesk.Revit.Exceptions.ArgumentException exceptionCanceled)
                        {
                            Message = exceptionCanceled.Message;
                            MessageBox.Show("Error : " + Message);
                            if (transaction.HasStarted())
                            {
                                transaction.RollBack();
                            }
                        }
                    }
                    transaction.Commit();
                }
                _xmlDocument.Save(XMLCRUID.FilePathXML);
                _taskCompletionSource.SetResult(true);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                _taskCompletionSource.SetResult(false);
            }
        }

        public string GetName()
        {
            return "";
        }
    }
}
