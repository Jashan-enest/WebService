using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


public class Product
{
    private string _productName;

    public string ProductName
    {
        get { return this._productName; }
        set { this._productName = value; } 
    }

	public Product()
	{
		
	}
}
