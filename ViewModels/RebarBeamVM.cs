using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibrantBIM.Extensions;
using VibrantBIM.Models.ElementOutput;
using VibrantBIM.Views;

namespace VibrantBIM.ViewModels
{
    public class RebarBeamVM
    {
        #region Revit
        private UIDocument _uidoc;
        private Document _document;
        DataContainer dataContainer = new DataContainer();
        #endregion
        private int[] _rebardimension = new int[] { 2,4,6,8,10,12,14,16,18,20,22,25,28,30};
        public int[] Rebardimension
        {
            get { return _rebardimension; }
            set { _rebardimension = value;}
        }
        private List<string> _loadCombo = new List<string>();
        public List<string> LoadCombo
        {
            get { return _loadCombo; }
            set { _loadCombo = value; }
        }

        private List<RebarShape> _rebarShapes = new List<RebarShape>();

        private List<string> _rebarShapeName = new List<string>();
        public List<string> RebarShapeName
        {
            get { return _rebarShapeName; }
            set { _rebarShapeName = value; }
        }
        private SteelBeamWindow _steelBeamView;
        public SteelBeamWindow SteelBeamView
        {
            get
            {
                if (_steelBeamView == null)
                {
                    _steelBeamView = new SteelBeamWindow() { DataContext = this };
                }
                return _steelBeamView;
            }
            set
            {
                _steelBeamView = value;
            }
        }
        public RebarBeamVM(UIDocument doc, Document getdoc)
        {
            this._uidoc = doc;
            this._document = getdoc;
            dataContainer = CXVCruid.ReadFile("C:\\Users\\ADMIN\\Documents\\99.cxv");
            LoadComboName();
            FilteredElementCollector ColRebarShape = new FilteredElementCollector(_document).OfClass(typeof(RebarShape));
            _rebarShapes = ColRebarShape.Cast<RebarShape>().ToList();
            foreach (var shape in _rebarShapes)
            {
                _rebarShapeName.Add(shape.Name);
            }    
        }
        private void LoadComboName()
        {
            List<ElementForces> elementForces = new List<ElementForces>();
            elementForces = dataContainer.Beams[0].FrameForce;
            foreach (var element in elementForces)
            {
                if(!_loadCombo.Contains(element.OutputCase))
                {
                    _loadCombo.Add(element.OutputCase);
                }    
            }
        }
    }
}
