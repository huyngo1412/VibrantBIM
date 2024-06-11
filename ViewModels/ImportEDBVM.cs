using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ETABSv1;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml.Serialization;
using VibrantBIM.Models;
using VibrantBIM.Views;
using VibrantBIM.Extensions;
using Autodesk.Revit.DB.Structure;
using System.Collections.ObjectModel;
using System.ComponentModel;
namespace VibrantBIM.ViewModels
{
    public class ImportEDBVM
    {
        #region Revit
        private UIDocument _uidoc;
        private Document _document;
        #endregion
        private DataContainer _container;
        private int countLevel { get; set; }
        private int countGrid { get; set; }
        private int countBeam { get; set; }
        private int countColumn { get; set; }
        private int countSlab { get; set; }
      
        private ImportEDBWindow _importEDBView;
        public ImportEDBWindow ImportEDBView
        {
            get
            {
                if (_importEDBView == null)
                {
                    _importEDBView = new ImportEDBWindow() { DataContext = this };
                }
                return _importEDBView;
            }
            set
            {
                _importEDBView = value;
            }
        }
        #region ICommand
        public ICommand ImportEDB { get; set; }
        public ICommand ChangeSectionBeam { get; set; }
        public ICommand ChangeSectionColumn { get;set; }
        public ICommand CreateProject { get; set; }
        #endregion
        public ImportEDBVM(UIDocument doc, Document getdoc)
        {
            this._uidoc = doc;
            this._document = getdoc;
            _container = new DataContainer();
            ChangeSectionBeam = new RelayCommand<object>((p) => true, (p) =>
            {
                var vm = new ChangeSectionVM(doc, getdoc );
                vm.FrameSTBeamView.ShowDialog();
            });
            ChangeSectionColumn = new RelayCommand<object>((p) => true, (p) =>
            {
                var vm = new ChangeSectionVM(doc, getdoc);
                vm.FrameSTColumnView.ShowDialog();
            });
            CreateProject = new RelayCommand<object>((p) => true, async (p) =>
            {
                _container = CXVCruid.ReadFile(CXVCruid.FilePathCXV);
                if (_importEDBView.chk_GridLine.IsChecked == true)
                {
                    CreateGrid();
                }
                if (_importEDBView.chk_Level.IsChecked == true)
                {
                    CreateLevel();
                }
                if(_importEDBView.chk_Beam.IsChecked == true)
                {
                    CreateBeam();
                }
                if (_importEDBView.chk_Column.IsChecked == true)
                {
                    CreateColumn();
                }
            });
            ImportEDB = new RelayCommand<object>((p) => true, p =>
            {
                OpenFileDialog OpenEDB = new OpenFileDialog();
                OpenEDB.InitialDirectory = "C:\\";
                OpenEDB.Filter = " (*.cxv)|*.cxv|All Files (*.*)|*.*";
                OpenEDB.Multiselect = true;
                if (OpenEDB.ShowDialog() == true)
                {
                    string filename = OpenEDB.FileName;               
                    _container = CXVCruid.ReadFile(filename);
                    countBeam = _container.Beams.Count();
                    countColumn = _container.Columns.Count();
                    countGrid = _container.GridLine.Count();
                    countLevel = _container.Stories.Count;
                    _importEDBView.tb_PathEDB.Text = filename;
                    _importEDBView.chk_Beam.Content = _importEDBView.chk_Beam.Content + " " + "(" + countBeam.ToString() + ")";
                    _importEDBView.chk_Column.Content = _importEDBView.chk_Column.Content + " " + "(" + countColumn.ToString() + ")";
                    _importEDBView.chk_GridLine.Content = _importEDBView.chk_GridLine.Content + " " + "(" + countGrid.ToString() + ")";
                    _importEDBView.chk_Level.Content = _importEDBView.chk_Level.Content + " " + "(" + countLevel.ToString() + ")";
                };
            });
        }
        private void CreateLevel()
        {
            string Message = "";
            FilteredElementCollector viewTypesCollector = new FilteredElementCollector(_document).OfClass(typeof(ViewFamilyType));
            List<ViewFamilyType> viewTypes = new List<ViewFamilyType>();
            foreach (Element elem in viewTypesCollector)
            {
                ViewFamilyType viewType = elem as ViewFamilyType;
                viewTypes.Add(viewType);
            }
            FilteredElementCollector filterLevel = new FilteredElementCollector(_document).OfClass(typeof(Level));
            IList<Element> filters = filterLevel.ToList();
            using (Transaction transaction = new Transaction(_document, "Tạo level"))
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
                            throw new Exception("Create a new level failed.");
                        }
                        // Thay đổi tên của level
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
                    catch(Exception ex)
                    {
                        Message = ex.Message;
                        MessageBox.Show("Error : "+ Message);
                        if (transaction.HasStarted())
                        {
                            transaction.RollBack();
                        }
                    }
                }
                transaction.Commit();
            }
        }
        private void CreateGrid()
        {
            string Message = "";
            using (Transaction transaction = new Transaction(_document, "Tạo lưới"))
            {
                transaction.Start();
                foreach (var Grid in _container.GridLine)
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
                            throw new Exception("Create a new straight grid failed.");
                        }
                        lineGrid.Name = Grid.Name;
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
        }
        private void CreateBeam()
        {
            string Message = "";
            Level level = _document.ActiveView.GenLevel;
            FilteredElementCollector collector = new FilteredElementCollector(_document);
            collector.OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructuralFraming);
            using (Transaction transaction = new Transaction(_document, "Tạo dầm"))
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
                        //create a new beam
                        FamilyInstance instance = _document.Create.NewFamilyInstance(beamLine, gotSymbol,
                                                                                    level, StructuralType.Beam);
                        
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
        }
        private void CreateColumn()
        {
            string Message = "";
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
        }
    }
}
