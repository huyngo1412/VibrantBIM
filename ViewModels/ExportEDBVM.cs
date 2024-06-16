using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using ETABSv1;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using System.Xml.Serialization;
using VibrantBIM.Extensions;
using VibrantBIM.Models;
using VibrantBIM.Models.ShapeType;

namespace VibrantBIM.ViewModels
{
    public class DataContainer
    {
        public ObservableCollection<Column> Columns = new ObservableCollection<Column>();
        public ObservableCollection<Beam> Beams = new ObservableCollection<Beam>();
        public ObservableCollection<GridLine> GridLine = new ObservableCollection<GridLine>();
        public ObservableCollection<Story> Stories = new ObservableCollection<Story>();
    }
    public class ExportEDBVM
    {
        #region Etabs
        private cSapModel _SapModel { get; set; }
        private cOAPI _EtabsObject { get; set; }
        internal int ret = -1;
        #endregion
        #region Data Grid ETABS
        private string filename = "";
        private int _numberNames = 1;
        private string[] _gridNames = null;
        private double Xo = 0;
        private double Yo = 0;
        private double RZ = 0;
        private string GridSysType = null;
        private int NumXLines = 0;
        private int NumYLines = 0;
        private string[] GridLineIDX = null;
        private string[] GridLineIDY = null;
        private double[] OrdinateX = null;
        private double[] OrdinateY = null;
        private bool[] VisibleX = null;
        private bool[] VisibleY = null;
        private string[] BubbleLocX = null;
        private string[] BubbleLocY = null;
        #endregion
        #region Data Level ETABS
        private int NumberStories = 0;
        private string[] StoryNames = null;
        private double[] StoryElevations = null;
        private double[] StoryHeights = null;
        private bool[] IsMasterStory = null;
        private string[] SimilarToStory = null;
        private bool[] SpliceAbove = null;
        private double[] SpliceHeight = null;
        #endregion
        #region Thông tin của các element ETABS
        private int NumberNames = 0;
        private string[] FrameName = null;
        private string[] PropName = null;
        private string[] StoryName = null;
        private string[] PointName1 = null;
        private string[] PointName2 = null;
        private double[] Point1X = null;
        private double[] Point1Y = null;
        private double[] Point1Z = null;
        private double[] Point2X = null;
        private double[] Point2Y = null;
        private double[] Point2Z = null;
        private double[] Angle = null;
        private double[] Offset1X = null;
        private double[] Offset2X = null;
        private double[] Offset1Y = null;
        private double[] Offset2Y = null;
        private double[] Offset1Z = null;
        private double[] Offset2Z = null;
        private int[] CardinalPoint = null;
        private string csys = "Global";
        #endregion
        #region Thông tin hình dạng và kiểu kết cấu
        private eFrameDesignOrientation TypeFrame = eFrameDesignOrientation.Null;
        #endregion
        #region Lấy thông tin các tiết diện
        private int NumberNamesProp = 0;
        private string[] SCProName = null;
        #endregion
        #region GetShapeType
        private eFramePropType PropTypeOAPI = eFramePropType.I;
        #endregion
        #region NameCombo 
        private int NumberCombo = 0;
        private string[] NameCombo = null;
        #endregion
        #region FrameForce
        int NumberResults = 0;
        string[] Obj = null;
        double[] ObjSta = null;
        string[] Elm = null;
        double[] ElmSta = null;
        string[] LoadCase = null;
        string[] StepType = null;
        double[] StepNum = null;
        double[] P = null;
        double[] V2 = null;
        double[] V3 = null;
        double[] T = null;
        double[] M2 = null;
        double[] M3 = null;
        #endregion
        DataContainer dataContainer = new DataContainer();
        public ICommand BrowseEDB { get; set; }
        public ICommand ExportEDB { get;set; }
        public ExportEDBVM(ref cSapModel _sapModel, ref cOAPI _etabsObject) {
            this._SapModel = _sapModel;
            this._EtabsObject = _etabsObject;
            string savedFilePath = "";
            ret = _SapModel.RespCombo.GetNameList(ref NumberCombo, ref NameCombo);
            ret = _SapModel.SetPresentUnits(eUnits.kN_mm_C);
            LoadDistributed();
            BrowseEDB = new RelayCommand<object>((p) => true, (p) =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = "C:\\";
                saveFileDialog.Filter = "Text files (*.cxv)|*.cxv|All files (*.*)|*.*";
                saveFileDialog.Title = "Save your file";
                if (saveFileDialog.ShowDialog() == true)
                {
                    savedFilePath = saveFileDialog.FileName;
                    CXVCruid.FilePathCXV = saveFileDialog.FileName;
                    ret = _SapModel.FrameObj.GetAllFrames(ref NumberNames, ref FrameName, ref PropName, ref StoryName, ref PointName1, ref PointName2,
                         ref Point1X, ref Point1Y, ref Point1Z, ref Point2X, ref Point2Y, ref Point2Z, ref Angle,
                         ref Offset1X, ref Offset2X, ref Offset1Y, ref Offset2Y, ref Offset1Z, ref Offset2Z, ref CardinalPoint, csys);
                    ret = _SapModel.GridSys.GetNameList(ref _numberNames, ref _gridNames);//Lấy thông tin Grid System
                    ret = _SapModel.GridSys.GetGridSys_2(_gridNames.FirstOrDefault(), ref Xo, ref Yo, ref RZ, ref GridSysType, ref NumXLines, ref NumYLines, ref GridLineIDX, ref GridLineIDY, ref OrdinateX, ref OrdinateY,
                       ref VisibleX, ref VisibleY, ref BubbleLocX, ref BubbleLocY);//Lấy hệ lưới XYZ
                    ret = _SapModel.Story.GetStories(ref NumberStories, ref StoryNames, ref StoryElevations, ref StoryHeights, ref IsMasterStory,
                       ref SimilarToStory, ref SpliceAbove, ref SpliceHeight);//Lấy Story Data
                    dataContainer.Columns = new ObservableCollection<Column>(GetColumnEDB().ToList());
                    dataContainer.Beams = new ObservableCollection<Beam>(GetBeamEDB().ToList());
                    dataContainer.GridLine = new ObservableCollection<GridLine>(GetGridData().ToList());
                    dataContainer.Stories = new ObservableCollection<Story>(GetStoryData().ToList());
                    TextBlock textBlock = p as TextBlock;
                    if(textBlock != null )
                    {
                        textBlock.Text = savedFilePath;
                    }    
                }
            });
            ExportEDB = new RelayCommand<object>((p)=>true,(p)=>
            {
                CXVCruid.CreateFile(dataContainer, savedFilePath);
         
            });
        }
        private void LoadDistributed()
        {
            foreach (var loadcase in NameCombo)
            {
                ret = _SapModel.Results.Setup.SetComboSelectedForOutput(loadcase);
            }
            ret = _SapModel.Analyze.RunAnalysis();

        }
        private IEnumerable<GridLine> GetGridData()
        {
            for (int i = 0; i < GridLineIDX.Length; i++)
            {
                GridLine gridLineDX = new GridLine() {
                    Name = GridLineIDX[i],
                    FirstPoint = new Point3D(OrdinateX[i], OrdinateY.Min() - 1500, 0),
                    LastPoint = new Point3D(OrdinateX[i], OrdinateY.Max() + 1500, 0),
                    GridOrientation = "X",      
                };
                yield return gridLineDX;
            }
            for (int i = 0; i < GridLineIDY.Length; i++)
            {
                GridLine gridLineDY = new GridLine()
                {
                    Name = GridLineIDY[i],
                    FirstPoint = new Point3D(OrdinateX.Min() - 1500, OrdinateY[i], 0),
                    LastPoint = new Point3D(OrdinateX.Max() + 1500, OrdinateY[i], 0),
                    GridOrientation = "Y",
                };
                yield return gridLineDY;
            }
        }
        private IEnumerable<Story> GetStoryData()
        {
            for (int i = 0;i < StoryNames.Length; i++ )
            {
                Story story = new Story()
                {
                    StoryName = StoryNames[i],
                    Elevation = StoryElevations[i],
                };
                yield return story;
            }
        }
        private IEnumerable<Column> GetColumnEDB()
        {
            int count = 0;
            for (int i = 0; i < FrameName.Length; i++)
            {
                
                ret = _SapModel.FrameObj.GetDesignOrientation(FrameName[i], ref TypeFrame);
                if (TypeFrame.ToString() == "Column")
                {
                    ret = _SapModel.Results.FrameForce(FrameName[i], eItemTypeElm.ObjectElm, ref NumberResults, ref Obj, ref ObjSta, ref Elm, ref ElmSta, ref LoadCase, ref StepType, ref StepNum, ref P, ref V2, ref V3, ref T, ref M2, ref M3);

                    ret = _SapModel.PropFrame.GetTypeOAPI(PropName[i], ref PropTypeOAPI);
                    Column column = new Column()
                    {
                        Name = FrameName[i],
                        FirstPoint = new Point3D(Point1X[i], Point1Y[i], Point1Z[i]),
                        LastPoint = new Point3D(Point2X[i], Point2Y[i], Point2Z[i]),
                        PropName = PropName[i],
                        StoryName = StoryName[i],
                        FrameShapeType = PropTypeOAPI,
                        ShapeType = Get_SetShapeInstance.SetShapeInstance(PropTypeOAPI),
                        RevitFamily = "",
                        ElementID = "",
                    };
                    column.GetSectionPro(_SapModel, PropName[i]);
                    yield return column;
                }
            }
        }

        private IEnumerable<Beam> GetBeamEDB() 
        {
            for (int i = 0; i < FrameName.Length; i++)
            {
                ret = _SapModel.FrameObj.GetDesignOrientation(FrameName[i], ref TypeFrame);
                if (TypeFrame.ToString() == "Beam" )
                {
                    ret = _SapModel.Results.FrameForce(FrameName[i], eItemTypeElm.ObjectElm, ref NumberResults, ref Obj, ref ObjSta, ref Elm, ref ElmSta, ref LoadCase, ref StepType, ref StepNum, ref P, ref V2, ref V3, ref T, ref M2, ref M3);

                    ret = _SapModel.PropFrame.GetTypeOAPI(PropName[i], ref PropTypeOAPI);
                    Beam beam = new Beam()
                    {
                        Name = FrameName[i],
                        FirstPoint = new Point3D(Point1X[i], Point1Y[i], Point1Z[i]),
                        LastPoint = new Point3D(Point2X[i], Point2Y[i], Point2Z[i]),
                        PropName = PropName[i],
                        StoryName = StoryName[i],
                        FrameShapeType = PropTypeOAPI,
                        ShapeType = Get_SetShapeInstance.SetShapeInstance(PropTypeOAPI),
                        RevitFamily = "",
                        ElementID = "",
                    };
                    beam.GetSectionPro(_SapModel, PropName[i]);
                    yield return beam;
                }
            }
        }
       
    }
}
