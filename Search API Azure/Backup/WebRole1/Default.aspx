<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="WebRole1._Default" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Demo Auto-Suggest Callback</title>
    <link href="Site.css" rel="stylesheet" type="text/css" />
    <%-- <link href="Site.css" rel="stylesheet" type="text/css" />--%>
    <script language="javascript" type="text/javascript">

        var word = '';
        var UP = 38;
        var DOWN = 40;
        var ENTER = 13;
        var index = -1;
        var TAB = 9;
        var BACKSPACE = 8;

        var table = null;
        var rows = null;

        var selectedRow = null;

        function GetProducts(e) {
            var keynum
            var keychar
            var numcheck

            if (window.event) // IE
            {
                keynum = e.keyCode
            }
            else if (e.which) // Netscape/Firefox/Opera
            {
                keynum = e.which
            }
            //keychar= document.getElementById("txtSearch").value = selectedRow.childNodes[1].innerHTML;  
            keychar = String.fromCharCode(keynum)
            numcheck = /\d/

            // If the down key is pressed
            if (keynum == DOWN) {
                MoveCursorDown();
                return;
            }

            else if (keynum == UP) {
                MoveCursorUp();
                return;
            }

            else if (keynum == ENTER) {
                //alert(1);
                //alert(selectedRow.childNodes[1]);
                //alert(selectedRow.innerHTML);
                if (IsFireFox()) {

                    window.location = "Default.aspx?name=" + selectedRow.childNodes[1].innerHTML + "";
                    document.getElementById("TextBox2").value = selectedRow.childNodes[1].innerHTML;
                    (selectedRow.childNodes[1].innerHTML);
                }

                else {
                    window.location = "Default.aspx?name=" + selectedRow.innerText;
                    document.getElementById("TextBox2").value = selectedRow.innerText;

                    //                document.getElementById("HiddenField1").value = selectedRow.innerText;
                    //                window.location = "Default2.aspx?name=" + selectedRow.innerText + "";
                }
                document.getElementById("results").innerHTML = '';

                // false is returned so that the postback won't occur when the return key is pressed
                return false;
            }

            if (keynum != DOWN && keynum != UP && keynum >= 65 && keynum <= 90) {
                //word=document.getElementById("txtSearch").value;
                word = word + keychar;
            }

            else if (keynum == BACKSPACE) {

                word = word.substring(0, word.length - 1);
            }

            // Call the server side method

            CallServer(document.getElementById("TextBox2").value, '');

        }

        function IsFireFox() {
            return (navigator.appName == 'Netscape');
        }

        function MoveCursorUp() {
            selectedRow = null;
            table = document.getElementById("MyTable");

            if (table == null) return;

            rows = table.getElementsByTagName("TR");

            if (index > 0) {
                index--;

                SetDefaultRowColor();
                selectedRow = rows[index];
                selectedRow.className = 'HighlightRow'
            }
        }

        function MoveCursorDown() {
            selectedRow = null;
            table = document.getElementById("MyTable");

            if (table == null) return;

            rows = table.getElementsByTagName("TR");

            if (index < rows.length) {

                if (index < rows.length - 1) {
                    index++;
                    SetDefaultRowColor();
                    selectedRow = rows[index];
                    selectedRow.className = 'HighlightRow';
                }

            }
        }

        function SetDefaultRowColor() {
            for (i = 0; i < rows.length; i++) {
                rows[i].className = 'DefaultRowColor';
            }
        }


        function RecieveServerData(response) {

            document.getElementById("results").innerHTML = response;
        }


    </script>
    <style type="text/css">
        #results
        {
            width: 1159px;
        }
        .style1
        {
            font-size: small;
            font-weight: bold;
        }
        .style2
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HyperLink runat="server" Target="_blank" Text="eBay Xml Parser" NavigateUrl="~/eBayXmlParser.aspx"></asp:HyperLink><br />
        <strong>&nbsp; <strong>Keyword:-</strong><asp:TextBox ID="TextBox2" onkeyup="return GetProducts(event)"
            runat="server" AutoCompleteType="Disabled"></asp:TextBox>
            <strong>Search From :</strong><asp:RadioButton ID="RadioButton1" runat="server" AutoPostBack="true"
                GroupName="a" OnCheckedChanged="RadioButton1_CheckedChanged" Text="Google" />
            <asp:RadioButton ID="RadioButton2" runat="server" AutoPostBack="true" GroupName="a"
                Text="Amazon" OnCheckedChanged="RadioButton2_CheckedChanged" />
            <asp:RadioButton ID="RadioButton3" runat="server" AutoPostBack="true" GroupName="a"
                Text="eBay" OnCheckedChanged="RadioButton3_CheckedChanged" />
            <asp:Label ID="Label11" runat="server" Visible="False" Text="Select Categories:-"
                Style="font-weight: 700"></asp:Label>
            <asp:DropDownList ID="DropDownList1" runat="server" Visible="false" Height="16px"
                Width="109px">
                <asp:ListItem>All</asp:ListItem>
                <asp:ListItem>Electronics</asp:ListItem>
                <asp:ListItem>Boooks</asp:ListItem>
                <asp:ListItem>Mobiles &amp; Accessories</asp:ListItem>
                <asp:ListItem>Music</asp:ListItem>
            </asp:DropDownList>
            <%--<input type="text"  onkeyup ="return GetProducts(event)" id="txtSearch" name="txtSearch" runat="server" />--%></strong>&nbsp;<asp:Button
                ID="Button1" runat="server" OnClick="Button1_Click" Text="Search..!" Height="25px"
                Width="86px" Style="font-weight: 700" />
        &nbsp;&nbsp;<div id="results">
        </div>
    </div>
    &nbsp;<asp:Label ID="Label1" runat="server" Text="" Style="font-size: medium; font-weight: 700"></asp:Label>
    <br />
    <br />
    &nbsp;
    <asp:Label ID="Label12" runat="server" Style="font-size: medium; font-weight: 700"></asp:Label>
    <br />
    <br />
    <asp:Label ID="Label14" runat="server" Style="font-size: large; color: #CC0000"></asp:Label>
    <p style="margin-left: 40px">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click" Visible="False">Previous 
        Page</asp:LinkButton>
        &nbsp;
        <asp:Label ID="Label6" runat="server" Visible="False"></asp:Label>
        &nbsp;&nbsp;&nbsp; :&nbsp;
        <asp:Label ID="Label5" runat="server" Visible="False"></asp:Label>
        &nbsp;
        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" Visible="False">Next 
        Page</asp:LinkButton>
        <asp:Panel ID="Panel2" runat="server" Visible="false">
            <div style="height: 319px; width: 840px; overflow: scroll">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                    DataKeyNames="name" Height="300px" Width="818px" CellPadding="2" ForeColor="Black"
                    GridLines="None" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px">
                    <AlternatingRowStyle BackColor="PaleGoldenrod" />
                    <Columns>
                        <asp:TemplateField HeaderText="Product Name">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%#Eval("name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product Price">
                            <ItemTemplate>
                                <asp:Label ID="Label33" runat="server" Text='<%#Eval("price") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%--CommandArgument='<%#Eval("UPC1")%>'--%>
                                <asp:LinkButton ID="lbtn1" Text="Select Product" CommandName="select" runat="server"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="Tan" />
                    <HeaderStyle BackColor="Tan" Font-Bold="True" />
                    <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                    <SortedAscendingCellStyle BackColor="#FAFAE7" />
                    <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                    <SortedDescendingCellStyle BackColor="#E1DB9C" />
                    <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                </asp:GridView>
            </div>
        </asp:Panel>
    </p>
    <asp:Panel ID="Panel1" runat="server" Visible="False">
        <asp:Label ID="Label15" runat="server" Style="font-weight: 700; color: #CC0000" Text="We have following offer for you::"></asp:Label>
        <span class="style1">
            <br />
            <br />
            <asp:Image ID="Image1" Height="141px" Width="124px" runat="server" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Image ID="Image2" Height="111px" Width="121px" runat="server" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Image ID="Image3" runat="server" Height="160px" Width="359px" />
            <br />
            <br />
            <asp:Label ID="lblamazon" runat="server" Style="color: #A55129; text-decoration: underline">Amazon</asp:Label>
            <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" CellPadding="3"
                Width="794px" Height="99px" BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None"
                BorderWidth="1px" CellSpacing="2">
                <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
                <Columns>
                    <asp:BoundField DataField="Title" HeaderText="Title" />
                    <asp:BoundField DataField="Price" HeaderText="Price" />
                </Columns>
                <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#FFF1D4" />
                <SortedAscendingHeaderStyle BackColor="#B95C30" />
                <SortedDescendingCellStyle BackColor="#F1E5CE" />
                <SortedDescendingHeaderStyle BackColor="#93451F" />
            </asp:GridView>
            <p>
                <asp:Label ID="lblgoogle" runat="server" Style="color: #A55129; text-decoration: underline">Google</asp:Label>
            </p>
            <div style="height: 286px; width: 903px; overflow: scroll">
                <asp:GridView ID="grdgoogle" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px"
                    CellPadding="3" CellSpacing="2" Height="271px" OnSorting="grdgoogle_Sorting1"
                    Width="884px">
                    <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
                    <Columns>
                        <asp:BoundField DataField="Title" HeaderText="Title" />
                        <asp:BoundField DataField="merchant" HeaderText="Merchant" />
                        <asp:BoundField DataField="Price" HeaderText="Price" />
                        <asp:BoundField DataField="unit" HeaderText="Price Unit" />
                    </Columns>
                    <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                    <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#FFF1D4" />
                    <SortedAscendingHeaderStyle BackColor="#B95C30" />
                    <SortedDescendingCellStyle BackColor="#F1E5CE" />
                    <SortedDescendingHeaderStyle BackColor="#93451F" />
                </asp:GridView>
            </div>
            <asp:Label ID="lbleBay" runat="server" Style="color: #A55129; text-decoration: underline">eBay</asp:Label>
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" BackColor="#DEBA84"
                BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2"
                Height="73px" Width="827px">
                <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
                <Columns>
                    <asp:BoundField DataField="Title" HeaderText="Title" />
                    <asp:BoundField DataField="Price" HeaderText="Price " />
                    <asp:BoundField DataField="unit" HeaderText="Price Unit" />
                </Columns>
                <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#FFF1D4" />
                <SortedAscendingHeaderStyle BackColor="#B95C30" />
                <SortedDescendingCellStyle BackColor="#F1E5CE" />
                <SortedDescendingHeaderStyle BackColor="#93451F" />
            </asp:GridView>
            <br />
            <br />
            <br />
            <br />
    </asp:Panel>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    </form>
</body>
</html>
