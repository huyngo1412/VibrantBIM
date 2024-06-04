using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VibrantBIM.Models
{
    public class Story
    {
        [XmlElement("StoryName")]
        private string _StoryName;
        public string StoryName
        {
            get { return _StoryName; }
            set
            {
                _StoryName = value;
            }
        }
        [XmlElement("FirstPoint")]
        private double _Elevation;
        public double Elevation
        {
            get { return _Elevation; }
            set
            {
                _Elevation = value;
            }
        }
    }
}
