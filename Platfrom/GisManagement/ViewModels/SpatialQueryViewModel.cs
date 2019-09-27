using ESRI.ArcGIS.Client;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6b8f11a0-793d-42c8-a476-db0e8d359372      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: GisManagement.ViewModels
/////    Project Description:    
/////             Class Name: SpatialQueryViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/5 13:37:12
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/5 13:37:12
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace GisManagement.ViewModels
{
    [ExportAsViewModel(GisName.SpatialQueryViewModel)]
    public class SpatialQueryViewModel : BaseViewModel,
        IEventSink<DrawEventArgs>,
        IPartImportsSatisfiedNotification
    {
        public ICommand SpatialQueryCommand { get; private set; }

        public SpatialQueryViewModel()
        {
            SpatialQueryCommand = new ActionCommand<object>((obj) => SpatialQueryMethod(obj));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void SpatialQueryMethod(object obj)
        {
            //MessageBoxHelper.ShowDialog("提示", "测试内容", MessageDialogButton.OkAndCancel);

            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Test"), MessageDialogButton.OkAndCancel);
        }

        //public QueryContentType contentType = new QueryContentType() { Key = 0, Value = "选择查询类型" };
        public QueryContentType contentType = new QueryContentType() { Key = 0, Value = ApplicationContext.Instance.StringResourceReader.GetString("PleaseSearchType") };
        public QueryContentType ContentType
        {
            get { return contentType; }
            set
            {
                contentType = value;
                RaisePropertyChanged(() => ContentType);
            }
        }

        private string queryContent;
        public string QueryContent
        {
            get { return queryContent; }
            set
            {
                queryContent = value;
                RaisePropertyChanged(() => QueryContent);
            }
        }

        private Dictionary<int, string> queryContentList;
        public Dictionary<int, string> QuerytContentList
        {
            get { return queryContentList; }
            set
            {
                queryContentList = value;
                RaisePropertyChanged(() => QuerytContentList);
            }
        }

        public class QueryContentType
        {
            public int Key { get; set; }
            public string Value { get; set; }
        }









        #region Old Code

        ///// <summary>
        ///// Xml parsing query
        ///// </summary>
        //private string xmlString = "";
        ///// <summary>
        ///// Get maps
        ///// </summary>
        //private ESRI.ArcGIS.Client.Map _MyMap;
        ///// <summary>
        ///// Get maps
        ///// </summary>
        //public ESRI.ArcGIS.Client.Map MyMap
        //{
        //    get
        //    {
        //        if (_MyMap == null)
        //        {
        //            object mview = Router.ViewQuery(GisName.MonitorGisView);
        //            if (mview != null)
        //            {
        //                _MyMap = (mview as GisView).MyMap;
        //            }
        //        }
        //        return _MyMap;
        //    }

        //}

        ///// <summary>
        ///// Initialization
        ///// </summary>
        //public SpatialQueryViewModel()
        //{
        //    CloseQueryPage = new ActionCommand<object>(obj =>
        //    {
        //        //object mview = Router.ViewQuery(GisName.GisView);
        //        UserControl mapView = Router.ViewQuery(ApplicationContext.Instance.CurrentGISName);
        //        ToggleButton toggleButton = mapView.FindName("MapQueryBtn") as ToggleButton;
        //        toggleButton.IsChecked = false;
        //    }
        //    );
        //    SelectFind = true;

        //    try
        //    {
        //        xmlString = ApplicationContext.Instance.ServerConfig.LayersSearchParams;
        //        this.LayersInfoList = ReadMapLayerData();

        //        List<string> tmp = new List<string>();
        //        tmp.Add("TODAS LAS AREAS");
        //        tmp.Add("AREA ESCOGIDA");
        //        this.SelectDrawList = tmp;

        //        //Obtain a regional list
        //        DistrictServiceClient regionServiceClient = ServiceClientFactory.Create<DistrictServiceClient>();

        //        regionServiceClient.GetDistrictCompleted += regionServiceClient_GetAllProvincesCompleted;
        //        regionServiceClient.GetDistrictAsync();


        //        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DistrictList));
        //        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectDrawList));

        //        bool Committed = true;
        //        SpatialQueryCommand = new ActionCommand<object>(obj => SpatialQuery(), obj => Committed);
        //        _searchText = "";
        //        QueryResultList = new ObservableCollection<TResultInfo>();
        //        CanQuery = true;
        //    }
        //    catch (Exception ee)
        //    {
        //        ApplicationContext.Instance.Logger.LogException("SpatialQueryViewModel", ee);
        //    }
        //}

        //public void OnImportsSatisfied()
        //{
        //    EventAggregator.SubscribeOnDispatcher<DrawEventArgs>(this);
        //}

        ///// <summary>
        ///// Accepted a collection of objects
        ///// </summary>
        ///// <param name="publishedEvent"></param>

        //public void HandleEvent(DrawEventArgs publishedEvent)
        //{
        //   // MessageBox.Show("123");
        //}

        ///// <summary>
        ///// Get all the regions
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void regionServiceClient_GetAllProvincesCompleted(object sender, GetDistrictCompletedEventArgs e)
        //{
        //    //Areas.Clear();
        //    try
        //    {
        //        DistrictList.Add(TranslateInfo.Translate(TranslateInfo.AllProv));
        //        foreach (District r in e.Result.Result)
        //        {
        //            DistrictList.Add(r.Name);
        //            //Areas.Add(new Area { ID = r.ID, Code = r.Code, Name = r.Name, Level = r.Level, Children = new ObservableCollection<Area>() });
        //        }

        //        if (DistrictList.Count > 0) SelectDistrictItem = DistrictList[0];
        //    }
        //    catch (Exception ee)
        //    {
        //        ApplicationContext.Instance.Logger.LogException("SpatialQueryViewModel", ee);
        //    }
        //    //vehicleServiceClient.GetAllCompanyAsync();
        //}
        //private List<LayersInfo> _LayersInfo;
        ///// <summary>
        ///// Can be found in the project configuration
        ///// </summary>
        //public List<LayersInfo> LayersInfoList
        //{
        //    get
        //    {
        //        return _LayersInfo;
        //    }
        //    set
        //    {
        //        _LayersInfo = value;
        //        if (_LayersInfo.Count > 0)
        //        {
        //            SelectLayersInfo = _LayersInfo[0];
        //        }
        //        ////RaisePropertyChanged("LayersInfo");
        //    }
        //}
        //private LayersInfo _SelectLayersInfo;
        ///// <summary>
        ///// Select query
        ///// </summary>
        //public LayersInfo SelectLayersInfo
        //{
        //    get
        //    {
        //        return _SelectLayersInfo;
        //    }
        //    set
        //    {
        //        _SelectLayersInfo = value;
        //        //if _SelectLayersInfo  If the Find method, disable region selection box
        //        if (_SelectLayersInfo.method.ToLower() == "find")
        //        {
        //            if (DistrictList.Count > 0) SelectDistrictItem = DistrictList[0];
        //            DistrictEnbled = false;
        //        }
        //        else
        //        {
        //            if ((_SelectLayersInfo.DistrictField != null) && (_SelectLayersInfo.DistrictField.ToString() != ""))
        //                DistrictEnbled = true;
        //            else
        //                DistrictEnbled = false;
        //        }
        //        RaisePropertyChanged("SelectLayersInfo");
        //    }
        //}
        //List<string> _DistrictList = new List<string>();
        ///// <summary>
        ///// Regions
        ///// </summary>
        //public List<string> DistrictList
        //{
        //    get
        //    {
        //        return _DistrictList;
        //    }
        //    set
        //    {
        //        _DistrictList = value;
        //    }
        //}


        //private string _SelectDistrictItem;
        ///// <summary>
        ///// The selected district
        ///// </summary>
        //public string SelectDistrictItem
        //{
        //    get
        //    {
        //        return _SelectDistrictItem;
        //    }
        //    set
        //    {
        //        _SelectDistrictItem = value;
        //        RaisePropertyChanged("SelectDistrictItem");
        //    }
        //}
        //private bool _DistrictEnbled;
        ///// <summary>
        ///// Administrative regions are optional
        ///// </summary>
        //public bool DistrictEnbled
        //{
        //    get
        //    {
        //        return _DistrictEnbled;
        //    }
        //    set
        //    {
        //        _DistrictEnbled = value;
        //        RaisePropertyChanged("DistrictEnbled");
        //    }
        //}
        ///// <summary>
        ///// Select the query specified region
        ///// </summary>
        //private List<string> _SelectDrawList;
        //public List<string> SelectDrawList
        //{
        //    get
        //    {
        //        return _SelectDrawList;
        //    }
        //    set
        //    {
        //        _SelectDrawList = value;
        //        if (_SelectDrawList.Count > 0)
        //        {
        //            SelectDrawInfo = _SelectDrawList[0];
        //        }
        //    }
        //}
        ///// <summary>
        ///// Specify the manner in which the query
        ///// </summary>
        //private string _SelectDrawInfo;
        //public string SelectDrawInfo
        //{
        //    get
        //    {
        //        return _SelectDrawInfo;
        //    }
        //    set
        //    {
        //        if (_SelectDrawInfo == null)
        //        {
        //            _SelectDrawInfo = value;
        //        }
        //        else
        //        {
        //            _SelectDrawInfo = value;
        //            if (_SelectDrawInfo == "Selected Area")
        //            {
        //                GraphicsLayer gly = MyMap.Layers[ConstDefine.MyDrawQueryGraphicsLayer] as GraphicsLayer;
        //                gly.Graphics.Clear();
        //                gly.Graphics.Add(new Graphic()
        //                {
        //                    Geometry = ApplicationContext.Instance.CurrentDrawArgs,
        //                    Symbol =
        //                        new SimpleFillSymbol()
        //                        {
        //                            BorderBrush = new SolidColorBrush(Colors.Red) { },
        //                            BorderThickness = 2,
        //                            Fill = new SolidColorBrush(Colors.Green) { Opacity = 0.5 }
        //                        }
        //                });
        //            }
        //            else
        //            {
        //                GraphicsLayer gly = MyMap.Layers[ConstDefine.MyDrawQueryGraphicsLayer] as GraphicsLayer;
        //                gly.Graphics.Clear();
        //            }
        //            RaisePropertyChanged("SelectDrawInfo");
        //        }
        //    }
        //}
        ///// <summary>
        ///// Are Precise query
        ///// </summary>
        //public bool ExactFind
        //{
        //    get;
        //    set;
        //}
        ///// <summary>
        ///// Are marquee
        ///// </summary>
        //public bool SelectFind
        //{
        //    get;
        //    set;
        //}
        //private string _searchText;
        ///// <summary>
        ///// Search string
        ///// </summary>
        //public string SearchText
        //{
        //    get { return _searchText; }
        //    set
        //    {
        //        _searchText = value;
        //        RaisePropertyChanged("SearchText");
        //    }
        //}
        ///// <summary>
        ///// Can query
        ///// </summary>
        //private bool _CanQuery;
        //public bool CanQuery
        //{
        //    get
        //    {
        //        return _CanQuery;
        //    }
        //    set
        //    {
        //        _CanQuery = value;
        //        RaisePropertyChanged("CanQuery");
        //    }
        //}
        //private ObservableCollection<TResultInfo> _QueryResultList;
        ///// <summary>
        ///// query result
        ///// </summary>
        //public ObservableCollection<TResultInfo> QueryResultList
        //{
        //    get
        //    {
        //        return _QueryResultList;
        //    }
        //    set
        //    {
        //        _QueryResultList = value;
        //    }
        //}
        //public IActionCommand SpatialQueryCommand { get; private set; }
        ///// <summary>
        ///// Selected rows: Perform positioning and labeling
        ///// </summary>
        //private TResultInfo _UserSelectedRow;
        //public TResultInfo UserSelectedRow
        //{
        //    get
        //    {
        //        return _UserSelectedRow;
        //    }
        //    set
        //    {
        //        _UserSelectedRow = value;
        //        if ((UserSelectedRow != null))
        //        {
        //            if (double.IsNaN(UserSelectedRow.Lon) == false && double.IsNaN(UserSelectedRow.Lat) == false)
        //            {
        //                LocatePoint(UserSelectedRow.Lon, UserSelectedRow.Lat);
        //            }
        //            GraphicsLayer gly = MyMap.Layers[ConstDefine.TempDrawLayer] as GraphicsLayer;
        //            if (gly != null && UserSelectedRow.Graphic != null)
        //            {
        //                gly.Graphics.Clear();
        //                if (UserSelectedRow.Graphic.Symbol == null)
        //                {
        //                    UserSelectedRow.Graphic.Symbol = new ESRI.ArcGIS.Client.Symbols.SimpleMarkerSymbol()
        //                    {
        //                        Color = new SolidColorBrush() { Color = Colors.Red },
        //                        Size = 20
        //                    };
        //                }
        //                gly.Graphics.Add(UserSelectedRow.Graphic);
        //            }
        //            //  EventAggregator.Publish<LocateEventArgs>(new LocateEventArgs() { Lat = UserSelectedRow.Lat, Lon = UserSelectedRow.Lon });
        //            // EventAggregator.Publish<DrawGraphicsEventArgs>(new DrawGraphicsEventArgs() { Graphic = UserSelectedRow.Graphic });
        //        }
        //    }
        //}
        ///// <summary>
        ///// LocatePoint
        ///// </summary>
        ///// <param name="dx"></param>
        ///// <param name="dy"></param>
        //public void LocatePoint(double dx, double dy)
        //{
        //    if (MyMap == null) return;
        //    try
        //    {
        //        string strLocal = ApplicationContext.Instance.ServerConfig.AutoLocateResolution;
        //        strLocal = strLocal.Replace(".", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
        //        //string CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
        //        //if (CurrentUICulture.ToLower().Trim() == "es-es")
        //        //{
        //        //    strLocal = strLocal.Replace('.', ',');
        //        //}
        //        if (MyMap.Resolution > Convert.ToDouble(strLocal))
        //        {
        //            CenterAndZoom(Convert.ToDouble(strLocal), dx, dy);
        //        }
        //        else
        //        {
        //            MapPoint pPoint = new MapPoint();
        //            pPoint.X = dx;
        //            pPoint.Y = dy;
        //            pPoint.SpatialReference = _MyMap.SpatialReference;
        //            MyMap.PanTo(pPoint);
        //        }

        //    }
        //    catch (Exception ee)
        //    {
        //        ApplicationContext.Instance.Logger.LogException("SpatialQueryViewModel", ee);
        //    }
        //}


        ///// <summary>
        ///// With a vision Location Map
        ///// </summary>
        ///// <param name="myResolution"></param>
        ///// <param name="myMapPoint"></param>
        //public void CenterAndZoom(double myResolution, double dx, double dy)
        //{
        //    double ratio = 1.0;
        //    if (MyMap.Resolution != 0.0)
        //    {
        //        ratio = myResolution / MyMap.Resolution;
        //    }
        //    MapPoint pPoint = new MapPoint();
        //    pPoint.X = dx;
        //    pPoint.Y = dy;
        //    pPoint.SpatialReference = _MyMap.SpatialReference;
        //    if (ratio == 1.0)
        //    {
        //        MyMap.PanTo(pPoint);
        //    }
        //    else
        //    {
        //        ESRI.ArcGIS.Client.Geometry.Envelope myEnvelope = MyMap.Extent;

        //        ESRI.ArcGIS.Client.Geometry.MapPoint pt = myEnvelope.GetCenter();

        //        double x = (pPoint.X - ratio * pt.X) / (1 - ratio);
        //        double y = (pPoint.Y - ratio * pt.Y) / (1 - ratio);
        //        ESRI.ArcGIS.Client.Geometry.MapPoint newpt = new ESRI.ArcGIS.Client.Geometry.MapPoint(x, y);

        //        MyMap.ZoomToResolution(myResolution, newpt);

        //    }
        //}

        ///// <summary>
        ///// Close the query window event
        ///// </summary>
        //public IActionCommand CloseQueryPage { get; private set; }
        ///// <summary>
        ///// read config
        ///// </summary>
        ///// <returns></returns>
        //public List<LayersInfo> ReadMapLayerData()
        //{
        //    List<LayersInfo> LayersList = new List<LayersInfo>();
        //    try
        //    {
        //        LayersInfo templayer = null;
        //        string maptype = "";
        //        string mapurl = "";
        //        if (xmlString.Equals("") == true)
        //            return null;
        //        using (XmlReader reader = XmlReader.Create(new System.IO.StringReader(xmlString)))
        //        {
        //            while (reader.Read())
        //            {
        //                if (reader.NodeType == XmlNodeType.Element)
        //                {
        //                    string elementName = reader.Name.ToLower();

        //                    if (elementName == "onesearch")
        //                    {
        //                        templayer = new LayersInfo();
        //                        templayer.MapName = maptype;
        //                        templayer.MapUrl = mapurl;
        //                        templayer.LayerName = reader["name"].ToString();
        //                        templayer.method = reader["method"].ToString();
        //                        templayer.LayerID = reader["layerid"].ToString();
        //                        templayer.QueryFields = reader["queryfields"].ToString();
        //                        templayer.DistrictField = reader["districtfield"].ToString();
        //                        templayer.OutFields = reader["outfields"].ToString();
        //                        LayersList.Add(templayer);
        //                    }
        //                    if (elementName == "map")
        //                    {
        //                        maptype = reader["name"];
        //                        mapurl = reader["url"];
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ee)
        //    {
        //        ApplicationContext.Instance.Logger.LogException("SpatialQueryViewModel", ee);
        //    }
        //    return LayersList;
        //}
        ///// <summary>
        ///// query methord
        ///// </summary>
        //private void SpatialQuery()
        //{
        //    //if (ApplicationContext.Instance.CurrentDrawArgs != null)
        //    //{
        //    //    //query.Geometry = ApplicationContext.Instance.CurrentDrawArgs;
        //    //    SelectFind = true;
        //    //}

        //    CanQuery = false;
        //    try
        //    {
        //        if (SelectLayersInfo.method.ToLower() == "find".ToLower())
        //        {
        //            FindTask findtask = new FindTask(SelectLayersInfo.MapUrl);
        //            findtask.ExecuteCompleted += findtask_ExecuteCompleted;
        //            findtask.Failed += findtask_Failed;

        //            FindParameters findParameters = new FindParameters();
        //            int[] range = GetIntArray(SelectLayersInfo.LayerID);
        //            findParameters.LayerIds.AddRange(range);

        //            findParameters.SearchFields.AddRange(new string[] { SelectLayersInfo.QueryFields });
        //            findParameters.Contains = !ExactFind;
        //            findParameters.SearchText = SearchText;

        //            findtask.ExecuteAsync(findParameters);
        //        }
        //        else if (SelectLayersInfo != null)
        //        {
        //            QueryTask queryTask = new QueryTask(SelectLayersInfo.MapUrl + SelectLayersInfo.LayerID);
        //            queryTask.ExecuteCompleted += queryTask_ExecuteCompleted;
        //            queryTask.Failed += queryTask_Failed;


        //            Query query = new Query();
        //            query.OutFields.AddRange(new string[] { SelectLayersInfo.QueryFields });

        //            if ((ApplicationContext.Instance.CurrentDrawArgs != null) && (SelectDrawInfo == "Selected Area"))
        //            {
        //                query.Geometry = ApplicationContext.Instance.CurrentDrawArgs;
        //                //SelectFind = true;
        //            }
        //            //else
        //            //{
        //            //    GraphicsLayer gly = MyMap.Layers[ConstDefine.TempDrawQueryLayer] as GraphicsLayer;
        //            //    gly.Graphics.Clear();
        //            //}



        //            query.ReturnGeometry = true;
        //            if (ExactFind)
        //            {
        //                if ((SelectLayersInfo.DistrictField.ToString() != "") && (SelectDistrictItem != TranslateInfo.Translate(TranslateInfo.AllProv)))
        //                {
        //                    query.Where = SelectLayersInfo.DistrictField + "='" + SelectDistrictItem + "' and " + SelectLayersInfo.QueryFields + " = '" + SearchText + "'";
        //                }
        //                else
        //                {
        //                    query.Where = SelectLayersInfo.QueryFields + " = '" + SearchText + "'";
        //                }
        //            }
        //            else
        //            {
        //                if ((SelectLayersInfo.DistrictField != null) && (SelectLayersInfo.DistrictField.ToString() != "") && (SelectDistrictItem != TranslateInfo.Translate(TranslateInfo.AllProv)))
        //                {
        //                    query.Where = SelectLayersInfo.DistrictField + "='" + SelectDistrictItem + "' and " + SelectLayersInfo.QueryFields + " like '%" + SearchText + "%'";
        //                }
        //                else
        //                {
        //                    query.Where = SelectLayersInfo.QueryFields + " like '%" + SearchText + "%'";
        //                }
        //            }
        //            queryTask.ExecuteAsync(query);
        //        }
        //        //ApplicationContext.Instance.CurrentDrawArgs = null;
        //    }
        //    catch (Exception ee)
        //    {
        //        ApplicationContext.Instance.Logger.LogException("SpatialQueryViewModel", ee);
        //    }
        //}
        ///// <summary>
        ///// Determine whether there is an invalid character
        ///// </summary>
        ///// <param name="_searchText"></param>
        ///// <returns></returns>
        //private bool HasInValidChar(string _searchText)
        //{
        //    char[] temp = "\\\'*%".ToString().ToCharArray();
        //    for (int i = 0; i <= temp.Length - 1; i++)
        //    {
        //        if (_searchText.IndexOf(temp[i]) > -1) return true;
        //    }
        //    return false;
        //}
        ///// <summary>
        ///// The semicolon-delimited string into an array of integers
        ///// </summary>
        ///// <param name="str"></param>
        ///// <returns></returns>
        //private int[] GetIntArray(string str)
        //{
        //    try
        //    {
        //        string[] temp = str.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //        int[] c = new int[temp.Length];
        //        for (int i = 0; i <= temp.Length - 1; i++)
        //        {
        //            c[i] = Convert.ToInt32(temp[i].ToString());
        //        }
        //        return c;
        //    }
        //    catch (Exception ee)
        //    {
        //        ApplicationContext.Instance.Logger.LogException("SpatialQueryViewModel", ee);
        //        return null;
        //    }
        //}
        //#region Results of event processing

        ///// <summary>
        ///// Query execution success events
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void queryTask_ExecuteCompleted(object sender, QueryEventArgs e)
        //{
        //    try
        //    {
        //        QueryResultList.Clear();
        //        if (e.FeatureSet != null)
        //        {
        //            foreach (Graphic g in e.FeatureSet.Features)
        //            {
        //                if (g.Geometry != null)
        //                {
        //                    TResultInfo item = new TResultInfo();
        //                    item.LayerName = SelectLayersInfo.LayerName;
        //                    item.Value = g.Attributes[SelectLayersInfo.QueryFields].ToString();
        //                    //  ESRI.ArcGIS.Client.Geometry.MapPoint pt = GpsCarListViewModel.GetGeoCoord(g.Geometry.Extent.GetCenter().X, g.Geometry.Extent.GetCenter().Y);
        //                    ESRI.ArcGIS.Client.Geometry.MapPoint pt = g.Geometry as MapPoint;
        //                    item.Lon = pt.X;
        //                    item.Lat = pt.Y;
        //                    item.Graphic = g;
        //                    QueryResultList.Add(item);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        ApplicationContext.Instance.Logger.LogException("SpatialQueryViewModel", ee);
        //    }
        //    CanQuery = true;
        //}
        ///// <summary>
        ///// Query failed event
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void queryTask_Failed(object sender, TaskFailedEventArgs e)
        //{
        //    MessageBox.Show(TranslateInfo.Translate(TranslateInfo.QueryFindError), ApplicationContext.Instance.StringResourceReader.GetString("ConfirmMessage"), MessageBoxButton.OK);

        //    ApplicationContext.Instance.Logger.LogException("SpatialQueryViewModel", e.Error);
        //    CanQuery = true;
        //}
        ///// <summary>
        ///// Find  Successful event
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void findtask_ExecuteCompleted(object sender, FindEventArgs e)
        //{
        //    try
        //    {
        //        QueryResultList.Clear();
        //        if (e.FindResults != null)
        //        {
        //            foreach (FindResult fr in e.FindResults)
        //            {
        //                if (fr.Feature.Geometry != null)
        //                {
        //                    TResultInfo item = new TResultInfo();
        //                    item.LayerName = fr.LayerName;
        //                    item.Value = fr.Value.ToString();
        //                    //ESRI.ArcGIS.Client.Geometry.MapPoint pt = GpsCarListViewModel.GetGeoCoord(fr.Feature.Geometry.Extent.GetCenter().X, fr.Feature.Geometry.Extent.GetCenter().Y);
        //                    ESRI.ArcGIS.Client.Geometry.MapPoint pt = fr.Feature.Geometry as MapPoint;
        //                    item.Lon = pt.X;
        //                    item.Lat = pt.Y;
        //                    item.Graphic = fr.Feature;
        //                    QueryResultList.Add(item);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        ApplicationContext.Instance.Logger.LogException("SpatialQueryViewModel", ee);
        //    }
        //    CanQuery = true;
        //}
        ///// <summary>
        ///// Find failed event
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void findtask_Failed(object sender, TaskFailedEventArgs args)
        //{
        //    MessageBox.Show(TranslateInfo.Translate(TranslateInfo.QueryFindError) + Environment.NewLine + args.Error, ApplicationContext.Instance.StringResourceReader.GetString("ConfirmMessage"), MessageBoxButton.OK);

        //    ApplicationContext.Instance.Logger.LogException("SpatialQueryViewModel", args.Error);
        //    CanQuery = true;
        //}
        //#endregion
        //#region The results define the content
        ///// <summary>
        ///// Class definition query results
        ///// </summary>
        //public class TResultInfo
        //{
        //    /// <summary>
        //    /// layer name
        //    /// </summary>
        //    public string LayerName { get; set; }
        //    /// <summary>
        //    /// result
        //    /// </summary>
        //    public string Value { get; set; }
        //    /// <summary>
        //    /// lat
        //    /// </summary>
        //    public double Lat { get; set; }
        //    /// <summary>
        //    /// lon
        //    /// </summary>
        //    public double Lon { get; set; }
        //    /// <summary>
        //    /// graphic
        //    /// </summary>
        //    public ESRI.ArcGIS.Client.Graphic Graphic { get; set; }
        //}
        //#endregion
        //#region Defined layers inquiry
        //public class LayersInfo
        //{
        //    /// <summary>
        //    /// Map collation name
        //    /// </summary>
        //    public string MapName
        //    {
        //        get;
        //        set;
        //    }
        //    /// <summary>
        //    /// Url
        //    /// </summary>
        //    private string _MapUrl;
        //    public string MapUrl
        //    {
        //        get { return _MapUrl; }
        //        set { _MapUrl = value; }
        //    }

        //    private string _LayerName;
        //    /// <summary>
        //    /// layer name
        //    /// </summary>
        //    public string LayerName
        //    {
        //        get { return _LayerName; }
        //        set { _LayerName = value; }
        //    }

        //    private string _LayerID;
        //    /// <summary>
        //    /// layer ID
        //    /// </summary>
        //    public string LayerID
        //    {
        //        get { return _LayerID; }
        //        set { _LayerID = value; }
        //    }

        //    private string _QueryFields;
        //    /// <summary>
        //    /// query field
        //    /// </summary>
        //    public string QueryFields
        //    {
        //        get { return _QueryFields; }
        //        set { _QueryFields = value; }
        //    }

        //    private string _OutFields;
        //    /// <summary>
        //    /// out fields
        //    /// </summary>
        //    public string OutFields
        //    {
        //        get { return _OutFields; }
        //        set { _OutFields = value; }
        //    }

        //    /// <summary>
        //    /// province field
        //    /// </summary>
        //    public string DistrictField { get; set; }
        //    /// <summary>
        //    /// method
        //    /// </summary>
        //    public string method { get; set; }
        //}
        //#endregion

        #endregion


        public void HandleEvent(DrawEventArgs publishedEvent)
        {

        }

        public void OnImportsSatisfied()
        {

        }
    }
}
