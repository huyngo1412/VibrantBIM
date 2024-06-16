using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrantBIM.Extensions
{
    public class Convert_Internal_Unit
    {
        /// <summary>
        /// Source Code : https://learnrevitapi.com/python-snippets
        /// </summary>
        /// <param name="value">value to convert</param>
        /// <param name="get_internal">True to get internal units, False to get Meters</param>
        /// <param name="units">Select desired Units: ['m', 'm2']</param>
        /// <returns>Length in Internal units or Meters</returns>
        public double Convert_Internal_Units(double value, bool get_internal = false, string units = "m")
        {
            dynamic UnitType = null;
            if(units == "m") { UnitType = UnitTypeId.Meters; }
            else if(units == "m2") { UnitType = UnitTypeId.SquareMeters; }
            else if(units == "cm") { UnitType = UnitTypeId.Centimeters; }
            if (UnitType == null)
            {
                throw new ArgumentException("Invalid unit type");
            }
            if (get_internal)
                return UnitUtils.ConvertToInternalUnits(value, UnitType);
            return UnitUtils.ConvertFromInternalUnits(value, UnitType);
        }
    }
}
