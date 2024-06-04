using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibrantBIM.ViewModels;
using Autodesk.Revit.DB;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Windows.Media.Media3D;
using ETABSv1;
using VibrantBIM.Models.ShapeType;
using VibrantBIM.Extensions;
using VibrantBIM.Views;

namespace VibrantBIM.Models
{
    [Serializable]
    [XmlInclude(typeof(Rectangular))]
    public class Beam  : INotifyPropertyChanged  
    { 
        // Sự kiện PropertyChanged để hỗ trợ việc thông báo thay đổi thuộc tính
        public event PropertyChangedEventHandler PropertyChanged;

        // Phương thức để gọi sự kiện PropertyChanged
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Thuộc tính Name với thông báo thay đổi
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

        // Thuộc tính FirstPoint (không cần thông báo thay đổi)
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

        // Thuộc tính PropName với thông báo thay đổi
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

        // Thuộc tính StoryName với thông báo thay đổi
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
        
    }
}
