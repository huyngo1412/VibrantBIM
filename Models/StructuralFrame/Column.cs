using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using ETABSv1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Xml.Serialization;
using VibrantBIM.Abtract;
using VibrantBIM.Extensions;
using VibrantBIM.Models.ShapeType;
using VibrantBIM.ViewModels;
using VibrantBIM.Models.ElementOutput;

namespace VibrantBIM.Models
{
    [Serializable]
    [XmlInclude(typeof(Rectangular))]
    [XmlInclude(typeof(I))]
    [XmlInclude(typeof(ElementForces))]
    public class Column : ViewModelBase, IShapeTypeFrame, IRebarShape
    {
        private string filename = "";
        private string Mat = "";
        private double T3 = 0;
        private double T2 = 0;
        private int Color = 1;
        private string Notes = null;
        private string GUID = null;
        private int ret = -1;
        [XmlElement("Name")]
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        [XmlElement("FirstPoint")]
        private Point3D _firstPoint;
        public Point3D FirstPoint
        {
            get { return _firstPoint; }
            set
            {
                _firstPoint = value;
            }
        }
        [XmlElement("LastPoint")]
        private Point3D _lastPoint;
        public Point3D LastPoint
        {
            get { return _lastPoint; }
            set
            {
                _lastPoint = value;
            }
        }
        [XmlElement("PropName")]
        private string _propName;
        public string PropName
        {
            get { return _propName; }
            set
            {
                _propName = value;
                OnPropertyChanged(nameof(PropName));
            }
        }
        [XmlElement("StoryName")]
        private string _storyName;
        public string StoryName
        {
            get { return _storyName; }
            set
            {
                _storyName = value;
                OnPropertyChanged(nameof(StoryName));
            }
        }
        [XmlElement("FrameShapeType")]
        private eFramePropType _frameShapeType;
        public eFramePropType FrameShapeType
        {
            get { return _frameShapeType; }
            set
            {
                _frameShapeType = value;
            }
        }
        [XmlElement("ShapeType")]
        private object _shapeType;
        public object ShapeType
        {
            get { return _shapeType; }
            set
            {
                _shapeType = value;
            }
        }
        [XmlElement("RevitFamily")]
        private string _revitFamily;
        public string RevitFamily
        {
            get { return _revitFamily; }
            set
            {
                _revitFamily = value;
            }
        }
        [XmlElement("ElementID")]
        private string _elementID;
        public string ElementID
        {
            get { return _elementID; }
            set
            {
                _elementID = value;
            }
        }
        [XmlElement("FrameForce")]
        private List<ElementForces> _frameForce;
        public List<ElementForces> FrameForce
        {
            get { return _frameForce; }
            set
            {
                _frameForce = value;
            }
        }

        public void GetSectionPro(cSapModel _SapModel, string ProName)
        {
            if (this.ShapeType is Rectangular)
            {
                Rectangular(_SapModel, ProName);
            }
        }
        public void Rectangular(cSapModel _SapModel, string ProName)
        {
            ret = _SapModel.PropFrame.GetRectangle(ProName, ref filename, ref Mat, ref T3, ref T2, ref Color, ref Notes, ref GUID);
            try
            {
                Rectangular rect = (Rectangular)this.ShapeType;
                rect.FrameShapeName = ProName;
                rect.Width = T2;
                rect.Height = T3;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.ToString());
                return;
            }
        }
    }
}
