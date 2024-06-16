using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using ETABSv1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VibrantBIM.Models;
using VibrantBIM.Models.ShapeType;

namespace VibrantBIM.Extensions
{
    public static class Get_SetShapeInstance
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shapeType"></param>
        /// <returns></returns>
        public static  object SetShapeInstance(eFramePropType shapeType)
        {
            switch (shapeType)
            {
                case eFramePropType.Rectangular:
                    return new Rectangular();
                case eFramePropType.I:
                    return new I();
            }
            return null;
        }
    }
}
