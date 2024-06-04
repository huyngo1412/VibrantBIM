using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Xml.Serialization;

namespace VibrantBIM.Models
{
    [Serializable]
    public class GridLine
    {
        [XmlElement("XGridName")]
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }
        [XmlAttribute("GridOrientation")]
        private string _gridOrientation;
        public string GridOrientation
        {
            get { return _gridOrientation; }
            set
            {
                _gridOrientation = value;
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

        // Thuộc tính LastPoint (không cần thông báo thay đổi)
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
    }
}
