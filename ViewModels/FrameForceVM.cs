using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VibrantBIM.Extensions;
using VibrantBIM.Models.ElementOutput;
using VibrantBIM.Views;
using VibrantBIM.Models;

namespace VibrantBIM.ViewModels
{
    public class DisplayFrameForceData
    {
        public string Name {  get; set; }
        public string OutputCase { get; set; }
        public double EleStation { get; set; }
        public double V2 { get; set; }
        public double V3 { get; set; }
        public double M2 { get; set; }
        public double M3 { get; set; }
    }
    public  class FrameForceVM
    {
        public DataContainer _container;
        private ObservableCollection<DisplayFrameForceData> _beams  = new ObservableCollection<DisplayFrameForceData>();
        public ObservableCollection<DisplayFrameForceData> Beams
        {
            get { return _beams; }
            set
            {
                _beams = value;
            }
        }
        private FrameForceDataWindow _frameForceDataView;
        public FrameForceDataWindow FrameForceDataView
        {
            get
            {
                if (_frameForceDataView == null)
                {
                    _frameForceDataView = new FrameForceDataWindow() { DataContext = this };
                }
                return _frameForceDataView;
            }
            set
            {
                _frameForceDataView = value;
            }
        }
        public FrameForceVM()
        {
            _container = new DataContainer();
            _container = CXVCruid.ReadFile("C:\\Users\\ADMIN\\Documents\\5.cxv");
            foreach (var beamitem in _container.Columns)
            {
                foreach (var frameforceitem in beamitem.FrameForce)
                {
                    DisplayFrameForceData data = new DisplayFrameForceData()
                    {
                        Name = beamitem.Name,
                        OutputCase = frameforceitem.OutputCase,
                        EleStation = frameforceitem.Station,
                        V2 = frameforceitem.V2,
                        V3 = frameforceitem.V3,
                        M2 = frameforceitem.M2,
                        M3 = frameforceitem.M3,
                    };
                    //MessageBox.Show(beamitem.Name);
                    _beams.Add(data);
                }
            }
            FrameForceDataView.Dg_FrameForceData.ItemsSource = Beams;
            
        }
    }
}
