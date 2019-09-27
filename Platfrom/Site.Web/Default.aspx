<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PTMS.Web.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PTMS</title>
    <style type="text/css">
        html, body {
            height: 100%;
            overflow: hidden;
        }

        body {
            padding: 0;
            margin: 0;
        }

        #silverlightControlHost {
            height: 100%;
            width: 100%;
            text-align: center;
        }
    </style>
    <script type="text/javascript" src="Silverlight.js"></script>
    <script type="text/javascript">

        function CloseShell() {
            window.external.CloseShell();

        }
        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }

            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
                return;
            }

            var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";

            errMsg += "Code: " + iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " + args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }


        function GetLanguage() {
            var languageControl = document.getElementById("language");
            return languageControl.value;
        }
    </script>
    <script type="text/javascript">
        function reportHide() {
            try {
                for (var i = 0; i < window.frames.length ; i++) {
                    if (window.frames[i].CloseCander) {
                        window.frames[i].CloseCander();
                    }
                }
            }
            catch (exp) {

            }
        }

        function fnc_fullscreen() {
            var obj = new ActiveXObject("Wscript.shell");
            obj.SendKeys("{f11}");
        }

        function Full() {
            window.resizeTo(window.screen.availWidth, window.screen.availHeight);
            window.moveTo(0, 0);
        }
    </script>
</head>
<body onload="javascript:Full()">
    <form id="form1" runat="server" style="height: 100%">
        <div id="silverlightControlHost">
            <object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%" codebase="SDK/Silverlight.exe" id="silverlightControl">
                <param name="source" value="ClientBin/Site.xap" />
                <param name="onError" value="onSilverlightError" />
                <param name="background" value="white" />
                <param name="minRuntimeVersion" value="5.0.61118.0" />
                <param name="autoUpgrade" value="true" />
                <param name="windowless" value="false" />
                <param name="InitParams" value='<%=ServerConfigInfor %>' />
                <param name="enableHtmlAccess" value="true" />
                <a href="SDK/Silverlight.exe" style="text-decoration: none">
                    <img src="SDK/sliverlight.jpg" alt="Install Microsoft Silverlight" style="border-style: none" />
                </a>
            </object>
            <iframe id="_sl_historyFrame" style="visibility: hidden; height: 0px; width: 0px; border: 0px"></iframe>
        </div>
        <input type="hidden" id="language" runat="server" />
    </form>
</body>
</html>
