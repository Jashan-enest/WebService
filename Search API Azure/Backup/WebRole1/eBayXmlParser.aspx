<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="eBayXmlParser.aspx.cs"
    Inherits="WebRole1.eBay_Xml_Parser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>eBay Xml Parser</title>
    <style>
        .display
        {
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">

        function Validate() {
            if (document.getElementById('ddlCountries').value == -1) {
                alert("Please select the country");
                return false;
            }
            if (document.getElementById('rdoKeyWord').checked) {
                if (document.getElementById('txtKeyword').value == '') {
                    alert("Please enter the Keyword.");
                }
            }
        }

        function ShowDiv() {
            if (document.getElementById('rdoCategory').checked) {
                document.getElementById('dvCategory').style.display = "block";
                document.getElementById('dvKeyword').style.display = "none";
            }
            else {
                document.getElementById('dvCategory').style.display = "none";
                document.getElementById('dvKeyword').style.display = "block";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="dvControls" style="width: 100%">
        <div style="float: left; margin-top: 2px;">
            <asp:DropDownList ID="ddlCountries" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountries_SelectedIndexChanged">
                <asp:ListItem Value="-1">-- Select Country --</asp:ListItem>
                <asp:ListItem Value="0">eBay United States </asp:ListItem>
                <asp:ListItem Value="2">eBay Canada (English) </asp:ListItem>
                <asp:ListItem Value="3">eBay UK </asp:ListItem>
                <asp:ListItem Value="15">eBay Australia </asp:ListItem>
                <asp:ListItem Value="16">eBay Austria </asp:ListItem>
                <asp:ListItem Value="23">eBay Belgium (French) </asp:ListItem>
                <asp:ListItem Value="71">eBay France </asp:ListItem>
                <asp:ListItem Value="77">eBay Germany </asp:ListItem>
                <asp:ListItem Value="100">eBay Motors </asp:ListItem>
                <asp:ListItem Value="101">eBay Italy </asp:ListItem>
                <asp:ListItem Value="123">eBay Belgium (Dutch) </asp:ListItem>
                <asp:ListItem Value="146">eBay Netherlands </asp:ListItem>
                <asp:ListItem Value="186">eBay Spain </asp:ListItem>
                <asp:ListItem Value="193">eBay Switzerland </asp:ListItem>
                <asp:ListItem Value="201">eBay Hong Kong </asp:ListItem>
                <asp:ListItem Value="203">eBay India </asp:ListItem>
                <asp:ListItem Value="205">eBay Ireland </asp:ListItem>
                <asp:ListItem Value="207">eBay Malaysia </asp:ListItem>
                <asp:ListItem Value="210">eBay Canada (French) </asp:ListItem>
                <asp:ListItem Value="211">eBay Philippines </asp:ListItem>
                <asp:ListItem Value="212">eBay Poland </asp:ListItem>
                <asp:ListItem Value="216">eBay Singapore </asp:ListItem>
                <asp:ListItem Value="218">eBay Sweden </asp:ListItem>
            </asp:DropDownList>
        </div>
        <div style="float: left; margin-top: 2px;">
            <asp:RadioButton ID="rdoCategory" GroupName="a" Checked="true" onclick="return ShowDiv();"
                Text="Show by Category" runat="server" />
            <asp:RadioButton ID="rdoKeyWord" GroupName="a" Text="Want to enter keyword" onclick="return ShowDiv();"
                runat="server" />
        </div>
        <div style="float: right;">
            <a target="_blank" href="WeUniteTrendGraph.aspx">See this month popular item trend</a>
        </div>
    </div>
    <div style="width: 100%; float: left;">
        <div id="dvCategory" style="float: left; margin-top: 2px;">
            <asp:DropDownList ID="ddlCategories" runat="server">
                <asp:ListItem Value="-1">-- Select Category --</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div id="dvKeyword" style="float: left; margin-top: 2px; display: none;">
            <asp:TextBox ID="txtKeyword" runat="server" Width="175px"></asp:TextBox>
        </div>
        <div style="float: left">
            <asp:Button ID="btnSearch" runat="server" Text="Search..." OnClientClick="return Validate();"
                OnClick="btnSearch_Click" />
        </div>
    </div>
    <div style="width: 100%; float: left;">
        <asp:Label ID="lblError" Visible="false" ForeColor="Red" runat="server"></asp:Label>
        <asp:GridView ID="grdEbayData" runat="server" BorderColor="AliceBlue" BorderWidth="1"
            Visible="false" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="headerURL" Text="Images"></asp:Label></HeaderTemplate>
                    <ItemTemplate>
                        <asp:Image ID="Image1" ImageUrl='<%# Eval("GalleryURL") %>' AlternateText="No image found"
                            runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Label runat="server" ID="headerDetails" Text="Details"></asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <b>ItemID : </b>
                        <asp:Label ID="lblItemId" runat="server" Text='<%# Eval("ItemID") %>'></asp:Label>
                        <br />
                        <b>Title : </b>
                        <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                        <br />
                        <b>BidCount : </b>
                        <asp:Label ID="lblBidCount" runat="server" Text='<%# Eval("BidCount") %>'></asp:Label>
                        <br />
                        <b>Listing Type : </b>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("ListingType") %>'></asp:Label>
                        <br />
                        <b>PrimaryCategoryName : </b>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("PrimaryCategoryName") %>'></asp:Label>
                        <br />
                        <b>ItemURLForNaturalSearch : </b>
                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("ViewItemURLForNaturalSearch") %>'></asp:Label>
                        <br />
                        <b>ListingStatus : </b>
                        <asp:Label ID="Label4" runat="server" Text='<%# Eval("ListingStatus") %>'></asp:Label>
                        <br />
                        <b>ConvertedCurrentPrice : </b>
                        <%# Eval("ConvertedCurrencyID") %>
                        -
                        <asp:Label ID="Label5" runat="server" Text='<%# Eval("ConvertedCurrentPrice") %>'></asp:Label>
                        <br />
                        <b>WatchCount : </b>
                        <asp:Label ID="Label6" runat="server" Text='<%# Eval("WatchCount") %>'></asp:Label>
                        <br />
                        <b>EndTime : </b>
                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("EndTime") %>'></asp:Label>
                        <br />
                        <u><b>Shipping Cost Summary</b> </u>
                        <br />
                        <b>ShippingType : </b>
                        <asp:Label ID="Label8" runat="server" Text='<%# Eval("ShippingType") %>'></asp:Label>
                        <br />
                        <b>ShippingServiceCost : </b>
                        <%# Eval("CurrencyID") %>
                        -
                        <asp:Label ID="Label9" runat="server" Text='<%# Eval("ShippingServiceCost") %>'></asp:Label>
                        <br />
                        <b>ListedShippingServiceCost : </b>
                        <%# Eval("ListShippingCurrencyID")%>
                        -
                        <asp:Label ID="Label10" runat="server" Text='<%# Eval("ListedShippingServiceCost") %>'></asp:Label>
                        <br />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <h2>
                    No data found.</h2>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
<script language="javascript" type="text/javascript">
    if (document.getElementById('rdoCategory').checked) {
        document.getElementById('dvCategory').style.display = "block";
        document.getElementById('dvKeyword').style.display = "none";
    }
    else {
        document.getElementById('dvCategory').style.display = "none";
        document.getElementById('dvKeyword').style.display = "block";
    }
</script>
