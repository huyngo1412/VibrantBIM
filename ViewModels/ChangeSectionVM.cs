using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using VibrantBIM.Extensions;
using VibrantBIM.Views;

namespace VibrantBIM.ViewModels
{
    public class FrameST
    {
        private string _etabsSTName;
        public string EtabsSTName
        {
            get { return _etabsSTName; }
            set
            {
                _etabsSTName = value;
            }
        }
        private List<string> _revitFamilyName;
        public List<string> RevitFamilyName
        {
            get { return _revitFamilyName; }
            set
            {
                _revitFamilyName = value;
            }
        }
    }
    public class ChangeSectionVM
    {
        private UIDocument _uidoc;
        private Document _document;
        private string _filename;
        private DataContainer _dataContainer;
        private List<FrameST> _lstframeSTBeam = new List<FrameST>();
        public List<FrameST> LstframeSTBeam
        {
            get
            {
                return _lstframeSTBeam;
            }
            set
            {
                _lstframeSTBeam = value;
            }
        }
        private List<FrameST> _lstframeSTColumn = new List<FrameST>();
        public List<FrameST> LstframeSTColumn
        {
            get
            {
                return _lstframeSTColumn;
            }
            set
            {
                _lstframeSTColumn = value;
            }
        }
        private FrameSTBeamWindow _frameSTBeamView;
        public FrameSTBeamWindow FrameSTBeamView
        {
            get
            {
                if (_frameSTBeamView == null)
                {
                    _frameSTBeamView = new FrameSTBeamWindow() { DataContext = this };
                }
                return _frameSTBeamView;
            }
            set
            {
                _frameSTBeamView = value;
            }
        }
        private FrameSTColumnWindow _frameSTColumnView;
        public FrameSTColumnWindow FrameSTColumnView
        {
            get
            {
                if (_frameSTColumnView == null)
                {
                    _frameSTColumnView = new FrameSTColumnWindow() { DataContext = this };
                }
                return _frameSTColumnView;
            }
            set
            {
                _frameSTColumnView = value;
            }
        }

        public ICommand ChangeSectionBeam { get; set; }
        public ICommand ChangeSectionColumn { get; set; }
        public ChangeSectionVM(UIDocument doc, Document getdoc)
        {
            XmlDocument xmldoc = new XmlDocument();
            this._uidoc = doc;
            this._document = getdoc;
            _dataContainer = (DataContainer)XMLCRUID.ReadFile(XMLCRUID.FilePathXML);
            //Lọc dữ liệu dầm
            FilteredElementCollector collectorBeam = new FilteredElementCollector(_document);
            collectorBeam.OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructuralFraming);
            List<string> Beamrevitfamilyname = collectorBeam.Select(x=>x.Name).ToList();
            List<string> etabsSTBeam = new List<string>();
            foreach (var item in _dataContainer.Beams)
            {
                if(!etabsSTBeam.Contains(item.PropName))
                {
                    etabsSTBeam.Add(item.PropName);
                }    
            }
            foreach (var item in etabsSTBeam)
            {
                FrameST frameST = new FrameST()
                { EtabsSTName = item, RevitFamilyName = Beamrevitfamilyname };
                _lstframeSTBeam.Add(frameST);
            }
            //Lọc dữ liệu cột
            FilteredElementCollector collectorColumn = new FilteredElementCollector(_document);
            collectorColumn.OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructuralColumns);
            List<string> Columnrevitfamilyname = collectorColumn.Select(x => x.Name).ToList();
            List<string> etabsSTColumn = new List<string>();
            foreach (var item in _dataContainer.Columns)
            {
                if (!etabsSTColumn.Contains(item.PropName))
                {
                    etabsSTColumn.Add(item.PropName);
                }
            }
            foreach (var item in etabsSTColumn)
            {
                FrameST frameST = new FrameST()
                { EtabsSTName = item, RevitFamilyName = Columnrevitfamilyname };
                _lstframeSTColumn.Add(frameST);
            }
            ChangeSectionBeam = new RelayCommand<object>((p) => true, (p) =>
            {
                DataGrid dataGrid = p as DataGrid;
                if(dataGrid != null)
                {
                    UpdateSection(dataGrid, "Beam");
                }
            });
            ChangeSectionColumn= new RelayCommand<object>((p) => true, (p) =>
            {
                DataGrid dataGrid = p as DataGrid;
                if (dataGrid != null)
                {
                    UpdateSection(dataGrid, "Column");
                }
            });
        }
        private void UpdateSection(DataGrid dataGrid, string TypeFrame)
        {
            string NodePath = "";
            if (TypeFrame.ToLower() == "beam")
                NodePath = "//Beams/Beam";
            if(TypeFrame.ToLower() == "column")
                NodePath = "//Columns/Column";
            if (TypeFrame.ToLower() == "floor")
                NodePath = "//Floors/Floor";
            if (TypeFrame.ToLower() == "wall")
                NodePath = "//Walls/Wall";
            XmlDocument _xmlDocument = new XmlDocument();
            _xmlDocument.Load(XMLCRUID.FilePathXML);
            foreach (var item in dataGrid.Items)
            {
                DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(item);
                if (row == null)
                {
                    dataGrid.UpdateLayout();
                    dataGrid.ScrollIntoView(item);
                    row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(item);
                }
                if (row != null)
                {
                    // Lấy cột chứa ComboBox (cột thứ hai)
                    DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);
                    if (presenter != null)
                    {
                        DataGridCell cellEtabsSTName = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(0); // Chỉ số cột chứa ComboBox
                        DataGridCell cellFamilyName = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(1); // Chỉ số cột chứa ComboBox
                        if (cellFamilyName != null && cellFamilyName != null)
                        {
                            System.Windows.Controls.ComboBox comboBox = GetVisualChild<System.Windows.Controls.ComboBox>(cellFamilyName);
                            System.Windows.Controls.TextBlock textBlock = GetVisualChild<System.Windows.Controls.TextBlock>(cellEtabsSTName);
                            if (comboBox != null && textBlock != null)
                            {
                                string RevitFamily = comboBox.SelectedValue.ToString();
                                string EtasbSTName = textBlock.Text;
                                XMLCRUID.UpdateFile(ref _xmlDocument, NodePath, EtasbSTName, "PropName", "RevitFamily", RevitFamily);
                            }
                        }
                    }
                }
            }
            _xmlDocument.Save(XMLCRUID.FilePathXML);
        }
        private T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
