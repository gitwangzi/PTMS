using System.ComponentModel.DataAnnotations;

namespace Gsafety.PTMS.SecuritySuite.ViewModels
{
    public class MaintainInfo
    {
        private string vihcleID;
        [Display(Name = "Placa", GroupName = "Name",AutoGenerateField=false)]
        public string VihcleID
        {
            get { return vihcleID; }
            set { vihcleID = value; }
        }

        private string suiteID;///suiteID
        [Display(Name = "Número de Kit de seguridad", GroupName = "Name", AutoGenerateField = false)]
        public string SuiteID
        {
            get { return suiteID; }
            set { suiteID = value; }
        }

        private string maintainStation;///maintainStation
        [Display(Name = "Punto de mantenimiento", GroupName = "Name", AutoGenerateField = false)]
        public string MaintainStation
        {
            get { return maintainStation; }
            set { maintainStation = value; }
        }

        private string maintainTime;///maintainTime
        [Display(Name = "Tiempo de último mantenimiento", GroupName = "Name", AutoGenerateField = false)]
        public string MaintainTime
        {
            get { return maintainTime; }
            set { maintainTime = value; }
        }

        private string maintainer;///maintainer
        [Display(Name = "Personal de mantenimiento", GroupName = "Name", AutoGenerateField = false)]
        public string Maintainer
        {
            get { return maintainer; }
            set { maintainer = value; }
        }

#if test
        public static IEnumerable<MaintainInfo> Infos
        {
            get
            {
                return new ObservableCollection<MaintainInfo>
                {
                    new MaintainInfo{ 
                        VihcleID="BJ001"
                        , MaintainTime="2013/11/01"
                        , Maintainer="Ant_tester"
                        , MaintainStation="sdfewe"
                        , SuiteID="suit00001"}

                        ,new MaintainInfo{ 
                        VihcleID="BJ001"
                        , MaintainTime="2013/11/01"
                        , Maintainer="Ant_tester"
                        , MaintainStation="sdfewe"
                        , SuiteID="suit00001"}

                                                ,new MaintainInfo{ 
                        VihcleID="BJ003"
                        , MaintainTime="2013/11/01"
                        , Maintainer="Ant_tester"
                        , MaintainStation="sdfewe"
                        , SuiteID="suit00001"}

                                                ,new MaintainInfo{ 
                        VihcleID="BJ001"
                        , MaintainTime="2013/11/01"
                        , Maintainer="Ant_tester"
                        , MaintainStation="sdfewe"
                        , SuiteID="suit00001"}

                                                ,new MaintainInfo{ 
                        VihcleID="BJ003"
                        , MaintainTime="2013/11/01"
                        , Maintainer="Ant_tester"
                        , MaintainStation="sdfewe"
                        , SuiteID="suit00001"}

                                                ,new MaintainInfo{ 
                        VihcleID="BJ001"
                        , MaintainTime="2013/11/01"
                        , Maintainer="Ant_tester"
                        , MaintainStation="sdfewe"
                        , SuiteID="suit00001"}

                                                ,new MaintainInfo{ 
                        VihcleID="BJ001"
                        , MaintainTime="2013/11/01"
                        , Maintainer="Ant_tester"
                        , MaintainStation="sdfewe"
                        , SuiteID="suit00001"}

                                                ,new MaintainInfo{ 
                        VihcleID="BJ001"
                        , MaintainTime="2013/11/01"
                        , Maintainer="Ant_tester"
                        , MaintainStation="sdfewe"
                        , SuiteID="suit00001"}

                                                ,new MaintainInfo{ 
                        VihcleID="BJ003"
                        , MaintainTime="2013/11/01"
                        , Maintainer="Ant_tester"
                        , MaintainStation="sdfewe"
                        , SuiteID="suit00001"}

                                                ,new MaintainInfo{ 
                        VihcleID="BJ001"
                        , MaintainTime="2013/11/01"
                        , Maintainer="Ant_tester"
                        , MaintainStation="sdfewe"
                        , SuiteID="suit00001"}

                                                ,new MaintainInfo{ 
                        VihcleID="BJ004"
                        , MaintainTime="2013/11/01"
                        , Maintainer="Ant_tester"
                        , MaintainStation="sdfewe"
                        , SuiteID="suit00001"}

                                                ,new MaintainInfo{ 
                        VihcleID="BJ002"
                        , MaintainTime="2013/11/01"
                        , Maintainer="Ant_tester"
                        , MaintainStation="sdfewe"
                        , SuiteID="suit00001"}

                                                ,new MaintainInfo{ 
                        VihcleID="BJ001"
                        , MaintainTime="2013/11/01"
                        , Maintainer="Ant_tester"
                        , MaintainStation="sdfewe"
                        , SuiteID="suit00001"}

                                                ,new MaintainInfo{ 
                        VihcleID="BJ004"
                        , MaintainTime="2013/11/01"
                        , Maintainer="Ant_tester"
                        , MaintainStation="sdfewe"
                        , SuiteID="suit00001"}

                                                ,new MaintainInfo{ 
                        VihcleID="BJ001"
                        , MaintainTime="2013/11/01"
                        , Maintainer="Ant_tester"
                        , MaintainStation="sdfewe"
                        , SuiteID="suit00001"}

                };
            }
        }
#endif

    }
}
