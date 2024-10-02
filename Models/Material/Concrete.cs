using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VibrantBIM.ViewModels;

namespace VibrantBIM.Models.Material
{
    [XmlInclude(typeof(Concrete))]
    public class Concrete : ViewModelBase
    {
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
    }
}
