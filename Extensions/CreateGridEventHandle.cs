using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VibrantBIM.Abtract;
using VibrantBIM.ViewModels;

namespace VibrantBIM.Extensions
{
    public class CreateGridEventHandle : IExternalEventHandler, ISetData
    {
        private Document _document;
        private DataContainer _container;
        private static TaskCompletionSource<bool> _taskCompletionSource = new TaskCompletionSource<bool>();
        public static Task<bool> Task => _taskCompletionSource.Task;
        public CreateGridEventHandle(Document document)
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
                using (Transaction trans = new Transaction(_document, "Create Grid"))
                {
                    trans.Start();
                    foreach (var Grid in _container.GridLines)
                    {
                        double X1 = Grid.FirstPoint.X;
                        double Y1 = Grid.FirstPoint.Y;
                        double Z1 = Grid.FirstPoint.Z;
                        double X2 = Grid.LastPoint.X;
                        double Y2 = Grid.LastPoint.Y;
                        double Z2 = Grid.LastPoint.Z;
                        try
                        {
                            XYZ start = new XYZ(ConvertUnit.MmToFoot(X1), ConvertUnit.MmToFoot(Y1), ConvertUnit.MmToFoot(Z1));
                            XYZ end = new XYZ(ConvertUnit.MmToFoot(X2), ConvertUnit.MmToFoot(Y2), ConvertUnit.MmToFoot(Z2));
                            Line geomLine = Line.CreateBound(start, end);
                            Autodesk.Revit.DB.Grid lineGrid = Autodesk.Revit.DB.Grid.Create(_document, geomLine);
                            if (null == lineGrid)
                            {
                                throw new Exception("Grid Line creation failed");
                            }
                            lineGrid.Name = Grid.Name;
                        }
                        catch (Autodesk.Revit.Exceptions.ArgumentException exceptionCanceled)
                        {
                            Message = exceptionCanceled.Message;
                            MessageBox.Show("Error : " + Message);
                            if (trans.HasStarted())
                            {
                                trans.RollBack();
                            }
                        }
                        catch (Exception ex)
                        {
                            Message = ex.Message;
                            MessageBox.Show("Error : " + Message);
                            if (trans.HasStarted())
                            {
                                trans.RollBack();
                            }
                        }
                    }
                    trans.Commit();
                }
                _taskCompletionSource.SetResult(true);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                MessageBox.Show("Error : " + Message);
                _taskCompletionSource.SetResult(false);
            }
        }
        public string GetName()
        {
            return "";
        }
    }
}
