namespace Gsafety.PTMS.Analysis.Alarm
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Web.Services.Description;
    using Microsoft.CSharp;
    using System.Threading.Tasks;

    /// <summary>
    ///     WebServiceHelper
    /// </summary>
    public class WebServiceHelper
    {
        /// <summary>
        ///     call webservice
        ///     demo nothing parameter：object obj = InvokeWebService("http://localhost/HardwareTerminalSevice.asmx",
        ///     "VideoAppealWebService", "HardwareTerminalSevice", "HelloWorld", new object[] {  });
        ///     demo have parameter：object obj = InvokeWebService("http://localhost/HardwareTerminalSevice.asmx",
        ///     "VideoAppealWebService", "HardwareTerminalSevice", "HelloWorld1", new object[] {  msg});
        /// </summary>
        /// <param name="url"></param>
        /// <param name="namespace"></param>
        /// <param name="classname"></param>
        /// <param name="methodname"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object InvokeWebService(string url, string @namespace, string classname, string methodname,
            object[] args)
        {
            try
            {
                //GetWSDL
                var wc = new WebClient();
                Stream stream = wc.OpenRead(url + "?WSDL");
                ServiceDescription sd = ServiceDescription.Read(stream);
                var sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                var cn = new CodeNamespace(@namespace);


                var ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                var icc = new CSharpCodeProvider();


                var cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");

                CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
                if (cr.Errors.HasErrors)
                {
                    var sb = new StringBuilder();
                    foreach (CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce);
                        sb.Append(Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }

                Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                object obj = Activator.CreateInstance(t);
                MethodInfo mi = t.GetMethod(methodname);

                return mi.Invoke(obj, args);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }
    }
}