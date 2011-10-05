using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.IO;


public class HtmlTableHelper
{
    public static string ConvertProductListToTable(IList<Product> list)
    {
        Table table = new Table();
        table.ID = "MyTable";
        table.CssClass = "TableStyle";
        if (list != null)
        {
            foreach (Product product in list)
            {
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                cell.Text = product.ProductName;

                row.Cells.Add(cell);
                table.Rows.Add(row);
            }
        }
       
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);

        table.RenderControl(htw);
        return sw.ToString();
    }
}
