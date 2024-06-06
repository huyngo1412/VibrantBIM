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
    public class ChangeSectionBeamVM
    {
        private UIDocument _uidoc;
        private Document _document;
        private string _filename;
        private FrameSTBeamWindow _frameSTBeamView;
        private DataContainer _dataContainer;
        private List<FrameST> _lstframeST = new List<FrameST>();
        public List<FrameST> LstFrameST
        {
            get
            {
               
                return _lstframeST;
            }
            set
            {
                _lstframeST = value;
            }
        }
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
        public ICommand ChangeSection { get; set; }
        public ChangeSectionBeamVM(UIDocument doc, Document getdoc)
        {
            this._uidoc = doc;
            this._document = getdoc;
            _dataContainer = (DataContainer)CXVCruid.ReadFile(CXVCruid.FilePathCXV);
            FilteredElementCollector collector = new FilteredElementCollector(_document);
            collector.OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_StructuralFraming);
            List<string> revitfamilyname = collector.Select(x=>x.Name).ToList();
            List<string> etabsST = new List<string>();
            foreach (var item in _dataContainer.Beams)
            {
                if(!etabsST.Contains(item.PropName))
                {
                    etabsST.Add(item.PropName);
                }    
            }
            foreach (var item in etabsST)
            {
                FrameST frameST = new FrameST()
                { EtabsSTName = item, RevitFamilyName = revitfamilyname};
                _lstframeST.Add(frameST);
            }
            ChangeSection = new RelayCommand<object>((p) => true, (p) =>
            {
                DataGrid dataGrid = p as DataGrid;
                if(dataGrid != null)
                {
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
                                    }
                                }
                            }
                        }
                    }
                }
            });
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
