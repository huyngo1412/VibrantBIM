using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VibrantBIM.Models.ShapeType
{
    [XmlInclude(typeof(I))]
    public class I
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
        [XmlElement("TotalDepth")]
        private double _totalDepth;
        public double TotalDepth
        {
            get { return _totalDepth; }
            set
            {
                _totalDepth = value;
            }
        }
        [XmlElement("TopFlangeWidth")]
        private double _topFlangeWidth;
        public double TopFlangeWidth
        {
            get { return _topFlangeWidth; }
            set
            {
                _topFlangeWidth = value;
            }
        }
        [XmlElement("TopFlangeThickness")]
        private double _topFlangeThickness;
        public double TopFlangeThickness
        {
            get { return _topFlangeThickness; }
            set
            {
                _topFlangeThickness = value;
            }
        }
        [XmlElement("WebThickness")]
        private double _webThickness;
        public double WebThickness
        {
            get { return _webThickness; }
            set
            {
                _webThickness = value;
            }
        }
        [XmlElement("BottomFlangeWidth")]
        private double _bottomFlangeWidth;
        public double BottomFlangeWidth
        {
            get { return _bottomFlangeWidth; }
            set
            {
                _bottomFlangeWidth = value;
            }
        }
        [XmlElement("BottomFlangeThickness")]
        private double _bottomFlangeThickness;
        public double BottomFlangeThickness
        {
            get { return _bottomFlangeThickness; }
            set
            {
                _bottomFlangeThickness = value;
            }
        }
        [XmlElement("FilletRadius")]
        private double _filletRadius;
        public double FilletRadius
        {
            get { return _filletRadius; }
            set
            {
                _filletRadius = value;
            }
        }
    }
}
