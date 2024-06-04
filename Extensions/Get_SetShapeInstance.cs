using Autodesk.Revit.DB.Mechanical;
using ETABSv1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibrantBIM.Models.ShapeType;

namespace VibrantBIM.Extensions
{
    public class Get_SetShapeInstance
    {
        public enum FrameShapeType
        {
            Rectangular,
            I,
        }
        public static object SetShapeInstance(eFramePropType shapeType)
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
