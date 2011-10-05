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
using System.Data.SqlClient;
using Google.GData.GoogleBase;

public class DataAccess
{
    public static IList<Product> GetProducts(string criteria)
    {
        List<Product> list = new List<Product>();
        //string connectionString = "Server=ANUJ-PC\\SQLEXPRESS2008;Database=test;uid=sa;pwd=systems@123";
        //string query = "SELECT * FROM tbemp where ename LIKE @Criteria + '%'";
        //using (SqlConnection myConnection = new SqlConnection(connectionString))
        //{
        //    SqlCommand myCommand = new SqlCommand(query, myConnection);
        //    myCommand.Parameters.AddWithValue("@Criteria", criteria);
        //    myConnection.Open();
        //    SqlDataReader reader = myCommand.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        Product product = new Product();
        //        product.ProductName = reader["ename"] as String;
        //        list.Add(product);
        //    }
        //    myConnection.Close();
        //    myCommand.Dispose();
        //    reader.Close();
        //}
        GBaseService service = new GBaseService("Google-Tutorial-1.0", "1/VTK_D3r3i5IzrmZFLXKajjyafXGrA38opNl93fZh3oY");
        GBaseQuery query = new GBaseQuery(GBaseUriFactory.Default.SnippetsFeedUri);
        query.GoogleBaseQuery = criteria;
        try
        {
            GBaseFeed feed = service.Query(query);
            foreach (GBaseEntry entry in feed.Entries)
            {
                Product product = new Product();
                product.ProductName = entry.Title.Text as String;
                list.Add(product);
            }
            return list;
        }
        catch(Exception ex)
        {
            Product product = new Product();
            product.ProductName = "";
             list.Add(product);
             return list;
        }
    }
}
