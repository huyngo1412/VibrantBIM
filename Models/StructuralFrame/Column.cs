﻿using Autodesk.Revit.DB;
using ETABSv1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Xml.Serialization;
using VibrantBIM.Extensions;
using VibrantBIM.Models.ShapeType;
using VibrantBIM.ViewModels;

namespace VibrantBIM.Models
{
    [Serializable]
    public class Column : ViewModelBase
    {
        //// Sự kiện PropertyChanged để hỗ trợ việc thông báo thay đổi thuộc tính
        //public event PropertyChangedEventHandler PropertyChanged;

        //// Phương thức để gọi sự kiện PropertyChanged
        //protected void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        //// Thuộc tính Name với thông báo thay đổi
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

    }
}