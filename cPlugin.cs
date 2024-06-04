using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibrantBIM.Views;
namespace VibrantBIM
{
    public class cPlugin
    {
        public void Main(ref ETABSv1.cSapModel SapModel, ref ETABSv1.cPluginCallback ISapPlugin)
        {
            cPluginEtabsWindow pluginEtabsWindow = new cPluginEtabsWindow(ref SapModel, ref ISapPlugin);
            pluginEtabsWindow.Show();
        }

        public long Info(ref string Text)
        {
            Text = "my first plugin using API Etabs\n";
            return 0;
        }
    }
}
