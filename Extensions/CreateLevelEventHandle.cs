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
    public class CreateLevelEventHandle : IExternalEventHandler, ISetData
    {
        private Document _document;
        private DataContainer _container;
        private static TaskCompletionSource<bool> _taskCompletionSource = new TaskCompletionSource<bool>();

        public static Task<bool> Task => _taskCompletionSource.Task;
        public CreateLevelEventHandle(Document document)
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
                FilteredElementCollector viewTypesCollector = new FilteredElementCollector(_document).OfClass(typeof(ViewFamilyType));
                List<ViewFamilyType> viewTypes = new List<ViewFamilyType>();
                foreach (Element elem in viewTypesCollector)
                {
                    ViewFamilyType viewType = elem as ViewFamilyType;
                    viewTypes.Add(viewType);
                }
                FilteredElementCollector filterLevel = new FilteredElementCollector(_document).OfClass(typeof(Level));
                IList<Element> filters = filterLevel.ToList();
                using (Transaction transaction = new Transaction(_document, "Create Level"))
                {
                    transaction.Start();
                    for (int i = 0; i < _container.Stories.Count; i++)
                    {
                        if (i < filters.Count())
                        {
                            Element elem = filters[i];
                            Parameter parametername = elem.LookupParameter("Name");
                            parametername.Set(_container.Stories[i].StoryName);
                            Parameter paraelevation = elem.LookupParameter("Elevation");
                            paraelevation.Set(ConvertUnit.MmToFoot(_container.Stories[i].Elevation));
                            continue;
                        }
                        try
                        {
                            Level level = Level.Create(_document, ConvertUnit.MmToFoot(_container.Stories[i].Elevation));
                            if (null == level)
                            {
                                throw new Exception("Story creation failed.");
                            }
                            level.Name = _container.Stories[i].StoryName;
                            foreach (ViewFamilyType viewType in viewTypes)
                            {
                                if (viewType.ViewFamily == ViewFamily.FloorPlan || viewType.ViewFamily == ViewFamily.CeilingPlan)
                                {
                                    ViewPlan view = ViewPlan.Create(_document, viewType.Id, level.Id);
                                }
                            }
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
                        catch (Exception ex)
                        {
                            Message = ex.Message;
                            MessageBox.Show("Error : " + Message);
                            if (transaction.HasStarted())
                            {
                                transaction.RollBack();
                            }
                        }
                    }
                    transaction.Commit();
                }
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
