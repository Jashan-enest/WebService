<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeUniteTopSubCategoriesGraph.aspx.cs"
    Inherits="WebRole1.WeUniteTopSubCategoriesGraph" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>WeUnite Most Popular Item Trend Graph</title>
    <script language="javascript" type="text/javascript" src="Scripts/FusionCharts.js"></script>
    <script src="jQuery/jquery-1.5.1.js" language="javascript" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        //CallItemInframe
        function eBayItemWebService(subCategoryId, eBaySiteId) {
            parent.CallItemInframe(subCategoryId, eBaySiteId);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="float: left;">
            <b>eBay's 10 Top SubCategories In Selected Category</b>
            <asp:Literal ID="FCLiteral" runat="server"></asp:Literal>
        </div>
    </div>
    </form>
</body>
</html>
