using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Symbols;
using GisManagement.Views;
using Gsafety.PTMS.ServiceReference.BscGeoPoiService;
using Gsafety.PTMS.Share;
using Jounce.Framework;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace GisManagement.ViewModels
{
    public partial class GisViewModel
    {
        public BscGeoPoiServiceClient client = null;
        public ICommand SpatialQueryCommand { get; private set; }
        public ICommand MouseLeftClickDownCommand { get; private set; }
        public ICommand BtnIconSearchGeo { get; private set; }

        /// <summary>
        /// 初始化地图查询功能组建及相关属性    
        /// </summary>
        public void InitSearchProperty()
        {
            SpatialQueryCommand = new ActionCommand<object>((obj) => SpatialQueryMethod(obj));
            MouseLeftClickDownCommand = new ActionCommand<MouseButtonEventArgs>((e) => Mouse_MouseLeftClickDown(e));
            BtnIconSearchGeo = new ActionCommand<object>((obj) => ClearSearchContent());

            LoadSearchContentType();//记载查询位置类型

        }

        private void ClearSearchContent()
        {
            this.SearchContent = "";
            LocateType = new SearchContentType();
            LocateType.Key = MapSearchType.None;
            QuerytContentList = new Dictionary<int, string>();

            ClearGraphicsLayer();
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SearchContent));
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => LocateType));
            JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => QuerytContentList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void Mouse_MouseLeftClickDown(MouseButtonEventArgs e)
        {

        }


        protected void LoadSearchContentType()
        {

            //学校
            //医院
            //政府
            //银行
            //ATM机
            //商场
            //机场
            //景区
            //None = -1,
            //School = 0,
            //Hospital = 1,
            //Goverment = 2,
            //Bank = 3,
            //ATM = 4,
            //Market = 5,
            //Airport = 6,
            //ScenerySpot = 7

            LocateTypeList = new List<SearchContentType>();

            //LocateTypeList.Add(new SearchContentType()
            //{
            //    Key = MapSearchType.None,
            //    //Value = "选择查询类型"
            //    Value = ApplicationContext.Instance.StringResourceReader.GetString("PleaseSearchType")
            //});

            LocateTypeList.Add(new SearchContentType()
            {
                Key = MapSearchType.School,
                //Value = "学校"
                Value = ApplicationContext.Instance.StringResourceReader.GetString("School")
            });

            LocateTypeList.Add(new SearchContentType()
            {
                Key = MapSearchType.Hospital,
                //Value = "医院"
                Value = ApplicationContext.Instance.StringResourceReader.GetString("Hospital")
            });
            LocateTypeList.Add(new SearchContentType()
            {
                Key = MapSearchType.Goverment,
                //Value = "政府"
                Value = ApplicationContext.Instance.StringResourceReader.GetString("Goverment")
            });
            LocateTypeList.Add(new SearchContentType()
            {
                Key = MapSearchType.Bank,
                //Value = "银行"
                Value = ApplicationContext.Instance.StringResourceReader.GetString("Bank")
            });
            LocateTypeList.Add(new SearchContentType()
            {
                Key = MapSearchType.ATM,
                //Value = "ATM取款机"
                //Value = ApplicationContext.Instance.StringResourceReader.GetString("ATM")
            });
            LocateTypeList.Add(new SearchContentType()
            {
                Key = MapSearchType.Market,
                //Value = "超市"
                Value = ApplicationContext.Instance.StringResourceReader.GetString("Market")
            });
            LocateTypeList.Add(new SearchContentType()
            {
                Key = MapSearchType.Airport,
                //Value = "机场"
                Value = ApplicationContext.Instance.StringResourceReader.GetString("Airport")
            });
            LocateTypeList.Add(new SearchContentType()
            {
                Key = MapSearchType.ScenerySpot,
                //Value = "景区"
                Value = ApplicationContext.Instance.StringResourceReader.GetString("ScenerySpot")
            });
            LocateType = LocateTypeList[0];

        }


        void client_GetBscGeoPoiListCompleted(object sender, GetBscGeoPoiListCompletedEventArgs e)
        {
            try
            {
                QuerytContentList = new Dictionary<int, string>();
                Coordinate = new Dictionary<int, MapPoint>();

                string lineFeed = "\n";
                string colon = " : ";
                int row = 0;

                if (e.Result.Result.Count > 0)
                    foreach (var info in e.Result.Result)
                    {
                        row = row + 1;
                        StringBuilder sb = new StringBuilder();

                        sb.Append(info.Name + "," + info.Contry
                            + lineFeed);
                        sb.Append(ApplicationContext.Instance.StringResourceReader.GetString("Latitude") + colon + info.Latidue
                            + " " + ApplicationContext.Instance.StringResourceReader.GetString("Longitude") + colon + info.Longitude
                            + lineFeed);
                        sb.Append(ApplicationContext.Instance.StringResourceReader.GetString("Address") + colon + info.Address
                            + lineFeed);


                        //类型名称
                        string propertyName = ApplicationContext.Instance.StringResourceReader.GetString("Unkown");
                        foreach (var item in LocateTypeList)
                        {
                            if (((int)item.Key) == ((int)info.Property))
                            {
                                propertyName = item.Value;
                            }
                        }
                        //sb.Append(ApplicationContext.Instance.StringResourceReader.GetString("TargetProperty") + colon + info.Property);
                        sb.Append(ApplicationContext.Instance.StringResourceReader.GetString("TargetProperty") + colon + propertyName);

                        QuerytContentList.Add(row, sb.ToString().Trim());

                        //MapPoint mapPoint =
                        //    ESRI.ArcGIS.Client.Bing.Transform.GeographicToWebMercator(new MapPoint(Convert.ToDouble(info.Latidue), Convert.ToDouble(info.Longitude)));
                        MapPoint mapPoint = Gsafety.Common.Transform.GeographicToWebMercator(new MapPoint(Convert.ToDouble(info.Latidue), Convert.ToDouble(info.Longitude)));
                        //
                        Coordinate.Add(row, new MapPoint(Convert.ToDouble(info.Latidue), Convert.ToDouble(info.Longitude)));

                        //添加锚点图层
                        this.MarkerAnchorLocate(mapPoint, GisView.anchorPointSymbol, GisView.anchorPointGraphicsLayer, info);
                    }
                else
                    QuerytContentList.Add(1, ApplicationContext.Instance.StringResourceReader.GetString("None"));
            }
            catch (Exception)
            {
                client = ServiceClientFactory.Create<BscGeoPoiServiceClient>();
            }
            finally
            {
                client.CloseAsync();
                JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => QuerytContentList));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void SpatialQueryMethod(object obj)
        {
            if (!string.IsNullOrEmpty(this.searchContent.Trim()))
            {
                client = ServiceClientFactory.Create<BscGeoPoiServiceClient>();
                client.GetBscGeoPoiListCompleted += client_GetBscGeoPoiListCompleted;

                ClearGraphicsLayer();
                QuerytContentList = new Dictionary<int, string>();
                SearchContentVisibility = Visibility.Visible;
                JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => LocateType));
                client.GetBscGeoPoiListAsync(0, 0, this.SearchContent, (decimal)LocateType.Key);
            }
            else
            {
                QuerytContentList = new Dictionary<int, string>();
                QuerytContentList.Add(1, ApplicationContext.Instance.StringResourceReader.GetString("None"));
            }
        }


        #region Field


        /// <summary>
        /// 查询内容数据加载框是否可用
        /// </summary>
        private Visibility searchContentVisibility = Visibility.Collapsed;
        public Visibility SearchContentVisibility
        {
            get
            {
                return searchContentVisibility;
            }
            set
            {
                searchContentVisibility = value;
                RaisePropertyChanged(() => SearchContentVisibility);
            }
        }

        /// <summary>
        /// 查询的内容
        /// </summary>
        private string searchContent;
        public string SearchContent
        {
            get { return searchContent; }
            set
            {
                searchContent = value;
                RaisePropertyChanged(() => SearchContent);
                if (string.IsNullOrEmpty(SearchContent))
                {
                    SearchContentVisibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// 查询到的位置数据列表
        /// </summary>
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

        /// <summary>
        /// 位置类型
        /// </summary>
        private SearchContentType locateType;
        public SearchContentType LocateType
        {
            get { return locateType; }
            set
            {
                locateType = value;
                RaisePropertyChanged(() => LocateType);
            }
        }


        private List<MapSearchType> locateTypeCollecte;
        public List<MapSearchType> LocateTypeCollecte
        {
            get
            {
                return locateTypeCollecte;
            }
            set
            {
                locateTypeCollecte = value;
            }
        }


        /// <summary>
        /// 位置类型列表
        /// </summary>
        private List<SearchContentType> locateTypeList;
        public List<SearchContentType> LocateTypeList
        {
            get { return locateTypeList; }
            set
            {
                locateTypeList = value;
                RaisePropertyChanged(() => LocateTypeList);
            }
        }


        public static Dictionary<int, MapPoint> Coordinate = new Dictionary<int, MapPoint>();



        /// <summary>
        /// 
        /// </summary>
        public class SearchContentType
        {
            public MapSearchType Key { get; set; }
            public string Value { get; set; }
        }
        #endregion


        /// <summary>
        /// 清理图层
        /// </summary>
        void ClearGraphicsLayer()
        {
            foreach (Layer layer in GisView.myMap.Layers)
            {
                if (layer is GraphicsLayer)
                {
                    if (layer.ID.Trim() == GisView.anchorPointGraphicsLayer.ID.Trim())
                    {
                        (layer as GraphicsLayer).Graphics.Clear();
                    }
                }
            }
        }



        /// <summary>
        /// 锚点
        /// </summary>
        /// <param name="mapPoint">坐标</param>
        /// <param name="layoutRoot"></param>
        /// <param name="anchors">所有位置点</param>
        /// <param name="anchorLayer">放锚点的图层</param>
        /// <param name="tipModel">锚点详细信息</param>
        void MarkerAnchorLocate(MapPoint mapPoint, Symbol anchorPointSymbol, GraphicsLayer anchorLayer, BscGeoPoi tipModel)
        {
            Graphic anchor = new Graphic()
            {
                Geometry = mapPoint,
                Symbol = anchorPointSymbol //layoutRoot.Resources["AnchorPointSymbol"]
            };
            anchor.Attributes.Add("PointNumber", anchorLayer.Graphics.Count + 1);
            anchor.Geometry = mapPoint;
            anchor.MapTip = this.LoadAnchorTipInfo(tipModel);
            anchorLayer.Graphics.Add(anchor);//添加一个锚点土层

        }



        /// <summary>
        /// 加载锚点上的提示信息框
        /// </summary>
        /// <returns></returns>
        public AnchorInfoView LoadAnchorTipInfo(BscGeoPoi tipModel)
        {
            AnchorInfoView myMapTip = new AnchorInfoView();
            myMapTip.Name.Text = tipModel.Name;
            myMapTip.Latitude.Text = tipModel.Latidue.ToString();
            myMapTip.Longitude.Text = tipModel.Longitude.ToString();
            myMapTip.Address.Text = tipModel.Address;
            myMapTip.Country.Text = tipModel.Contry;
            return myMapTip;
        }


        /// <summary>
        /// 选择查询类型
        /// </summary>
        public enum MapSearchType
        {
            //学校
            //医院
            //政府
            //银行
            //ATM机
            //商场
            //机场
            //景区
            None = -1,
            School = 0,
            Hospital = 1,
            Goverment = 2,
            Bank = 3,
            ATM = 4,
            Market = 5,
            Airport = 6,
            ScenerySpot = 7
        }


        /// <summary>
        /// 对地图图层进行操作
        /// </summary>
        public class MapLayerService
        {
            /// <summary>
            /// 清理GraphicsLayer
            /// </summary>
            /// <param name="myMap"></param>
            public static void ClearAnchorByGraphicLayers(Map myMap)
            {
                foreach (Layer layer in myMap.Layers)
                {
                    if (layer is GraphicsLayer)
                    {
                        (layer as GraphicsLayer).Graphics.Clear();
                    }
                }
            }
        }

    }
}
