using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VibrantBIM.ViewModels;
using System.Windows.Documents;
using System.Windows.Controls;
using VibrantBIM.Models.AreaDesignOrientation;
using VibrantBIM.Abtract;
using System.Windows;

namespace VibrantBIM.Extensions
{
    public class CreateFloorEventHandle : IExternalEventHandler, ISetData
    {
        private Document _document;
        private DataContainer _container;
        private static TaskCompletionSource<bool> _taskCompletionSource = new TaskCompletionSource<bool>();
        public static Task<bool> Task => _taskCompletionSource.Task;
        public CreateFloorEventHandle(Document document)
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
                FilteredElementCollector collector = new FilteredElementCollector(_document);
                collector.OfClass(typeof(FloorType)).OfCategory(BuiltInCategory.OST_Floors);

                FilteredElementCollector colLev = new FilteredElementCollector(_document).OfClass(typeof(Level));


                using (Transaction transaction = new Transaction(_document, "Create Floor"))
                {
                    transaction.Start();
                    for (int i = 0; i < _container.Floors.Count; i++)
                    {
                        CurveLoop profile = new CurveLoop();
                        for (int j = 0; j < _container.Floors[i].Point.Count() - 1 ; j++)
                        {
                            XYZ FirstPoint1 = new XYZ(_container.Floors[i].Point[j].X, _container.Floors[i].Point[j].Y, _container.Floors[i].Point[j].Z);
                            XYZ LastPoint1 = new XYZ(_container.Floors[i].Point[j+1].X, _container.Floors[i].Point[j+1].Y, _container.Floors[i].Point[j+1].Z);
                            profile.Append(Line.CreateBound(ConvertUnit.MmToFoot(FirstPoint1), ConvertUnit.MmToFoot(LastPoint1)));
                        }
                        int count = _container.Floors[i].Point.Count();
                        XYZ FirstPoint2 = new XYZ(_container.Floors[i].Point[count-1].X, _container.Floors[i].Point[count-1].Y, _container.Floors[i].Point[count-1].Z);
                        XYZ LastPoint2 = new XYZ(_container.Floors[i].Point[0].X, _container.Floors[i].Point[0].Y, _container.Floors[i].Point[0].Z);
                        profile.Append(Line.CreateBound(ConvertUnit.MmToFoot(FirstPoint2), ConvertUnit.MmToFoot(LastPoint2)));  
                        try
                        {
                            Level level = colLev.Where(x => x.Name == _container.Floors[i].StoryName).FirstOrDefault() as Level;
                            if(level == null)
                            {
                                MessageBox.Show("Sai rồi");
                            }
                            FloorType floorType = collector.Where(x => x.Name == _container.Floors[i].RevitFamily).FirstOrDefault() as FloorType;
                            Autodesk.Revit.DB.Floor floor = Autodesk.Revit.DB.Floor.Create(_document, new List<CurveLoop> { profile }, floorType.Id, level.Id);
                            //XMLCRUID.UpdateFile(ref _xmlDocument, "//Floors/Floor", _container.Floors[i].Name, "Name", "ElementID", floor.Id.ToString());
                        }
                        catch (Autodesk.Revit.Exceptions.ArgumentException exceptionCanceled)
                        {
                            Message = exceptionCanceled.Message;
                            MessageBox.Show("Error", exceptionCanceled.Message);
                            _taskCompletionSource.SetResult(false);
                            if (transaction.HasStarted())
                            {
                                transaction.RollBack();
                            }
                        }
                        catch (Exception ex)
                        {
                            Message = ex.Message;
                            MessageBox.Show("Error", ex.Message);
                            _taskCompletionSource.SetResult(false);
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
