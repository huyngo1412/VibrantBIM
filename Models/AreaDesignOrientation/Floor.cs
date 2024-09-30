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
        [XmlElement("MaterialName")]
        private string _materialName;
        public string MaterialName
        {
            get { return _materialName; }
            set
            {
                _materialName = value;
                OnPropertyChanged(nameof(_materialName));
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
    }
}
