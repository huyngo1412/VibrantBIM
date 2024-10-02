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
using System.Xml;
using System.Threading;
using System.Xml.Linq;
namespace VibrantBIM.ViewModels
{
    
    public class ImportModelETABSVM
    {
        #region Revit
        private UIDocument _uidoc;
        private Document _document;
        private UIApplication _uiApp;

        private static ExternalEvent _externalEventGrid;
        private static CreateGridEventHandle _gridEventhandler;

        private static ExternalEvent _externalEventLevel;
        private static CreateLevelEventHandle _levelEventhandler;

        private static ExternalEvent _externalEventColumn;
        private static CreateColumnEventHandle _columnEventhandler;

        private static ExternalEvent _externalEventBeam;
        private static CreateBeamEventHandle _beamEventhandler;
        #endregion
        private DataContainer _container;

        private ImportModelETABSWindow _importModelETABSView;
        public ImportModelETABSWindow ImportModelETABSView
        {
            get
            {
                if (_importModelETABSView == null)
                {
                    _importModelETABSView = new ImportModelETABSWindow() { DataContext = this };
                }
                return _importModelETABSView;
            }
            set
            {
                _importModelETABSView = value;
            }
        }
        #region ICommand
        public ICommand ImportModelETABS { get; set; }
        public ICommand ChangeSectionBeam { get; set; }
        public ICommand ChangeSectionColumn { get;set; }
        public ICommand CreateProject { get; set; }
        #endregion
        public ImportModelETABSVM(UIDocument doc, Document getdoc)
        {
            this._uidoc = doc;
            this._document = getdoc;
            _container = new DataContainer();
            if (_gridEventhandler == null || _externalEventGrid == null)
            {
                _gridEventhandler = new CreateGridEventHandle(_document);
                _externalEventGrid = ExternalEvent.Create(_gridEventhandler);
            }
            if (_levelEventhandler == null || _externalEventLevel == null)
            {
                _levelEventhandler = new CreateLevelEventHandle(_document);
                _externalEventLevel = ExternalEvent.Create(_levelEventhandler);
            }
            if (_columnEventhandler == null || _externalEventColumn == null)
            {
                _columnEventhandler = new CreateColumnEventHandle(_document);
                _externalEventColumn = ExternalEvent.Create(_columnEventhandler);
            }
            if (_beamEventhandler == null || _externalEventBeam == null)
            {
                _beamEventhandler = new CreateBeamEventHandle(_document);
                _externalEventBeam = ExternalEvent.Create(_beamEventhandler);
            }
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
                _importModelETABSView.Close();
                ProgressWindow _loadingView = new ProgressWindow();
                _loadingView.Show();
                _container = CXVCruid.ReadFile(CXVCruid.FilePathCXV);
                if (_importModelETABSView.chk_GridLine.IsChecked == true)
                {
                    _gridEventhandler.SetDataContainer(_container);
                    _externalEventGrid.Raise();
                }
                if (_importModelETABSView.chk_Level.IsChecked == true)
                {
                    _levelEventhandler.SetDataContainer(_container);
                    _externalEventLevel.Raise();
                }
                if (_importModelETABSView.chk_Column.IsChecked == true)
                {
                    _columnEventhandler.SetDataContainer(_container);
                    _externalEventColumn.Raise();
                }
                if (_importModelETABSView.chk_Beam.IsChecked == true)
                {
                    _beamEventhandler.SetDataContainer(_container);
                    _externalEventBeam.Raise();
                }
                await CreateGridEventHandle.Task;
                await CreateLevelEventHandle.Task;
                await CreateColumnEventHandle.Task;
                await CreateBeamEventHandle.Task;
                _loadingView.Close();
                
            });
            ImportModelETABS = new RelayCommand<object>((p) => true, p =>
            {
                OpenFileDialog OpenEDB = new OpenFileDialog();
                OpenEDB.InitialDirectory = "C:\\";
                OpenEDB.Filter = " (*.xml)|*.xml|All Files (*.*)|*.*";
                OpenEDB.Multiselect = true;
                if (OpenEDB.ShowDialog() == true)
                {
                    string filename = OpenEDB.FileName;               
                    _container = CXVCruid.ReadFile(filename);               
                    _importModelETABSView.tb_PathEDB.Text = filename;
                    _importModelETABSView.chk_Beam.Content = _importModelETABSView.chk_Beam.Content + " " + "(" + _container.Beams.Count() + ")";
                    _importModelETABSView.chk_Column.Content = _importModelETABSView.chk_Column.Content + " " + "(" + _container.Columns.Count() + ")";
                    _importModelETABSView.chk_GridLine.Content = _importModelETABSView.chk_GridLine.Content + " " + "(" + _container.GridLines.Count() + ")";
                    _importModelETABSView.chk_Level.Content = _importModelETABSView.chk_Level.Content + " " + "(" + _container.Stories.Count() + ")";
                    _importModelETABSView.chk_Floor.Content = _importModelETABSView.chk_Floor.Content + " " + "(" + _container.Floors.Count() + ")";
                    _importModelETABSView.chk_Wall.Content = _importModelETABSView.chk_Wall.Content + " " + "(" + _container.Walls.Count() + ")";
                };
            });
        }
       
    }
}
