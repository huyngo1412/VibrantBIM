using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VibrantBIM.Models.ShapeType
{
    [XmlInclude(typeof(Rectangular))]
    public struct Rectangular
    {
        [XmlElement("ShapeName")]
        private string _frameShapeName;
        public string FrameShapeName
        {
            get { return _frameShapeName; }
            set
            {
                _frameShapeName = value;
            }
        }
        [XmlElement("Width")]
        private double _width;
        public double Width 
        {
            get { return _width; }
            set
            {
                _width = value;
            }
        }
        [XmlElement("Height")]
        private double _height;
        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
            }
        }
    }
}
