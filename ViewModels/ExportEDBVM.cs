using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using ETABSv1;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
        public ObservableCollection<GridLine> GridLines = new ObservableCollection<GridLine>();
        public ObservableCollection<Story> Stories = new ObservableCollection<Story>();
        public ObservableCollection<Models.AreaDesignOrientation.Floor> Floors = new ObservableCollection<Models.AreaDesignOrientation.Floor>();
        public ObservableCollection<Models.AreaDesignOrientation.Wall> Walls = new ObservableCollection<Models.AreaDesignOrientation.Wall>();
    }
    public class ExportEDBVM
    {
        #region Etabs
        private cSapModel _SapModel { get; set; }
        private cOAPI _EtabsObject { get; set; }
        internal int ret = -1;
        #endregion
        #region Dữ liệu lưới ETABS
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
        #region Dữ liệu các tầng ETABS
        private int NumberStories = 0;
        private string[] StoryNames = null;
        private double[] StoryElevations = null;
        private double[] StoryHeights = null;
        private bool[] IsMasterStory = null;
        private string[] SimilarToStory = null;
        private bool[] SpliceAbove = null;
        private double[] SpliceHeight = null;
        #endregion
        #region Thông tin của các FrameDesignOrientation ETABS
        private int NumberNameFrame = 0;
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
        private eFrameDesignOrientation TypeFrame = eFrameDesignOrientation.Null; // Loại Frame {Beam, Column,...}
        #endregion
        #region Thông tin các AreaDesignOrientation ETABS
        int NumberNameArea = -1;
        string[] AreaName = null;
        eAreaDesignOrientation[] TypeAreaArray = null;
        int NumberBoundaryPts = -1;
        int[] PointDelimiter = null;
        string[] PointNames = null;
        double[] PointX = null;
        double[] PointY = null;
        double[] PointZ = null;
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
        //int NumberResults = 0;
        //string[] Obj = null;
        //double[] ObjSta = null;
        //string[] Elm = null;
        //double[] ElmSta = null;
        //string[] LoadCase = null;
        //string[] StepType = null;
        //double[] StepNum = null;
        //double[] P = null;
        //double[] V2 = null;
        //double[] V3 = null;
        //double[] T = null;
        //double[] M2 = null;
        //double[] M3 = null;
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
                    ret = _SapModel.FrameObj.GetAllFrames(ref NumberNameFrame, ref FrameName, ref PropName, ref StoryName, ref PointName1, ref PointName2,
                         ref Point1X, ref Point1Y, ref Point1Z, ref Point2X, ref Point2Y, ref Point2Z, ref Angle,
                         ref Offset1X, ref Offset2X, ref Offset1Y, ref Offset2Y, ref Offset1Z, ref Offset2Z, ref CardinalPoint, csys);// Lấy toàn bộ thông tin của các đối tượng khung
                    ret = _SapModel.AreaObj.GetAllAreas(ref NumberNameArea, ref AreaName, ref TypeAreaArray, ref NumberBoundaryPts, ref PointDelimiter, 
                        ref PointNames, ref PointX, ref PointY, ref PointZ);// Lấy toàn bộ thông tin các đối tượng Floor, Brace,...
                    ret = _SapModel.GridSys.GetNameList(ref _numberNames, ref _gridNames);//Lấy thông tin Grid System
                    ret = _SapModel.GridSys.GetGridSys_2(_gridNames.FirstOrDefault(), ref Xo, ref Yo, ref RZ, ref GridSysType, ref NumXLines, ref NumYLines, ref GridLineIDX, ref GridLineIDY, ref OrdinateX, ref OrdinateY,
                       ref VisibleX, ref VisibleY, ref BubbleLocX, ref BubbleLocY);//Lấy hệ lưới XYZ
                    ret = _SapModel.Story.GetStories(ref NumberStories, ref StoryNames, ref StoryElevations, ref StoryHeights, ref IsMasterStory,
                       ref SimilarToStory, ref SpliceAbove, ref SpliceHeight);//Lấy dữ liệu tầng
                    dataContainer.Columns = new ObservableCollection<Column>(GetColumnETABS().ToList());
                    dataContainer.Beams = new ObservableCollection<Beam>(GetBeamEDB().ToList());
                    dataContainer.GridLines = new ObservableCollection<GridLine>(GetGridETABS().ToList());
                    dataContainer.Stories = new ObservableCollection<Story>(GetStoryETABS().ToList());
                    dataContainer.Floors = new ObservableCollection<Models.AreaDesignOrientation.Floor>(GetFloorETABS().ToList());
                    dataContainer.Walls = new ObservableCollection<Models.AreaDesignOrientation.Wall>(GetWallETABS().ToList());
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
        private IEnumerable<GridLine> GetGridETABS()
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
        private IEnumerable<Story> GetStoryETABS()
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
        private IEnumerable<Column> GetColumnETABS()
        {
            int count = 0;
            for (int i = 0; i < FrameName.Length; i++)
            {
                
                ret = _SapModel.FrameObj.GetDesignOrientation(FrameName[i], ref TypeFrame);
                if (TypeFrame.ToString() == "Column")
                {
                    //ret = _SapModel.Results.FrameForce(FrameName[i], eItemTypeElm.ObjectElm, ref NumberResults, ref Obj, ref ObjSta, ref Elm, ref ElmSta, ref LoadCase, ref StepType, ref StepNum, ref P, ref V2, ref V3, ref T, ref M2, ref M3);

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
                    column.GetSection(_SapModel, PropName[i]);
                    yield return column;
                }
            }
        }
        private IEnumerable<Models.AreaDesignOrientation.Wall> GetWallETABS()
        {
            
            if(AreaName!=null)
            {
                for (int i = 0; i < AreaName.Length; i++)
                {
                    if (TypeAreaArray[i] == eAreaDesignOrientation.Wall)
                    {
                        eWallPropType WallType = eWallPropType.Specified;
                        eShellType ShellType = eShellType.ShellThick;
                        string MatProp = null;
                        double Thickness = -1;
                        int Color = -1;
                        string Notes = null;
                        string GUI = null;
                        string WallName = AreaName[i];
                        int NumberPoint = -1;
                        string[] PointNames = null;
                        int NumberNames = -1;
                        string[] MyName = null;
                        string PropName = null;
                        _SapModel.AreaObj.GetProperty(AreaName[i], ref PropName);//Lấy tên property area object 
                        _SapModel.AreaObj.GetPoints(AreaName[i], ref NumberPoint, ref PointNames);
                        _SapModel.PropArea.GetWall(PropName, ref WallType, ref ShellType, ref MatProp, ref Thickness, ref Color, ref Notes, ref GUI);
                        Point3D[] point3Ds = new Point3D[PointNames.Length];//Khởi tạo giá trị point của wall
                        for (int j = 0; j < PointNames.Length; j++)
                        {
                            double x = 0, y = 0, z = 0;
                            _SapModel.PointObj.GetCoordCartesian(PointNames[j], ref x, ref y, ref z);//Lấy giá trị x,y,z với mỗi namepoint
                            point3Ds[j] = new Point3D(x, y, z);
                        }
                        Models.AreaDesignOrientation.Wall wall = new Models.AreaDesignOrientation.Wall()
                        {
                            Name = WallName,
                            Thickness = Thickness,
                            MaterialName = MatProp,
                            Point = point3Ds
                        };
                        yield return wall;
                    }
                }
            }    
        }
        private IEnumerable<Models.AreaDesignOrientation.Floor> GetFloorETABS()
        {
            
            if (AreaName!= null)
            {
                for (int i = 0; i < AreaName.Length; i++)
                {
                    if (TypeAreaArray[i] == eAreaDesignOrientation.Floor)
                    {
                        eSlabType SlabType = eSlabType.Slab;
                        eShellType ShellType = eShellType.ShellThick;
                        string MatProp = null;
                        double Thickness = -1;
                        int Color = -1;
                        string Notes = null;
                        string GUI = null;
                        string SlabName = AreaName[i];
                        int NumberPoint = -1;
                        string[] pointNames = null;
                        string PropName = null;
                        _SapModel.AreaObj.GetProperty(AreaName[i], ref PropName);//Lấy tên property area object  
                        _SapModel.AreaObj.GetPoints(SlabName, ref NumberPoint, ref pointNames);
                        ret = _SapModel.PropArea.GetSlab(PropName, ref SlabType,ref ShellType, ref MatProp, ref Thickness,ref Color, ref Notes, ref GUI);
                        Point3D[] point3Ds = new Point3D[pointNames.Length];//Khởi tạo giá trị point của slab
                        for (int j = 0; j < pointNames.Length; j++)
                        {
                            double x = 0, y = 0, z = 0;
                            _SapModel.PointObj.GetCoordCartesian(pointNames[j], ref x, ref y, ref z);//Lấy giá trị x,y,z với mỗi namepoint
                            point3Ds[j] = new Point3D(x, y, z);
                        }
                        Models.AreaDesignOrientation.Floor floor = new Models.AreaDesignOrientation.Floor()
                        {
                            Name = SlabName,
                            Thickness = Thickness,
                            MaterialName = MatProp,
                            Point = point3Ds
                        };
                        yield return floor;
                    }        
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
                    //ret = _SapModel.Results.FrameForce(FrameName[i], eItemTypeElm.ObjectElm, ref NumberResults, ref Obj, ref ObjSta, ref Elm, ref ElmSta, ref LoadCase, ref StepType, ref StepNum, ref P, ref V2, ref V3, ref T, ref M2, ref M3);

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
                    beam.GetSection(_SapModel, PropName[i]);
                    yield return beam;
                }
            }
        }
       
    }
}
