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
using System.ServiceModel;
using WebRole1.ServiceReference1;
using System.Security.Cryptography;
//using WebRole1.com.ebay.developer;
using System.Net;
using System.Xml;
using System.ServiceModel.Channels;
using WebRole1.ServiceReference2;






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
        GBaseQuery query = new GBaseQuery("http://www.google.co.uk/base/feeds/snippets?q="+criteria+"&orderby=price%28float%20GBP%29&sortorder=ascending");
        //GBaseUriFactory.Default.SnippetsFeedUri
        //query.GoogleBaseQuery = criteria;
        //query.ExtraParameters = "Target Country=GB";

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
        catch (Exception ex)
        {
            Product product = new Product();
            product.ProductName = "";
            list.Add(product);
            return list;
        }

        //AMAZON


        //string accessKeyId = "AKIAI7X5QEXQNWUELB7A";
        ////"AKIAJ7BF65PQMKI4RDQQ";
        //string secretKey = "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";
        ////"KE72/nPGj1xQhNq7vQvPgg3vLx90UMy5nqoSjRnY";
        //BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        //binding.MaxReceivedMessageSize = int.MaxValue;
        //AWSECommerceServicePortTypeClient client = new AWSECommerceServicePortTypeClient();
        //client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(accessKeyId, secretKey));
        //ItemSearchRequest request = new ItemSearchRequest();

        //request.SearchIndex = "All";

        //// request.ItemPage = Convert.ToString(page);
        ////request.City="London";
        //request.Keywords = criteria;


        ////request.State = "Uk";

        //request.ResponseGroup = new string[] 
        //{
        //// "Small", "Images", "Medium", "EditorialReview", "BrowseNodes",
        //};
        //request.RelationshipType = new string[] 
        //{};
        // //"Tracks","AuthorityTitle", "DigitalMusicArranger",
        // // "DigitalMusicComposer", "DigitalMusicConductor", "DigitalMusicEnsemble", 
        // // "DigitalMusicLyricist", "DigitalMusicPerformer", 
        // // "DigitalMusicPrimaryArtist", "DigitalMusicProducer", 
        // //  "DigitalMusicSongWriter", "Episode", "Season" 

        //ItemSearch itemSearch = new ItemSearch();
        //itemSearch.Request = new ItemSearchRequest[] { request };
        //itemSearch.AWSAccessKeyId = accessKeyId;
        //// request.ItemPage = Convert.ToString(page);

        //try
        //{
        //    ItemSearchResponse response = client.ItemSearch(itemSearch);

        //    if (response.Items[0].Item != null)
        //    {


        //        //ASIN = response.Items[0].Item[0].ASIN;
        //        for (int i = 0; i < response.Items[0].Item.Length; i++)
        //        {
        //            if (response.Items[0].Item[i].ItemAttributes.Title != null)
        //            {
        //                Product product = new Product();
        //                product.ProductName = response.Items[0].Item[i].ItemAttributes.Title;
        //                list.Add(product);
        //            }
        //        }
        //    }
        //}
        //catch
        //{
        //    Product product = new Product();
        //    product.ProductName = "";
        //    list.Add(product);
        //    return list;
        //}
        //return list;
    }     
}