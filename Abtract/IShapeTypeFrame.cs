using ETABSv1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibrantBIM.Models.ShapeType;
using VibrantBIM.Models;

namespace VibrantBIM.Abtract
{
    public interface IShapeTypeFrame
    {
        void GetSection(cSapModel _SapModel, string ProName);
        void Rectangular(cSapModel _SapModel, string ProName);
        //void GetMaterial();
    }
}
