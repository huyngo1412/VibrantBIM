using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VibrantBIM.Models.ElementOutput
{
    [XmlInclude(typeof(ElementForces))]
    public class ElementForces
    {
        [XmlElement("OutputCase")]
        private string _outputCase;
        public string OutputCase
        {
            get { return _outputCase; }
            set
            {
                _outputCase = value;
            }
        }
        [XmlElement("Station")]
        private double _station;
        public double Station
        {
            get { return _station; }
            set
            {
                _station = value;
            }
        }

        [XmlElement("P")]
        private double _P;
        public double P
        {
            get { return _P; }
            set
            {   
                _P = value;
            }
        }
        [XmlElement("V2")]
        private double _V2;
        public double V2
        {
            get { return _V2; }
            set
            {
                _V2 = value;
            }
        }
        [XmlElement("V3")]
        private double _V3;
        public double V3
        {
            get { return _V3; }
            set
            {
                _V3 = value;
            }
        }
        [XmlElement("M2")]
        private double _M2;
        public double M2
        {
            get { return _M2; }
            set
            {
                _M2 = value;
            }
        }
        [XmlElement("M3")]
        private double _M3;
        public double M3
        {
            get { return _M3; }
            set
            {
                _M3 = value;
            }
        }
    }
}
