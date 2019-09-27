/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: a3d3bfda-ab12-4a3c-afc4-10d8b93ffbc1      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Alarm
/////    Project Description:    
/////             Class Name: WebServiceProxy
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/8/20 10:27:44
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/8/20 10:27:44
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Web.Services;
using System.Web.Services.Description;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using System.Web.Services.Protocols;
using System.Net;
using System.Xml;
using Gsafety.Common.Logging;


namespace Gsafety.PTMS.Analysis.Alarm
{
    public sealed class WebServiceProxy
    {
        private static Dictionary<string,Type> webserviceDic=new Dictionary<string,Type>();
        private static Type CreateWebServiceClient(string pcUrl)
        {
            string NameSpace = "Gsafety.PTMS.Analysis.Alarm";

            WebClient webclient = new WebClient();
            //Stream stream = webclient.OpenRead(pcUrl + "?WSDL");

            XmlTextReader reader = new XmlTextReader(pcUrl + "?WSDL");
            //创建和格式化wsdl文档
            LoggerManager.Logger.Warn(reader.ReadInnerXml());
            ServiceDescription sd = ServiceDescription.Read(reader);
            string className = sd.Services[0].Name;

            //创建客户端代理类
            ServiceDescriptionImporter SDI = new ServiceDescriptionImporter();
            SDI.AddServiceDescription(sd, "", "");

            //使用codedom编译客户端代理类
            CodeNamespace cn = new CodeNamespace(NameSpace);
            CodeCompileUnit ccu = new CodeCompileUnit();
            ccu.Namespaces.Add(cn);
            SDI.Import(cn, ccu);

            //CodeDomProvider csc = CodeDomProvider.CreateProvider("CSharp");
            //CompilerParameters cplist = new CompilerParameters();
            //cplist.GenerateExecutable = false;
            //cplist.GenerateInMemory = true;
            //cplist.ReferencedAssemblies.Add("System.dll");
            //cplist.ReferencedAssemblies.Add("System.XML.dll");
            //cplist.ReferencedAssemblies.Add("System.Web.Service.dll");
            //cplist.ReferencedAssemblies.Add("System.Data.dll");
            //CompilerResults cr = csc.CompileAssemblyFromDom(cplist, ccu);

            CSharpCodeProvider icc = new CSharpCodeProvider();
            CompilerParameters cplist = new CompilerParameters();
            cplist.GenerateExecutable = false;
            cplist.GenerateInMemory = true;
            cplist.ReferencedAssemblies.Add("System.dll");
            cplist.ReferencedAssemblies.Add("System.XML.dll");
            cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
            cplist.ReferencedAssemblies.Add("System.Data.dll");
            CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
            if (true == cr.Errors.HasErrors)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                {
                    sb.Append(ce.ToString());
                    sb.Append(System.Environment.NewLine);
                }
                throw new Exception(sb.ToString());
            }
            Assembly assembly = cr.CompiledAssembly;
            return assembly.GetType(NameSpace + "." + className, true, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pcUrl"></param>
        /// <param name="pcMethodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Task<object> CallMethodAsync(string pcUrl, string pcMethodName, object[] args)
        {
            LoggerManager.Logger.Info(pcUrl + "," + pcMethodName + "," + args[0] + "," +
                args[1] + "," + args[2] + "," + args[3] + "," + args[4] + "," + args[5] + "," + args[6] + "," + args[7] + "," + args[8] + "," + args[9]
                + "," + args[10] + "," + args[11]);
            if (!webserviceDic.Keys.Contains(pcUrl))
            {
                webserviceDic.Add(pcUrl,CreateWebServiceClient(pcUrl));
            }
            try
            {

                object obj = Activator.CreateInstance(webserviceDic[pcUrl]);
                MethodInfo mi = webserviceDic[pcUrl].GetMethod(pcMethodName);
                Func<object> fuc = () =>
                {
                    return mi.Invoke(obj, args);
                };
                Task<object> tObj = Task.Factory.StartNew(fuc);
                return tObj;
            }
            catch (Exception e){
                LoggerManager.Logger.Error("wsresultException" + e.InnerException);
                return null;
            }
        }
    }
}
