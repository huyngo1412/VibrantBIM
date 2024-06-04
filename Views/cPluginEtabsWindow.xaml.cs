using ETABSv1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VibrantBIM.ViewModels;

namespace VibrantBIM.Views
{
    /// <summary>
    /// Interaction logic for cPluginEtabsWindow.xaml
    /// </summary>
    public partial class cPluginEtabsWindow : Window
    {
        private cPluginCallback _Plugin = null;
        private cSapModel _SapModel = null;
        private cOAPI _EtabsObject = null;
        private ExportEDBViewModel _ExportEDBViewModel {  get; set; }
        public cPluginEtabsWindow(ref cSapModel SapModel, ref cPluginCallback Plugin)
        {
            _Plugin = Plugin;
            _SapModel = SapModel;
            cHelper myHelper = new Helper();
            _EtabsObject = myHelper.CreateObjectProgID("CSI.ETABS.API.ETABSObject");
            try
            {
                this.DataContext = _ExportEDBViewModel = new ExportEDBViewModel(ref _SapModel, ref _EtabsObject);
            }
            catch (Exception ex)
            {

            }
            InitializeComponent();
        }

        private void cPluginEtabs_Closed(object sender, EventArgs e)
        {
            _Plugin.Finish(0);
        }

        private void btn_Export_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
