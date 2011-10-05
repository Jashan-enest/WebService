using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Google.GData.GoogleBase;
using System.ServiceModel;
using ServiceReference1;
using System.Security.Cryptography;
using ServiceReference2;
using ServiceReference1;
using System.Net;
using System.Xml;

public partial class _Default : System.Web.UI.Page, ICallbackEventHandler
{
    String ASIN = null;
    List<String> tracks = new List<string>();
    IList<Product> list = null;
    static Int32 page=1;
    protected void Page_Load(object sender, EventArgs e)
   {

    
       if (Request.QueryString["name"] != null)
       {
           
           TextBox2.Text = Request.QueryString["name"].ToString();
       }
            RegisterClientCallbacks();
       
    }

    private void RegisterClientCallbacks()
    {
        string callbackRef = ClientScript.GetCallbackEventReference(this, "arg", "RecieveServerData", "context");

        string script = String.Empty;

        if (!ClientScript.IsClientScriptBlockRegistered("CallServer"))
        {
            script = "function CallServer(arg,context) { " + callbackRef + "}";

            ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", script, true);
        }
    }   

    public string GetCallbackResult()
    {
        return HtmlTableHelper.ConvertProductListToTable(list); 
    }

    public void RaiseCallbackEvent(string eventArgument)
    {
        if (String.IsNullOrEmpty(eventArgument))
        {
            list = null;
        }

        if (String.IsNullOrEmpty(eventArgument)) return;
        
        list = DataAccess.GetProducts(eventArgument); 

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        amazonsrch();
        
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string a;
       
        
        a = ((LinkButton)(GridView1.SelectedRow.FindControl("lbtn1"))).CommandArgument;



        amazon(a);
        google(a);
        ebay(a);

           
    }

    private void amazonsrch()
    { 
        string accessKeyId = "AKIAJ7BF65PQMKI4RDQQ";
        string secretKey = "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";

        BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        binding.MaxReceivedMessageSize = int.MaxValue;
        AWSECommerceServicePortTypeClient client = new AWSECommerceServicePortTypeClient();

        client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(accessKeyId, secretKey));

        ItemSearchRequest request = new ItemSearchRequest();
        request.SearchIndex = "All";
        request.ItemPage =Convert.ToString(page);

        request.Keywords = TextBox2.Text;
        

        request.ResponseGroup = new string[] 
        {
         "Small", "Images", "Medium", "EditorialReview", "BrowseNodes",
        };
        request.RelationshipType = new string[] 
        { "Tracks","AuthorityTitle", "DigitalMusicArranger",
          "DigitalMusicComposer", "DigitalMusicConductor", "DigitalMusicEnsemble", 
          "DigitalMusicLyricist", "DigitalMusicPerformer", 
          "DigitalMusicPrimaryArtist", "DigitalMusicProducer", 
           "DigitalMusicSongWriter", "Episode", "Season" };

        ItemSearch itemSearch = new ItemSearch();
        itemSearch.Request = new ItemSearchRequest[] { request };
        itemSearch.AWSAccessKeyId = accessKeyId;
        request.ItemPage =Convert.ToString(page);

        try
        {
            ItemSearchResponse response = client.ItemSearch(itemSearch);

            if (response.Items[0].Item != null)
            {
                DataTable tb = new DataTable();
                tb.Columns.Add("name");
                tb.Columns.Add("UPC");
                tb.Columns.Add("Price");
                tb.Columns.Add("model");
                DataRow r;
                //ASIN = response.Items[0].Item[0].ASIN;
                for (int i = 0; i < response.Items[0].Item.Length; i++)
                {
                    r = tb.NewRow();
                    if (response.Items[0].Item[i].ItemAttributes.Title != null)
                    {
                        r[0] = response.Items[0].Item[i].ItemAttributes.Title;
                    }
                    if (response.Items[0].Item[i].ItemAttributes.UPC !=null)
                    {
                        r[1] = response.Items[0].Item[i].ItemAttributes.UPC;
                    }

                    if (response.Items[0].Item[i].OfferSummary.LowestUsedPrice != null)
                    {
                        r[2] = response.Items[0].Item[i].OfferSummary.LowestUsedPrice.FormattedPrice;
                    }
                    if (response.Items[0].Item[i].ItemAttributes.Model != null)
                    {
                        r[3] = response.Items[0].Item[i].ItemAttributes.Model;
                    }

                    tb.Rows.Add(r);


                }
                Label1.Visible = false;
                GridView1.DataSource = tb;
                GridView1.DataBind();
                Label6.Text = page.ToString();
                Label5.Text = response.Items[0].TotalPages;




                //results.Add(entry.Title.Text);

                //Response.Write(entry.GBaseAttributes.ItemType +
                //                         ": " + entry.Title.Text +
                //                         " - " + entry.Id.Uri);
                //    if (entry.GBaseAttributes.Price != null)
                //    {
                //       Label1.Text = entry.GBaseAttributes.Price.Value.ToString();
                //    }
                //    if (entry.GBaseAttributes.ImageLinks.Length > 0)
                //    {
                //        Image1.ImageUrl = entry.GBaseAttributes.ImageLinks[0].ToString();
                //    }

                //}
                //GridView1.DataSource = tb;
                //GridView1.DataBind();
            }
            else
            {

                Label1.Text = "Item Not Available";
            }
        }
        catch
        {
            Label1.Text = "Please Try Again Later";
        
        }
            
            
            
    
    
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Label5.Text))
        {
            page = page + 1;
            amazonsrch();
        }
    
    }
    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Label5.Text))
        {
            page = page - 1;
            amazonsrch();
        }

    }

    private void amazon(string upccod)
    {


        string accessKeyId = "AKIAJ7BF65PQMKI4RDQQ";
        string secretKey = "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";
        System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding(System.ServiceModel.BasicHttpSecurityMode.Transport);
        //binding.MaxReceivedMessageSize = 81474836472323326;
        AWSECommerceServicePortTypeClient client = new AWSECommerceServicePortTypeClient();
        client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(accessKeyId, secretKey));
        String[] responseGroup = {
	        "Small",
	        "Images",
	        "Reviews",
	        "Tracks",
	        "Medium",
	        "EditorialReview",
	        "BrowseNodes",
	        "ItemAttributes",
	        "OfferFull",
	        "Offers"
                };
        // string ItemID = "610583163926";
        ItemLookupRequest itemLookupRequest = new ItemLookupRequest();
        itemLookupRequest.ItemId = new string[] { upccod };
        itemLookupRequest.SearchIndex = "All";
        itemLookupRequest.IdTypeSpecified = true;
        itemLookupRequest.IdType = ItemLookupRequestIdType.UPC;
        itemLookupRequest.MerchantId = "All";
        //**
        itemLookupRequest.Condition = ServiceReference1.Condition.All;
        itemLookupRequest.ResponseGroup = responseGroup;
        ItemLookup itemLookup = new ItemLookup();
        itemLookup.AWSAccessKeyId = accessKeyId;
        itemLookup.Request = new ItemLookupRequest[] { itemLookupRequest };
        ItemLookupResponse itemLookupResponse = new ItemLookupResponse();
        try
        {
            itemLookupResponse = client.ItemLookup(itemLookup);

            DataSet ds = new DataSet();
            ds.Tables.Add();
            ds.Tables[0].Columns.Add("Title");
            ds.Tables[0].Columns.Add("StockPhotoURL");
            //ds.Tables[0].Columns.Add("Version");
            ds.Tables[0].Columns.Add("Price");
            DataRow dr;
            dr = ds.Tables[0].NewRow();
            if (itemLookupResponse.Items[0].Item != null)
            {
                if (itemLookupResponse.Items[0].Item[0].ItemAttributes.Title != null)
                {
                    dr["Title"] = itemLookupResponse.Items[0].Item[0].ItemAttributes.Title;
                }
                if (itemLookupResponse.Items[0].Item[0].MediumImage.URL != null)
                {
                    dr["StockPhotoURL"] = itemLookupResponse.Items[0].Item[0].MediumImage.URL;
                }
                //dr["Version"] = myXMLDocument.GetElementsByTagName("Version").Item(0).InnerText;
                if (itemLookupResponse.Items[0].Item[0].OfferSummary.LowestUsedPrice != null)
                {
                    dr["Price"] = itemLookupResponse.Items[0].Item[0].OfferSummary.LowestUsedPrice.FormattedPrice;
                }
                ds.Tables[0].Rows.Add(dr);

                grdamazon.DataSource = ds;
                grdamazon.DataBind();
                Label10.Visible = false;

            }
        }
        catch
        {
            Label10.Text = "Item Not Avalable ...Please Try Again Later";
        
        }

        //if (itemLookupResponse.Items[0].Item[0].MediumImage != null)
        //try
        //{
        //    Image2.ImageUrl = itemLookupResponse.Items[0].Item[0].MediumImage.URL;
        //}
        //catch
        //{ 
        
        //}
        ////if (itemLookupResponse.Items[0].Item[0].OfferSummary.LowestUsedPrice != null)
        //try
        //{
        //    lblprc.Text = itemLookupResponse.Items[0].Item[0].OfferSummary.LowestUsedPrice.FormattedPrice;

        //}
        //catch
        //{ }
        ////if (itemLookupResponse.Items[0].Item[0].OfferSummary.LowestCollectiblePrice != null)

        //try
        //{
        //    Label7.Text = itemLookupResponse.Items[0].Item[0].OfferSummary.LowestCollectiblePrice.FormattedPrice;
        //}
        //catch { }
        ////if (itemLookupResponse.Items[0].Item[0].OfferSummary.LowestCollectiblePrice != null)
        //try
        //{
        //    Label7.Text = itemLookupResponse.Items[0].Item[0].OfferSummary.LowestCollectiblePrice.FormattedPrice;
        //}
        //catch { }
        ////if (itemLookupResponse.Items[0].Item[0].ItemAttributes.Title != null)
        //try
        //{
        //    lbltit.Text = itemLookupResponse.Items[0].Item[0].ItemAttributes.Title;
        //    // item(0).ItemAttributes.ListPrice.FormattedPrice
        //}
        //catch { }
    }

    private void google(string upccode)
    {
        GBaseService service = new GBaseService("Google-Tutorial-1.0", "1/VTK_D3r3i5IzrmZFLXKajjyafXGrA38opNl93fZh3oY");
        GBaseQuery query = new GBaseQuery(GBaseUriFactory.Default.SnippetsFeedUri);
        
        query.GoogleBaseQuery = upccode;


        try
        {
            GBaseFeed feed = service.Query(query);
            List<string> aa = new List<string>();
            DataSet ds = null;
            if (feed.Entries.Count>0)
            {
                ds = new DataSet();
                   
                    ds.Tables.Add();
                    ds.Tables[0].Columns.Add("Title");
                    ds.Tables[0].Columns.Add("StockPhotoURL");
                    //ds.Tables[0].Columns.Add("Version");
                    ds.Tables[0].Columns.Add("Price");
                foreach (GBaseEntry entry in feed.Entries)
                {
                    
                    DataRow dr;
                    dr = ds.Tables[0].NewRow();
                    if (feed.Entries[0].Title.Text != null)
                    {
                        dr["Title"] = feed.Entries[0].Title.Text.ToString();
                    }
                    if (entry.GBaseAttributes.ImageLinks.Length>0)
                    {
                        dr["StockPhotoURL"] = entry.GBaseAttributes.ImageLinks[0].ToString();
                    }
                    //dr["Version"] = myXMLDocument.GetElementsByTagName("Version").Item(0).InnerText;

                    if (entry.GBaseAttributes.Price != null)
                    {
                        dr["Price"] = entry.GBaseAttributes.Price.Value.ToString();
                    }
                    ds.Tables[0].Rows.Add(dr);


                    //if (entry.Title.Text != null)
                    //{
                    //    lbltit2.Text = feed.Entries[0].Title.Text.ToString();
                    //    lblnotavalable.Visible = false;
                    //}
                    //if (entry.GBaseAttributes.ImageLinks.Length > 0)
                    //{
                    //    // Image3.ImageUrl= feed.Entries[0].Links[0].AbsoluteUri;
                    //    Image3.ImageUrl = entry.GBaseAttributes.ImageLinks[0].ToString();
                    //}
                    //if (entry.GBaseAttributes.Price != null)
                    //{

                    //    aa.Add(entry.GBaseAttributes.Price.Value.ToString());
                    //    lblprc2.Text = entry.GBaseAttributes.Price.Value.ToString();
                    //}
                    //break;

                    //Label5.Text =
                    //(new System.Linq.SystemCore_EnumerableDebugView(entry.GBaseAttributes)).Items[10];



                    //Product product = new Product();
                    //product.ProductName = entry.Title.Text as String;

                }


                 grdgoogle.DataSource = ds;
            grdgoogle.DataBind();
            grdgoogle.Visible = true;
            lblnotavalable.Visible = false;
            }
            else
            {
                lblnotavalable.Text = "Item Not Available";
                grdgoogle.Visible = false;

            }

           


        }
        catch
        {
            lblnotavalable.Visible = true;
            lblnotavalable.Text = "Item Not Available";
            grdgoogle.Visible = false;
        
        }
       // GridView2.DataSource = aa;
       // GridView2.DataBind();
    }




    private void ebay(string upc)
    {
        String jsonResponse = String.Empty;
        HttpWebRequest myHttpWebRequest = null;     //Declare an HTTP-specific implementation of the WebRequest class.
        HttpWebResponse myHttpWebResponse = null;   //Declare an HTTP-specific implementation of the WebResponse class
        XmlDocument myXMLDocument = null;           //Declare XMLResponse document
        XmlTextReader myXMLReader = null;
        string url = "http://open.api.ebay.com/shipping?version=517&appid=enest5985-d029-4f10-94a2-e2f29a83cef&callname=FindProducts&ProductID.Type=UPC&ProductID.Value=" + upc + "&IncludeSelector=SellerInfo";
        //525  DomainHistogram, Details, Items, ItemSpecifics or SellerInfo.
        //Create Request 014633155488
        myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
        myHttpWebRequest.Method = "GET";
        myHttpWebRequest.ContentType = "text/xml; encoding='utf-8'";
        //Get Response
        myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();


        myXMLDocument = new XmlDocument();

        myXMLReader = new XmlTextReader(myHttpWebResponse.GetResponseStream());
        XmlReader red = myXMLReader;
        myXMLDocument.Load(myXMLReader);

        List<EbayClass> lstebay = new List<EbayClass>();
        EbayClass objebay = new EbayClass();

        try
        {
            objebay.Title = myXMLDocument.GetElementsByTagName("Title").Item(0).InnerText;


            objebay.Image = myXMLDocument.GetElementsByTagName("StockPhotoURL").Item(0).InnerText;


            objebay.Version = myXMLDocument.GetElementsByTagName("Version").Item(0).InnerText;


            objebay.Price = myXMLDocument.GetElementsByTagName("ConvertedCurrentPrice").Item(0).InnerText;
            Label9.Text = "";



            lstebay.Add(objebay);

            DataSet ds = new DataSet();
            ds.Tables.Add();
            ds.Tables[0].Columns.Add("Title");
            ds.Tables[0].Columns.Add("StockPhotoURL");
            ds.Tables[0].Columns.Add("Version");
            ds.Tables[0].Columns.Add("Price");
            DataRow dr;
            dr = ds.Tables[0].NewRow();
            dr["Title"] = myXMLDocument.GetElementsByTagName("Title").Item(0).InnerText;
            dr["StockPhotoURL"] = myXMLDocument.GetElementsByTagName("StockPhotoURL").Item(0).InnerText;
            dr["Version"] = myXMLDocument.GetElementsByTagName("Version").Item(0).InnerText;
            dr["Price"] = myXMLDocument.GetElementsByTagName("ConvertedCurrentPrice").Item(0).InnerText;
            ds.Tables[0].Rows.Add(dr);



            GridView2.DataSource = ds;
            GridView2.DataBind();
            GridView2.Visible = true;
        }
        catch(Exception ex)
        {
            GridView2.Visible = false;
            Label9.Text = "Item Not Alailable in eBay";
        
        }

    }

}
