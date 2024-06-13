using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VibrantBIM.ViewModels;

namespace VibrantBIM.Extensions
{
    public class CreateColumnEventHandle : IExternalEventHandler
    {
        private Document _document;
        private DataContainer _container;
        private static TaskCompletionSource<bool> _taskCompletionSource = new TaskCompletionSource<bool>();

        public static Task<bool> Task => _taskCompletionSource.Task;
        public CreateColumnEventHandle(Document document)
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
                _xmlDocument.Load(CXVCruid.FilePathCXV);
                FilteredElementCollector collector = new FilteredElementCollector(_document);
                collector.OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructuralColumns);

                FilteredElementCollector colLev = new FilteredElementCollector(_document);
                colLev.WhereElementIsNotElementType().OfCategory(BuiltInCategory.INVALID).OfClass(typeof(Level));
                List<Level> levels = new List<Level>();

                foreach (Element item in colLev)
                {
                    Level itemLevel = item as Level;
                    if (itemLevel != null)
                    {
                        levels.Add(itemLevel);
                    }
                }
                using (Transaction transaction = new Transaction(_document, "Tạo cột"))
                {
                    transaction.Start();
                    for (int i = 0; i < _container.Columns.Count; i++)
                    {

                        XYZ Start = new XYZ(_container.Columns[i].FirstPoint.X, _container.Columns[i].FirstPoint.Y, _container.Columns[i].FirstPoint.Z);
                        XYZ End = new XYZ(_container.Columns[i].LastPoint.X, _container.Columns[i].LastPoint.Y, _container.Columns[i].LastPoint.Z);
                        Autodesk.Revit.DB.Line columnLine = Line.CreateBound(ConvertUnit.MmToFoot(Start), ConvertUnit.MmToFoot(End));
                        Level level = levels.Where(x => x.Name == _container.Columns[i].StoryName.ToString()).FirstOrDefault();
                        try
                        {
                            FamilySymbol gotSymbol = collector.Where(x => x.Name == _container.Columns[i].RevitFamily).FirstOrDefault() as FamilySymbol;
                            gotSymbol.Activate();

                            FamilyInstance instance = _document.Create.NewFamilyInstance(columnLine, gotSymbol,
                                                                                        level, StructuralType.Column);
                            CXVCruid.UpdateFile(ref _xmlDocument, "//Columns/Column", _container.Columns[i].Name, "Name", "ElementID", instance.Id.ToString());
                        }
                        catch (Autodesk.Revit.Exceptions.ArgumentException exceptionCanceled)
                        {
                            Message = exceptionCanceled.Message;

                            if (transaction.HasStarted())
                            {
                                transaction.RollBack();
                            }
                        }

                    }
                    transaction.Commit();
                }
                _xmlDocument.Save(CXVCruid.FilePathCXV);
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
