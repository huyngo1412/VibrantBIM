using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Xml.Serialization;
using VibrantBIM.Models.ShapeType;
using VibrantBIM.ViewModels;

namespace VibrantBIM.Models.AreaDesignOrientation
{
    [Serializable]
    public class Floor : ViewModelBase
    {
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
        [XmlElement("Thickness")]
        private double _thickNess;
        public double Thickness
        {
            get { return _thickNess; }
            set
            {
                _thickNess = value;
                OnPropertyChanged(nameof(_thickNess));
            }
        }
        [XmlElement("Point")]
        private Point3D[] _Point;
        public Point3D[] Point
        {
            get { return _Point; }
            set
            {
                _Point = value;
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
    }
}
