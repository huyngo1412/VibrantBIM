using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using VibrantBIM.Abtract;
using VibrantBIM.ViewModels;
namespace VibrantBIM.Extensions
{
    public class CreateWallEventHandle : IExternalEventHandler, ISetData
    {
        private Document _document;
        private DataContainer _container;
        private static TaskCompletionSource<bool> _taskCompletionSource = new TaskCompletionSource<bool>();
        public static Task<bool> Task => _taskCompletionSource.Task;
        public CreateWallEventHandle(Document document)
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
                collector.OfClass(typeof(WallType)).OfCategory(BuiltInCategory.OST_Walls);
                using (Transaction transaction = new Transaction(_document, "Create Wall"))
                {
                    transaction.Start();
                    for (int i = 0; i < _container.Walls.Count; i++)
                    {
                        List<Curve> profile = new List<Curve>();
                        for (int j = 0; j < _container.Walls[i].Point.Count() - 1; j++)
                        {
                            XYZ FirstPoint1 = new XYZ(_container.Walls[i].Point[j].X, _container.Walls[i].Point[j].Y, _container.Walls[i].Point[j].Z);
                            XYZ LastPoint1 = new XYZ(_container.Walls[i].Point[j + 1].X, _container.Walls[i].Point[j + 1].Y, _container.Walls[i].Point[j+1].Z);
                            profile.Add(Line.CreateBound(ConvertUnit.MmToFoot(FirstPoint1), ConvertUnit.MmToFoot(LastPoint1)));
                        }
                        int count = _container.Walls[i].Point.Count();
                        XYZ FirstPoint2 = new XYZ(_container.Walls[i].Point[count - 1].X, _container.Walls[i].Point[count - 1].Y, _container.Walls[i].Point[count - 1].Z);
                        XYZ LastPoint2 = new XYZ(_container.Walls[i].Point[0].X, _container.Walls[i].Point[0].Y, _container.Walls[i].Point[0].Z);
                        profile.Add(Line.CreateBound(ConvertUnit.MmToFoot(FirstPoint2), ConvertUnit.MmToFoot(LastPoint2)));
                        //MessageBox.Show(profile.Count().ToString());
                        try
                        {

                            Level baseLevel = new FilteredElementCollector(_document).OfClass(typeof(Level)).Cast<Level>().FirstOrDefault(lv => lv.Name.Equals(_container.Walls[i].StoryName));
                            WallType wallType = collector.Where(x => x.Name == _container.Walls[i].RevitFamily).FirstOrDefault() as WallType;
                            Autodesk.Revit.DB.Wall wall = Autodesk.Revit.DB.Wall.Create(_document, profile, wallType.Id, baseLevel.Id, true);
                            Parameter baseOffsetParam = wall.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET);
                            
                            if (baseOffsetParam != null )
                            {
                                baseOffsetParam.Set(ConvertUnit.MmToFoot(0)); // Thiết lập chiều cao của tường (đơn vị là feet hoặc mét, tùy thuộc vào thiết lập Revit)
                            }
                            
                            //XMLCRUID.UpdateFile(ref _xmlDocument, "//Walls/Wall", _container.Walls[i].Name, "Name", "ElementID", wall.Id.ToString());
                        }
                        catch (Autodesk.Revit.Exceptions.ArgumentException exceptionCanceled)
                        {
                            Message = exceptionCanceled.Message;
                            MessageBox.Show("Error" + exceptionCanceled.Message);
                            _taskCompletionSource.SetResult(false);
                            if (transaction.HasStarted())
                            {
                                transaction.RollBack();
                            }
                        }
                        catch (Exception ex)
                        {
                            Message = ex.Message;
                            MessageBox.Show("Error" + ex.Message);
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
                MessageBox.Show("Error", ex.Message);
                _taskCompletionSource.SetResult(false);
            }
        }
        public string GetName()
        {
            return "";
        }
    } 
}
