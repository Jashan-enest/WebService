<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeUniteTrendItems.aspx.cs"
    Inherits="WebRole1.WeUniteTrendItems" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WeUnite Most Popular Item Trend Graph</title>
    <script language="javascript" type="text/javascript" src="Scripts/FusionCharts.js"></script>
    <script src="jQuery/jquery-1.5.1.js" language="javascript" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var varType;
        var varUrl;
        var varData;
        var varContentType;
        var varDataType;
        var varProcessData;

        //Generic function to call AXMX Service
        function CallService(Id) {

            $.ajax({
                type: varType, //GET or POST or PUT or DELETE verb
                url: varUrl, // Location of the service
                data: varData, //Data sent to server
                contentType: varContentType, // content type sent to server
                dataType: varDataType, //Expected data format from server
                processdata: varProcessData, //True or False
                success: function (msg) {//On Successfull service call
                    //alert(msg.d.ImageUrl);
                    ServiceSucceeded(msg.d);
                },
                error: ServiceFailed// When Service call fails
            });
        }

        function ServiceSucceeded(result) {
            var res = result.split('@');
            window.parent.CallOverLay(res[0], res[1], res[2], res[3], res[4]);
            varType = null; varUrl = null; varData = null; varContentType = null; varDataType = null; varProcessData = null;
        }

        function ServiceFailed(result) {
            alert('Service call failed: ' + result.status + '' + result.statusText);
            varType = null; varUrl = null; varData = null; varContentType = null; varDataType = null; varProcessData = null;
        }

        function eBayItemWebService(Id, eBaySiteId) {
            varType = "POST";
            varUrl = "eBayItemService/eBayAjaxCallService.asmx/eBayItem";
            //varUrl = "eBayItemService/eBayAjaxCallService.asmx/eBayItemCustom";
            varData = '{"ItemId": "' + Id + '","eBaySiteId": "' + eBaySiteId + '"}';
            varContentType = "application/json; charset=utf-8";
            varDataType = "json";
            varProcessData = true;
            CallService(Id);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="float: left;">
            <b>eBay's 5 top products in <span id="spnSubCategory"></span></b>
            <asp:Literal ID="FCLiteral" runat="server"></asp:Literal>
        </div>
    </div>
    </form>
</body>
</html>
