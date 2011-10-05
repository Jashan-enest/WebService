<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Demo Auto-Suggest Callback</title>
    <link href="Site.css" rel="stylesheet" type="text/css" />
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
            font-size: large;
            font-weight: bold;
        }
        .style3
        {
            font-size: x-large;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;<%--<input type="text"  onkeyup ="return GetProducts(event)" id="txtSearch" name="txtSearch" runat="server" />--%>&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="TextBox2" onkeyup="return GetProducts(event)" runat="server" AutoCompleteType="Disabled"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Search..!" />
        &nbsp;<div id="results">
        </div>
    </div>
    &nbsp;<asp:Label ID="Label1" runat="server" Text="" 
        style="font-size: large; font-weight: 700"></asp:Label>
    <p style="margin-left: 40px">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:LinkButton ID="LinkButton2" runat="server" onclick="LinkButton2_Click">Previous 
        Page</asp:LinkButton>
&nbsp;
        <asp:Label ID="Label6" runat="server" Text=""></asp:Label>
&nbsp;&nbsp;&nbsp; Of&nbsp;
        <asp:Label ID="Label5" runat="server" Text=""></asp:Label>
&nbsp;
        <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">Next 
        Page</asp:LinkButton>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        onselectedindexchanged="GridView1_SelectedIndexChanged">
    <Columns>
    <asp:TemplateField HeaderText="Product Name" >
    <ItemTemplate>
        <asp:Label ID="Label2" runat="server" Text='<%#Eval("name") %>'></asp:Label>
       
        
   
   
    </ItemTemplate>
    </asp:TemplateField>   
    <asp:TemplateField HeaderText="Price">
    <ItemTemplate>
     <asp:Label ID="Label3" runat="server" Text='<%#Eval("Price") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField> 
     <asp:TemplateField HeaderText="Model">
    <ItemTemplate>
     <asp:Label ID="Label4" runat="server" Text='<%#Eval("model") %>'></asp:Label>
    </ItemTemplate>
    </asp:TemplateField> 
    
    <asp:TemplateField>
    <ItemTemplate>
    <asp:LinkButton ID ="lbtn1" Text="Select Product" CommandArgument='<%#Eval("UPC")%>' CommandName="select" runat="server"></asp:LinkButton>
    </ItemTemplate>
    </asp:TemplateField> 
    </Columns>
    </asp:GridView>
        </p>
    <span class="style1">
    <br />
    </span>
    <span class="style2">
    Detail From amazon:-</span><asp:Label ID="Label10" runat="server" 
        Text=""></asp:Label>
    <span class="style1"><br />
    <asp:GridView ID="grdamazon" runat="server" AutoGenerateColumns="False" 
        CellPadding="4" ForeColor="#333333" GridLines="None" Width="548px">
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="Image">
            <ItemTemplate>
            <asp:Image ID="img1" runat="server" ImageUrl='<%#Eval("StockPhotoURL") %>' />
            </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Title" HeaderText="Title" />
           <%-- <asp:BoundField DataField="Version" HeaderText="Version" />--%>
            <asp:BoundField DataField="Price" HeaderText="Price" />
            
        </Columns>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
    <br />
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p class="style2">
        Detail From Google:-<asp:Label ID="lblnotavalable" runat="server"></asp:Label>
    </p>
    <p class="style1">
    <div style=" height:298px ; width:585px ;  overflow :scroll"  >
    
    <span class="style1">
        <asp:GridView ID="grdgoogle" runat="server" AutoGenerateColumns="False" 
        CellPadding="4" ForeColor="#333333" GridLines="None" Width="548px">
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="Image">
            <ItemTemplate>
            <asp:Image ID="img1" runat="server" Height="60px" Width ="50px" ImageUrl='<%#Eval("StockPhotoURL") %>' />
            </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Title" HeaderText="Title" />
            <%--<asp:BoundField DataField="Version" HeaderText="Version" />--%>
            <asp:BoundField DataField="Price" HeaderText="Price" />
            
        </Columns>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
        </span>
    
    </div>
    </p>
    <br />
    <br />
    <span class="style3">Detail From eBay:-</span><asp:Label ID="Label9" 
        runat="server" Text=""></asp:Label>
    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
        CellPadding="4" ForeColor="#333333" GridLines="None" Width="554px" 
        Height="417px">
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <Columns>
            <asp:TemplateField HeaderText="Image">
            <ItemTemplate>
            <asp:Image ID="img2" Height="100px" Width ="100px" runat="server" 
                    ImageUrl='<%#Eval("StockPhotoURL") %>' />
            </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Title" HeaderText="Title" />
            <asp:BoundField DataField="Version" HeaderText="Version" />
            <asp:BoundField DataField="Price" HeaderText="Price" />
            
        </Columns>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    </form>
</body>
</html>

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

function GetProducts(e) 
{
var keynum
var keychar
var numcheck

if(window.event) // IE
{
keynum = e.keyCode
}
else if(e.which) // Netscape/Firefox/Opera
{
keynum = e.which
}
 //keychar= document.getElementById("txtSearch").value = selectedRow.childNodes[1].innerHTML;  
keychar = String.fromCharCode(keynum)
numcheck = /\d/

// If the down key is pressed
if(keynum == DOWN) 
{   
    MoveCursorDown();    
    return;
}

else if(keynum == UP) 
{   
    MoveCursorUp();    
    return; 
}

else if(keynum == ENTER) 
{
//alert(1);
//alert(selectedRow.childNodes[1]);
//alert(selectedRow.innerHTML);
    if(IsFireFox()) 
    {
     window.location ="Default.aspx?name="+selectedRow.childNodes[1].innerHTML+"";
    document.getElementById("TextBox2").value = selectedRow.childNodes[1].innerHTML;
    document.getElementById("HiddenField1").value=selectedRow.childNodes[1].innerHTML;
        (selectedRow.childNodes[1].innerHTML);
      
    }
    else 
    {
        document.getElementById("TextBox2").value = selectedRow.innerText; 
        document.getElementById("HiddenField1").value=selectedRow.innerText;
        window.location ="Default2.aspx?name="+selectedRow.innerText+"";
    }
    document.getElementById("results").innerHTML = '';
    
    // false is returned so that the postback won't occur when the return key is pressed
    return false;
}

if(keynum != DOWN && keynum != UP && keynum >= 65 && keynum <= 90) 
{
    //word=document.getElementById("txtSearch").value;
    word = word + keychar; 
}

else if(keynum == BACKSPACE) 
{
    
    word = word.substring(0,word.length-1); 
}

// Call the server side method

CallServer(document.getElementById("TextBox2").value,'');

}

function IsFireFox() 
{
    return (navigator.appName == 'Netscape'); 
}

function MoveCursorUp() 
{
    selectedRow = null; 
    table = document.getElementById("MyTable");
    
    if(table == null) return;
        
    rows = table.getElementsByTagName("TR");   
    
    if(index > 0) 
    {
      index--; 
      
      SetDefaultRowColor();
      selectedRow = rows[index];
      selectedRow.className = 'HighlightRow'      
    } 
}

function MoveCursorDown() 
{
    selectedRow = null; 
    table = document.getElementById("MyTable");
    
    if(table == null) return;
    
    rows = table.getElementsByTagName("TR");
        
    if(index < rows.length) 
    {   
              
        if(index < rows.length -1)
        { 
        index++;       
        SetDefaultRowColor();          
        selectedRow = rows[index];
        selectedRow.className = 'HighlightRow';   
        }
       
    } 
}

function SetDefaultRowColor() 
{
   for(i=0;i<rows.length;i++) 
    {
        rows[i].className = 'DefaultRowColor';
    }   
}


function RecieveServerData(response) 
{
    document.getElementById("results").innerHTML = response; 
}


</script>

