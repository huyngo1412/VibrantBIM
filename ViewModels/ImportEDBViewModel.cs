﻿using Autodesk.Revit.DB;
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
namespace VibrantBIM.ViewModels
{
    public class ImportEDBViewModel
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
        public ICommand CreateProject { get; set; }
        #endregion
        public ImportEDBViewModel(UIDocument doc, Document getdoc)
        {
            this._uidoc = doc;
            this._document = getdoc;
            _container = new DataContainer();
            CreateProject = new RelayCommand<object>((p) => true, async (p) =>
            {
                if (_importEDBView.chk_GridLine.IsChecked == true)
                {
                    CreateGrid();
                }
                if (_importEDBView.chk_Level.IsChecked == true)
                {
                    CreateLevel();
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
                    var xmlSerializer = new XmlSerializer(typeof(DataContainer));
                    try
                    {
                        using (TextReader reader = new StreamReader(filename))
                        {
                            _container = (DataContainer)xmlSerializer.Deserialize(reader);
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show($"Deserialization error: {ex.Message}");
                        if (ex.InnerException != null)
                        {
                            MessageBox.Show($"Inner Exception: {ex.InnerException.Message}");
                        }
                    }
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
                        TaskDialog.Show("Error", Message);
                        if(transaction.HasStarted())
                        {
                            transaction.RollBack();
                        }
                    }
                    catch(Exception ex)
                    {
                        Message = ex.Message;
                        TaskDialog.Show("Error", Message);
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
                        TaskDialog.Show("Error", Message);
                        if (transaction.HasStarted())
                        {
                            transaction.RollBack();
                        }
                    }
                    catch (Exception ex)
                    {
                        Message = ex.Message;
                        TaskDialog.Show("Error", Message);
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