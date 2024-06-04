using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrantBIM.Extensions
{
    public class ConvertUnit
    {
        private const double _inchToMm = 25.4;
        private const double _footToMm = 12 * _inchToMm;
        private const double _footToMeter = _footToMm * 0.001;
        private const double _sqfToSqm = _footToMeter * _footToMeter;
        private const double _cubicFootToCubicMeter = _footToMeter * _sqfToSqm;
        public static double FootToMm(double length)
        {
            return length * _footToMm;
        }
        public static int FootToMmInt(double length)
        {
            return (int)Math.Round(_footToMm * length,
                MidpointRounding.AwayFromZero);
        }
        public static double FootToMetre(double length)
        {
            return length * _footToMeter;
        }
        public static double MmToFoot(double length)
        {
            return length / _footToMm;
        }
        public static XYZ MmToFoot(XYZ v)
        {
            return v.Divide(_footToMm);
        }
        public static double CubicFootToCubicMeter(double volume)
        {
            return volume * _cubicFootToCubicMeter;
        }

    }
}
