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
using System.Runtime.InteropServices;
using VibrantBIM.Abtract;
namespace VibrantBIM.ViewModels
{
    
    public class ImportModelETABSVM
    {
        #region Revit
        private UIDocument _uidoc;
        private Document _document;
        private UIApplication _uiApp;

        private  ExternalEvent _externalEventGrid;
        private  CreateGridEventHandle _gridEventhandler;

        private  ExternalEvent _externalEventLevel;
        private  CreateLevelEventHandle _levelEventhandler;

        private  ExternalEvent _externalEventColumn;
        private  CreateColumnEventHandle _columnEventhandler;

        private  ExternalEvent _externalEventBeam;
        private  CreateBeamEventHandle _beamEventhandler;

        private  ExternalEvent _externalEventFloor;
        private  CreateFloorEventHandle _floorEventhandler;
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
        public ICommand ChangeSectionFloor { get; set; }
        public ICommand CreateProject { get; set; }
        #endregion
        public ImportModelETABSVM(UIDocument doc, Document getdoc)
        {
            this._uidoc = doc;
            this._document = getdoc;
            _container = new DataContainer();
            //if (_gridEventhandler == null || _externalEventGrid == null)
            //{
            //    _gridEventhandler = new CreateGridEventHandle(_document);
            //    _externalEventGrid = ExternalEvent.Create(_gridEventhandler);
            //}
            //if (_levelEventhandler == null || _externalEventLevel == null)
            //{
            //    _levelEventhandler = new CreateLevelEventHandle(_document);
            //    _externalEventLevel = ExternalEvent.Create(_levelEventhandler);
            //}
            //if (_columnEventhandler == null || _externalEventColumn == null)
            //{
            //    _columnEventhandler = new CreateColumnEventHandle(_document);
            //    _externalEventColumn = ExternalEvent.Create(_columnEventhandler);
            //}
            //if (_beamEventhandler == null || _externalEventBeam == null)
            //{
            //    _beamEventhandler = new CreateBeamEventHandle(_document);
            //    _externalEventBeam = ExternalEvent.Create(_beamEventhandler);
            //}
            //if(_floorEventhandler == null || _externalEventFloor == null)
            //{

            //}    
            EnsureEventHandler(ref _gridEventhandler, ref _externalEventGrid, _document => new CreateGridEventHandle(_document));
            EnsureEventHandler(ref _levelEventhandler, ref _externalEventLevel, _document => new CreateLevelEventHandle(_document));
            EnsureEventHandler(ref _columnEventhandler, ref _externalEventColumn, _document => new CreateColumnEventHandle(_document));
            EnsureEventHandler(ref _beamEventhandler, ref _externalEventBeam, _document => new CreateBeamEventHandle(_document));
            EnsureEventHandler(ref _floorEventhandler, ref _externalEventFloor, _document => new CreateFloorEventHandle(_document));
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
            ChangeSectionFloor = new RelayCommand<object>((p) => true, (p) =>
            {
                var vm = new ChangeSectionVM(doc, getdoc);
                vm.STFloorView.ShowDialog();
            });
            CreateProject = new RelayCommand<object>((p) => true, async (p) =>
            {
                _importModelETABSView.Close();
                ProgressWindow _loadingView = new ProgressWindow();
                _loadingView.Show();
                _container = XMLCRUID.ReadFile(XMLCRUID.FilePathXML);
                //if (_importModelETABSView.chk_GridLine.IsChecked == true)
                //{
                //    _gridEventhandler.SetDataContainer(_container);
                //    _externalEventGrid.Raise();
                //}
                //if (_importModelETABSView.chk_Level.IsChecked == true)
                //{
                //    _levelEventhandler.SetDataContainer(_container);
                //    _externalEventLevel.Raise();
                //}
                //if (_importModelETABSView.chk_Column.IsChecked == true)
                //{
                //    _columnEventhandler.SetDataContainer(_container);
                //    _externalEventColumn.Raise();
                //}
                //if (_importModelETABSView.chk_Beam.IsChecked == true)
                //{
                //    _beamEventhandler.SetDataContainer(_container);
                //    _externalEventBeam.Raise();
                //}
                HandleEvents();
                await CreateGridEventHandle.Task;
                await CreateLevelEventHandle.Task;
                await CreateColumnEventHandle.Task;
                await CreateBeamEventHandle.Task;
                await CreateFloorEventHandle.Task;
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
                    _container = XMLCRUID.ReadFile(filename);               
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
        private void EnsureEventHandler<T>(ref T eventHandler, ref ExternalEvent externalEvent, Func<Document, T> createHandler) where T : IExternalEventHandler
        {
            if (eventHandler == null || externalEvent == null)
            {
                eventHandler = createHandler(_document);
                externalEvent = ExternalEvent.Create(eventHandler);
            }
        }

        private void RaiseEventIfChecked<T>(bool? isChecked, T handler, ref ExternalEvent externalEvent) where T : class, ISetData
        {
            if (isChecked == true)
            {
                handler.SetDataContainer(_container);
                externalEvent.Raise();
            }
        }
        private void HandleEvents()
        {
            RaiseEventIfChecked(_importModelETABSView.chk_GridLine.IsChecked, _gridEventhandler, ref _externalEventGrid);
            RaiseEventIfChecked(_importModelETABSView.chk_Level.IsChecked, _levelEventhandler, ref _externalEventLevel);
            RaiseEventIfChecked(_importModelETABSView.chk_Column.IsChecked, _columnEventhandler, ref _externalEventColumn);
            RaiseEventIfChecked(_importModelETABSView.chk_Beam.IsChecked, _beamEventhandler,ref _externalEventBeam);
            RaiseEventIfChecked(_importModelETABSView.chk_Floor.IsChecked, _floorEventhandler, ref _externalEventFloor);
        }


    }
}
