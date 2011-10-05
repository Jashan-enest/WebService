<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeUniteTrendGraph.aspx.cs"
    Inherits="WebRole1.WeUniteTrendGraph" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WeUnite Trend Graph</title>
    <script language="javascript" type="text/javascript" src="Scripts/FusionCharts.js"></script>
    <link href="jQuery/themes/base/jquery.ui.all.css" rel="Stylesheet" />
    <script src="jQuery/jquery-1.5.1.js" language="javascript" type="text/javascript"></script>
    <script src="jQuery/external/jquery.bgiframe-2.1.2.js" language="javascript" type="text/javascript"></script>
    <script src="jQuery/ui/jquery.ui.core.js" language="javascript" type="text/javascript"></script>
    <script src="jQuery/ui/jquery.ui.widget.js" language="javascript" type="text/javascript"></script>
    <script src="jQuery/ui/jquery.ui.mouse.js" language="javascript" type="text/javascript"></script>
    <script src="jQuery/ui/jquery.ui.draggable.js" language="javascript" type="text/javascript"></script>
    <script src="jQuery/ui/jquery.ui.position.js" language="javascript" type="text/javascript"></script>
    <script src="jQuery/ui/jquery.ui.resizable.js" language="javascript" type="text/javascript"></script>
    <script src="jQuery/ui/jquery.ui.dialog.js" language="javascript" type="text/javascript"></script>
    <script src="jQuery/ui/jquery.ui.datepicker.js" language="javascript" type="text/javascript"></script>
    <link rel="stylesheet" href="jQuery/demos.css" />
    <script language="javascript" type="text/javascript">
        function CallItemInframe(categoryId, eBaySiteId) {
            window.scrollBy(0, 600);
            document.getElementById('iframeItems').src = "WeUniteTrendItems.aspx?Category=" + categoryId + "&eBaySiteId=" + eBaySiteId + "&StartPrice=" + document.getElementById('txtSPrice').value + "&EndPrice=" + document.getElementById('txtEPrice').value + "&StartDate=" + document.getElementById('dtStart').value + "&EndDate=" + document.getElementById('dtEnd').value;
        }
    </script>
    <script language="javascript" type="text/javascript">
        function CallIframe(categoryId, eBaySiteId) {
            document.getElementById('frmItems').src = "WeUniteTopSubCategoriesGraph.aspx?Category=" + categoryId + "&eBaySiteId=" + eBaySiteId + "&StartPrice=" + document.getElementById('txtSPrice').value + "&EndPrice=" + document.getElementById('txtEPrice').value + "&StartDate=" + document.getElementById('dtStart').value + "&EndDate=" + document.getElementById('dtEnd').value;
        }
        function CallOverLay(url, title, eBayurl, Category, Price) {
            if (url != '') {
                document.getElementById('imgProduct').src = url;
            }
            else {
                document.getElementById('imgProduct').src = "Images/noImage.jpg";
                document.getElementById('imgProduct').alt = "No image found";
            }
            document.getElementById('spnTitle').innerHTML = title;
            document.getElementById('anceBayUrl').href = eBayurl;
            document.getElementById('anceBayUrl').target = "_blank";
            document.getElementById('hCategory').innerHTML = Category;
            document.getElementById('spnPrice').innerHTML = 'Approximately £' + Price;
            $(document).ready(function () {
                $("#Overlay").dialog({
                    width: 550,
                    modal: true,
                    zIndex: 3999,
                    draggable: false,
                    title: 'eBay Product Detail',
                    resizable: false,
                    close: function (event, ui) {
                        document.getElementById('dvHide').style.display = "block";
                    }
                });
            });
            document.getElementById('dvHide').style.display = "none";
            $("#Overlay").dialog('open');
        }
    </script>
    <script language="javascript" type="text/javascript">
        $(function () {
            $("#dtStart").datepicker();
            $("#dtEnd").datepicker();
        });
    </script>
    <script language="javascript" type="text/javascript">
        function validate() {
            // Validation for the Start Price and End Price
            if (document.getElementById('txtSPrice').value != '') {
                if (isNaN(document.getElementById('txtSPrice').value)) {
                    alert("Price must be a numeric.");
                    return false;
                }
                if (document.getElementById('txtEPrice').value != '') {
                    if (isNaN(document.getElementById('txtEPrice').value)) {
                        alert("Price must be a numeric.");
                        return false;
                    }
                }

                if (parseInt(document.getElementById('txtSPrice').value) >= parseInt(document.getElementById('txtEPrice').value)) {
                    alert('End Price can be equal or greater then the Start Price.');
                    return false;
                }
            }
            else {
                if (document.getElementById('txtEPrice').value != '') {
                    alert('Please enter the start price first.');
                    return false;
                }
            }

            // Validation for the Start Date and End Date
            var startDateSplit = document.getElementById('dtStart').value;
            var endDateSplit = document.getElementById('dtEnd').value;

            var dStartDate = new Date(startDateSplit.split('/')[2], parseInt(startDateSplit.split('/')[0]) - parseInt(1), startDateSplit.split('/')[1]);
            var dEndDate = new Date(endDateSplit.split('/')[2], parseInt(endDateSplit.split('/')[0]) - parseInt(1), endDateSplit.split('/')[1]);

            if (document.getElementById('dtStart').value != '') {
                if (document.getElementById('dtEnd').value == '')
                { }
                else {
                    if (dStartDate.getTime() > dEndDate.getTime()) {
                        alert('End Date must be greater than start date.');
                        return false;
                    }
                }
            }
            else {
                if (document.getElementById('dtEnd').value != '') {
                    alert('Please enter the start date first.');
                    return false;
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="width: 100%;">
            <div style="float: left; width: auto;">
                filter by price :
                <asp:TextBox ID="txtSPrice" runat="server" />&nbsp;&nbsp;
                <asp:TextBox ID="txtEPrice" runat="server" />
            </div>
            <div style="float: left; width: auto; margin-left: 225px;">
                filter by date :
                <asp:TextBox ID="dtStart" runat="server" />&nbsp;&nbsp;
                <asp:TextBox ID="dtEnd" runat="server" />
            </div>
            <div style="float: left; width: auto; margin-left: 15px; margin-top: -3px;">
                <asp:Button ID="btnSubmit" OnClientClick="return validate();" runat="server" Text=" submit "
                    OnClick="btnSubmit_Click" />
            </div>
            <div style="float: left; width: auto; margin-left: 100px;">
                select country :
                <asp:DropDownList ID="ddlCountry" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <br />
        <div id="dvHide" style="float: left;">
            <div style="float: left;">
                <b>eBay's Category</b>
                <asp:Literal ID="FCLiteral" runat="server"></asp:Literal>
            </div>
            <div style="float: left;">
                <iframe id="frmItems" frameborder="0" height="700px" width="700px"></iframe>
            </div>
        </div>
    </div>
    <div>
        <iframe id="iframeItems" frameborder="0" height="700px" width="700px"></iframe>
    </div>
    </form>
    <div id="Overlay" style="display: none;">
        <div style="float: left; width: 155px;">
            <img id="imgProduct" style="height: 150px; width: 150px;" />
        </div>
        <div style="float: right; width: 360px;">
            <h2 id="hCategory">
            </h2>
            <h2 id="spnTitle">
            </h2>
            <br />
            <b><span id="spnPrice" style="font-family: Arial;"></span></b>
            <br />
            <br />
            <a id="anceBayUrl">See more on eBay about product</a>
        </div>
    </div>
</body>
</html>
