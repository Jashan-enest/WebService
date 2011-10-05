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
using WebRole1.ServiceReference1;
using System.Security.Cryptography;
//using WebRole1.com.ebay.developer;
using System.Net;
using System.Xml;
using System.ServiceModel.Channels;
using WebRole1.ServiceReference2;
using System.Text.RegularExpressions;
using System.Text;
using System.ServiceModel.Dispatcher;
namespace WebRole1
{
    public partial class _Default : System.Web.UI.Page, ICallbackEventHandler
    {

        String ASIN = null;
        List<String> tracks = new List<string>();
        IList<Product> list = null;
        static Int32 page = 1;
        string d = null;
        DataSet ds = new DataSet();
        Int32 next = 0;
        Int32 nextimage = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            TextBox2.Focus();
            if (Page.IsPostBack == false)
            {
                RadioButton3.Checked = true;
            }

            if (Request.QueryString["name"] != null)
            {
                if (TextBox2.Text == "")
                {
                    TextBox2.Text = Request.QueryString["name"].ToString();
                }
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
            Label14.Visible = false;
            Panel2.Visible = true;
             Panel1.Visible = false;
            if (RadioButton1.Checked == true)
            {
                googlesrch(TextBox2.Text);
            }
            if (RadioButton2.Checked == true)
            {
                amazonsrch(TextBox2.Text);
            }
            if (RadioButton3.Checked == true)
            {
                ebaysearch(TextBox2.Text);
            }    
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Panel1.Visible = false;
            string a = ((LinkButton)(GridView1.SelectedRow.FindControl("lbtn1"))).CommandArgument;
            d = GridView1.SelectedDataKey.Value.ToString();
            if (!string.IsNullOrEmpty(a))
            {
                //amazon(a);
                google(a);
               // ebay(a);
            }
            else
            {
                savekeyword(d);
                amazonsrchbyekeyword(d);
                google(d);
                ebaynew(d);
               // saveresult();                
            }

        }
        private void amazonsrch(string keyword)
        {
            string accessKeyId = "AKIAI7X5QEXQNWUELB7A";
            //"AKIAJ7BF65PQMKI4RDQQ";
            //
            string secretKey = "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";
            // "KE72/nPGj1xQhNq7vQvPgg3vLx90UMy5nqoSjRnY";
            //
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.MaxReceivedMessageSize = int.MaxValue;
            AWSECommerceServicePortTypeClient client = new AWSECommerceServicePortTypeClient();
            Label12.Text = client.Endpoint.ListenUri.AbsoluteUri;
            client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(accessKeyId, secretKey));
            ItemSearchRequest request = new ItemSearchRequest();
            request.SearchIndex = DropDownList1.SelectedItem.Text;

            request.ItemPage = Convert.ToString(page);

            request.Keywords = keyword;

            //Creating signature



            string operation = Regex.Match("http://soap.amazon.com/ItemSearch", "[^/]+$").ToString();
            DateTime now = DateTime.UtcNow;
            string timestamp = now.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string signMe = operation + timestamp;
            byte[] bytesToSign = Encoding.UTF8.GetBytes(signMe);

            // sign the data

            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            HMAC hmacSha256 = new HMACSHA256(secretKeyBytes);
            byte[] hashBytes = hmacSha256.ComputeHash(bytesToSign);
            string signature = Convert.ToBase64String(hashBytes);



            Label12.Text = "http://ecs.amazonaws.co.uk/onca/xml?AWSAccessKeyId=AKIAI7X5QEXQNWUELB7A&Operation=ItemSearch&SearchIndex=All&Keywords=" + keyword + "&ResponseGroup=Small,Images,Medium,EditorialReview,BrowseNodes&Service=AWSECommerceService&Timestamp=" + timestamp + "&Signature=" + signature + "";

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
            request.ItemPage = Convert.ToString(page);
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
                    tb.Columns.Add("UPC1");
                    DataRow r;
                    for (int i = 0; i < response.Items[0].Item.Length; i++)
                    {
                        r = tb.NewRow();
                        if (response.Items[0].Item[i].ItemAttributes.UPC != null)
                        {
                            r[1] = "Available";
                            r[4] = response.Items[0].Item[i].ItemAttributes.UPC;
                        }
                        else
                        {
                            r[1] = "Not Available..!";
                        }
                        if (response.Items[0].Item[i].OfferSummary.LowestUsedPrice != null)
                        {
                            r[2] = response.Items[0].Item[i].OfferSummary.LowestUsedPrice.FormattedPrice;
                        }
                        if (response.Items[0].Item[i].ItemAttributes.Title != null)
                        {
                            r[0] = response.Items[0].Item[i].ItemAttributes.Title;
                        }
                        if (response.Items[0].Item[i].ItemAttributes.Model != null)
                        {
                            r[3] = response.Items[0].Item[i].ItemAttributes.Model;
                        }
                        tb.Rows.Add(r);
                    }
                    Label1.Visible = false;
                    GridView1.Visible = true;
                    GridView1.DataSource = tb;
                    GridView1.DataBind();
                    Label6.Text = page.ToString();
                    Label5.Text = response.Items[0].TotalPages;
                }
                else
                {
                    Label1.Visible = true;
                    Label1.Text = "Item Not Available";
                    Panel2.Visible = false;
                }
            }
            catch
            {
                Label1.Text = "Please Try Again Later";
                Panel2.Visible = false;
            }
        }

        private void googlesrch(string keyword)
        {
            GBaseService service = new GBaseService("Google-Tutorial-1.0", "1/VTK_D3r3i5IzrmZFLXKajjyafXGrA38opNl93fZh3oY");
            GBaseQuery query = new GBaseQuery("http://www.google.co.uk/base/feeds/snippets?q=" + keyword + "&orderby=price%28float%20GBP%29&sortorder=ascending");
            //GBaseUriFactory.Default.SnippetsFeedUri
            

            //query.ExtraParameters = "Target Country=GB";

            //query.GoogleBaseQuery = keyword;
            try
            {
                Label12.Text = query.Uri.OriginalString;
                GBaseFeed feed = service.Query(query);
                List<string> aa = new List<string>();
                DataSet ds1 = null;
                if (feed.Entries.Count > 0)
                {
                    ds1 = new DataSet();
                    ds1.Tables.Add();
                    ds1.Tables[0].Columns.Add("name");                    
                    ds1.Tables[0].Columns.Add("Price");

                    Int32 i = 0;
                    foreach (GBaseEntry entry in feed.Entries)
                    {
                        
                        DataRow dr;
                        dr = ds1.Tables[0].NewRow();
                        if (entry.GBaseAttributes.Price != null)
                        {
                            dr["Price"] = entry.GBaseAttributes.Price.Value.ToString();

                            if (feed.Entries[i].Title.Text != null)
                            {
                                dr["name"] = feed.Entries[i].Title.Text.ToString();
                            }                            
                            ds1.Tables[0].Rows.Add(dr);
                        }
                        i++;
                    }
                    GridView1.DataSource = ds1;
                    GridView1.DataBind();
                    GridView1.Visible = true;
                    Label1.Visible = false;
                }
                else
                {
                    Label1.Text = "Item Not Available";
                    Label1.Visible = true;
                    Panel2.Visible = false;
                }
            }
            catch
            {
                Label1.Visible = true;
                Label1.Text = "Item Not Available Try Again Later";
                Panel2.Visible = false;
            }          
        }
        private void ebaysearch(string keyword)
        {
            using (ServiceReference2.FindingServicePortTypeClient client = new ServiceReference2.FindingServicePortTypeClient())
            {
                MessageHeader header = MessageHeader.CreateHeader("My-CustomHeader", "http://www.mycustomheader.com", "Custom Header");
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    OperationContext.Current.OutgoingMessageHeaders.Add(header);
                    HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers.Add("X-EBAY-SOA-SECURITY-APPNAME", "enest5985-d029-4f10-94a2-e2f29a83cef");
                    httpRequestProperty.Headers.Add("X-EBAY-SOA-OPERATION-NAME", "findItemsByKeywords");
                    httpRequestProperty.Headers.Add("X-EBAY-SOA-GLOBAL-ID", "EBAY-GB");



                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;


                    FindItemsByKeywordsRequest request = new FindItemsByKeywordsRequest();

                    request.keywords = keyword;
                    Label12.Text = "http://svcs.ebay.com/services/search/FindingService/v1?OPERATION-NAME=findItemsByKeywords&SERVICE-VERSION=1.0.0&SECURITY-APPNAME=enest5985-d029-4f10-94a2-e2f29a83cef&GLOBAL-ID=EBAY-GB&keywords=" + keyword + "&paginationInput.entriesPerPage=50";

                    //ItemFilter[] objitem = new ItemFilter[1];

                    //ItemFilter obj = new ItemFilter();
                    //obj.value[0] = "Brand New";

                    //obj[0].name = ItemFilterType.Condition;
                    //obj[0].value[0] = "Brand New";
                    //obj.name = ItemFilterType.Condition;
                    // obj.value[0]="Brand New".ToString();


                    // request.itemFilter[0] = obj;


                    //WebRole1.ServiceReference2.Condition cd = new ServiceReference2.Condition();
                    //cd.conditionId = 1000;
                    //cd.conditionIdSpecified=true;
                    //cd.conditionDisplayName = "Brand New";

                    //ItemFilterType a=ItemFilterType.Condition;

                    //ItemFilter objFilter = new ItemFilter();



                    //request.itemFilter = obj;



                    //request.itemFilter[0].name = ItemFilterType.Condition;


                    ////request.itemFilter[0].name =Condition;

                    //request.itemFilter[0].value[0] = "Brand New";

                    //request.itemFilter

                    //&itemFilter(0).value(0)=New
                    //&itemFilter(0).value(1)=2000
                    //&itemFilter(0).value(2)=2500

                    DataSet ds2 = null;
                    try
                    {
                        FindItemsByKeywordsResponse response = client.findItemsByKeywords(request);// . findItemsByKeywords(request);
                        ds2 = new DataSet();
                        ds2.Tables.Add();
                        ds2.Tables[0].Columns.Add("name");
                        ds2.Tables[0].Columns.Add("Price");                        
                        foreach (var item in response.searchResult.item)
                        {
                            DataRow dr;
                            dr = ds2.Tables[0].NewRow();
                            if (item.title != null)
                            {
                                dr["name"] = item.title;
                                //Label11.Text = item.title;
                            }
                            if (item.sellingStatus.currentPrice != null)
                            {
                                // dr["price"]=response.searchResult.item[0].sellingStatus.currentPrice.Value;
                                dr["price"] = Convert.ToString(item.sellingStatus.currentPrice.Value);
                                // dr["unit"] = response.searchResult.item[0].sellingStatus.currentPrice.currencyId;                               
                            }
                            ds2.Tables[0].Rows.Add(dr);

                        }
                        Panel2.Visible = true;
                        GridView1.DataSource = ds2;
                        GridView1.DataBind();
                    }
                    catch
                    {
                        Label1.Visible = true;
                        Label1.Text = "ITEM NOT AVAILABLE";
                        Panel2.Visible = false;
                    }
                }
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Label5.Text))
            {
                page = page + 1;
                amazonsrch(TextBox2.Text);
            }
        }
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Label5.Text))
            {
                page = page - 1;
                amazonsrch(TextBox2.Text);
            }
        }
        //private void amazon(string upccod)
        //{
        //    string accessKeyId = "AKIAJ7BF65PQMKI4RDQQ";
        //    string secretKey = "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";
        //    System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding(System.ServiceModel.BasicHttpSecurityMode.Transport);
        //    AWSECommerceServicePortTypeClient client = new AWSECommerceServicePortTypeClient();
        //    client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(accessKeyId, secretKey));
        //    String[] responseGroup = {
        //    "Small",
        //    "Images",
        //    "Reviews",
        //    "Tracks",
        //    "Medium",
        //    "EditorialReview",
        //    "BrowseNodes",
        //    "ItemAttributes",
        //    "OfferFull",
        //    "Offers"
        //        };
        //    // string ItemID = "610583163926";
        //    ItemLookupRequest itemLookupRequest = new ItemLookupRequest();
        //    itemLookupRequest.ItemId = new string[] { upccod };
        //    itemLookupRequest.SearchIndex = "All";
        //    itemLookupRequest.IdTypeSpecified = true;
        //    itemLookupRequest.IdType = ItemLookupRequestIdType.UPC;
        //    itemLookupRequest.MerchantId = "All";
        //    //**
        //    itemLookupRequest.Condition = ServiceReference1.Condition.All;
        //    itemLookupRequest.ResponseGroup = responseGroup;
        //    ItemLookup itemLookup = new ItemLookup();
        //    itemLookup.AWSAccessKeyId = accessKeyId;
        //    itemLookup.Request = new ItemLookupRequest[] { itemLookupRequest };
        //    ItemLookupResponse itemLookupResponse = new ItemLookupResponse();
        //    try
        //    {
        //        itemLookupResponse = client.ItemLookup(itemLookup);
        //        DataSet ds = new DataSet();
        //        ds.Tables.Add();
        //        ds.Tables[0].Columns.Add("Title");
        //        ds.Tables[0].Columns.Add("StockPhotoURL");
        //        //ds.Tables[0].Columns.Add("Version");
        //        ds.Tables[0].Columns.Add("Price");
        //        DataRow dr;
        //        dr = ds.Tables[0].NewRow();
        //        if (itemLookupResponse.Items[0].Item != null)
        //        {
        //            if (itemLookupResponse.Items[0].Item[0].ItemAttributes.Title != null)
        //            {
        //                dr["Title"] = itemLookupResponse.Items[0].Item[0].ItemAttributes.Title;
        //            }
        //            if (itemLookupResponse.Items[0].Item[0].MediumImage.URL != null)
        //            {
        //                dr["StockPhotoURL"] = itemLookupResponse.Items[0].Item[0].MediumImage.URL;
        //            }
        //            //dr["Version"] = myXMLDocument.GetElementsByTagName("Version").Item(0).InnerText;
        //            if (itemLookupResponse.Items[0].Item[0].OfferSummary.LowestUsedPrice != null)
        //            {
        //                dr["Price"] = itemLookupResponse.Items[0].Item[0].OfferSummary.LowestUsedPrice.FormattedPrice;
        //            }
        //            if (itemLookupResponse.Items[0].Item[0].LargeImage != null)
        //            {
        //                Image1.Visible = true;
        //                Image1.ImageUrl = itemLookupResponse.Items[0].Item[0].LargeImage.URL;
        //            }
        //            if (itemLookupResponse.Items[0].Item[0].SmallImage != null)
        //            {
        //                Image2.Visible = true;
        //                Image2.ImageUrl = itemLookupResponse.Items[0].Item[0].SmallImage.URL;
        //            }
        //            // string gg = itemLookupResponse.Items[0].Item[0].ItemAttributes.Feature[0];
        //            ds.Tables[0].Rows.Add(dr);
        //            grdamazon.Visible = true;
        //            grdamazon.DataSource = ds;
        //            grdamazon.DataBind();
        //            Label10.Visible = false;
        //            //ebaynew(itemLookupResponse.Items[0].Item[0].ItemAttributes.Title);
        //            GridView4.Visible = false;
        //        }
        //    }
        //    catch
        //    {
        //        Label10.Text = "Item Not Avalable ...Please Try Again Later";
        //        grdamazon.Visible = false;
        //        Image1.Visible = false;
        //        Image2.Visible = false;

        //    }
        //}
        //private void ebay(string upc)
        //{
        //    String jsonResponse = String.Empty;
        //    HttpWebRequest myHttpWebRequest = null;     //Declare an HTTP-specific implementation of the WebRequest class.
        //    HttpWebResponse myHttpWebResponse = null;   //Declare an HTTP-specific implementation of the WebResponse class
        //    XmlDocument myXMLDocument = null;           //Declare XMLResponse document
        //    XmlTextReader myXMLReader = null;
        //    string url = "http://open.api.ebay.com/shipping?version=517&appid=enest5985-d029-4f10-94a2-e2f29a83cef&callname=FindProducts&ProductID.Type=UPC&ProductID.Value=" + upc + "&IncludeSelector=SellerInfo";
        //    // ttp://open.api.ebay.com/shopping?appid=enest5985-d029-4f10-94a2-e2f29a83cef&version=517&siteid=3&callname=FindItems&QueryKeywords=ipod&responseencoding=XML.
        //    //ttp://open.api.ebay.com/shipping?version=517&appid=enest5985-d029-4f10-94a2-e2f29a83cef&callname=FindProducts&ProductID.Type=UPC&ProductID.Value=899794002808&IncludeSelector=SellerInfo
        //    //siteid=3
        //    //525  DomainHistogram, Details, Items, ItemSpecifics or SellerInfo.
        //    //X-EBAY-API-SITE-ID
        //    //httpRequestProperty.Headers.Add("X-EBAY-API-SITE-ID", "3");
        //    //Create Request 014633155488
        //    myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
        //    myHttpWebRequest.Method = "GET";
        //    myHttpWebRequest.ContentType = "text/xml; encoding='utf-8'";
        //    //myHttpWebRequest.Headers.Add("X-EBAY-SOA-GLOBAL-ID", "EBAY-GB");
        //    // myHttpWebRequest.Headers.Add("X-EBAY-API-SITE-ID", "3");  
        //    //Get Response
        //    myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
        //    myXMLDocument = new XmlDocument();
        //    myXMLReader = new XmlTextReader(myHttpWebResponse.GetResponseStream());
        //    XmlReader red = myXMLReader;
        //    myXMLDocument.Load(myXMLReader);
        //    List<EbayClass> lstebay = new List<EbayClass>();
        //    EbayClass objebay = new EbayClass();
        //    try
        //    {
        //        objebay.Title = myXMLDocument.GetElementsByTagName("Title").Item(0).InnerText;
        //        objebay.Image = myXMLDocument.GetElementsByTagName("StockPhotoURL").Item(0).InnerText;
        //        objebay.Version = myXMLDocument.GetElementsByTagName("Version").Item(0).InnerText;
        //        objebay.Price = myXMLDocument.GetElementsByTagName("ConvertedCurrentPrice").Item(0).InnerText;
        //        Label9.Text = "";
        //        lstebay.Add(objebay);
        //        DataSet ds = new DataSet();
        //        ds.Tables.Add();
        //        ds.Tables[0].Columns.Add("Title");
        //        ds.Tables[0].Columns.Add("StockPhotoURL");
        //        ds.Tables[0].Columns.Add("Version");
        //        ds.Tables[0].Columns.Add("Price");
        //        DataRow dr;
        //        dr = ds.Tables[0].NewRow();
        //        dr["Title"] = myXMLDocument.GetElementsByTagName("Title").Item(0).InnerText;
        //        dr["StockPhotoURL"] = myXMLDocument.GetElementsByTagName("StockPhotoURL").Item(0).InnerText;
        //        dr["Version"] = myXMLDocument.GetElementsByTagName("Version").Item(0).InnerText;
        //        dr["Price"] = myXMLDocument.GetElementsByTagName("ConvertedCurrentPrice").Item(0).InnerText;
        //        ds.Tables[0].Rows.Add(dr);
        //        GridView3.DataSource = ds;
        //        GridView3.DataBind();
        //        GridView3.Visible = true;
        //        GridView2.Visible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        GridView3.Visible = false;
        //        ebaynew(d);
        //        // Label9.Text = "Item Not Alailable in eBay";
        //    }
        //}

        private void google(string upccode)
        {
            GBaseService service = new GBaseService("Google-Tutorial-1.0", "1/VTK_D3r3i5IzrmZFLXKajjyafXGrA38opNl93fZh3oY");
            GBaseQuery query = new GBaseQuery("http://www.google.co.uk/base/feeds/snippets?q=" + upccode + "&orderby=price%28float%20GBP%29&sortorder=ascending");
            //GBaseUriFactory.Default.SnippetsFeedUri


            
            //query.ExtraParameters = "Target Country=GB";
           // query.GoogleBaseQuery = upccode;
            try
            {
                ds.Tables.Add("Table0");
                ds.Tables["Table0"].Columns.Add("Title");
                ds.Tables["Table0"].Columns.Add("StockPhotoURL");
                //ds.Tables[0].Columns.Add("Version");
                ds.Tables["Table0"].Columns.Add("Price");
                ds.Tables["Table0"].Columns.Add("Merchant");
                ds.Tables["Table0"].Columns.Add("unit");

                GBaseFeed feed = service.Query(query);
                List<string> aa = new List<string>();
                

                if (feed.Entries.Count > 0)
                {
                    Int32 i = 0;
                    foreach (GBaseEntry entry in feed.Entries)
                    {
                        i++;
                        DataRow dr;
                        dr = ds.Tables["Table0"].NewRow();
                        if (entry.GBaseAttributes.Price != null)
                        {
                            if (entry.GBaseAttributes.Price.Unit == "gbp")
                            {
                                dr["Price"] = entry.GBaseAttributes.Price.Value.ToString();
                                dr["unit"] = entry.GBaseAttributes.Price.Unit;
                                if (feed.Entries[i].Title.Text != null)
                                {
                                    dr["Title"] = feed.Entries[0].Title.Text.ToString();
                                }
                                if (feed.Entries[i].Authors != null)
                                {
                                    dr["Merchant"] = feed.Entries[i].Authors[0].Name;
                                }

                                if (entry.GBaseAttributes.ImageLinks.Length > 0)
                                {
                                    dr["StockPhotoURL"] = entry.GBaseAttributes.ImageLinks[0].ToString();
                                }
                                //dr["Version"] = myXMLDocument.GetElementsByTagName("Version").Item(0).InnerText;
                                ds.Tables["Table0"].Rows.Add(dr);
                            }

                        }
                    }
                    //grdgoogle.DataSource = ds.Tables["Table0"];
                    //grdgoogle.DataBind();
                    //grdgoogle.Visible = true;
                    //lblnotavalable.Visible = false;

                }
                else
                {
                    //lblnotavalable.Text = "Item Not Available";
                    //lblnotavalable.Visible = true;
                    //grdgoogle.Visible = false;
                }
            }
            catch
            {
                //lblnotavalable.Visible = true;
                //lblnotavalable.Text = "Item Not Available";
                //grdgoogle.Visible = false;

            }
            // GridView2.DataSource = aa;
            // GridView2.DataBind();
        }
        private void ebaynew(string title)
        {
            using (ServiceReference2.FindingServicePortTypeClient client = new ServiceReference2.FindingServicePortTypeClient())
            {
                MessageHeader header = MessageHeader.CreateHeader("My-CustomHeader", "http://www.mycustomheader.com", "Custom Header");
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    OperationContext.Current.OutgoingMessageHeaders.Add(header);
                    HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers.Add("X-EBAY-SOA-SECURITY-APPNAME", "enest5985-d029-4f10-94a2-e2f29a83cef");
                    httpRequestProperty.Headers.Add("X-EBAY-SOA-OPERATION-NAME", "findItemsByKeywords");
                    httpRequestProperty.Headers.Add("X-EBAY-SOA-GLOBAL-ID", "EBAY-GB");
                    // httpRequestProperty.Headers.Add("CONDITION=", "NEW");
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
                    FindItemsByKeywordsRequest request = new FindItemsByKeywordsRequest();
                    request.keywords = title;

                    try
                    {
                        FindItemsByKeywordsResponse response = client.findItemsByKeywords(request);// . findItemsByKeywords(request);                       
                        //ds = new DataSet();
                        ds.Tables.Add("Table1");
                        ds.Tables["Table1"].Columns.Add("Title");
                        ds.Tables["Table1"].Columns.Add("image");
                        //ds.Tables[0].Columns.Add("Version");
                        ds.Tables["Table1"].Columns.Add("Price");
                        ds.Tables["Table1"].Columns.Add("unit");

                        foreach (var item in response.searchResult.item)
                        {
                            DataRow dr;
                            dr = ds.Tables["Table1"].NewRow();
                            if (item.title != null)
                            {
                                dr["Title"] = item.title;
                                //Label11.Text = item.title;
                            }
                            if (item.sellingStatus.currentPrice != null)
                            {
                                // dr["price"]=response.searchResult.item[0].sellingStatus.currentPrice.Value;
                                dr["price"] = Convert.ToString(item.sellingStatus.currentPrice.Value);
                                // dr["unit"] = response.searchResult.item[0].sellingStatus.currentPrice.currencyId;
                                dr["unit"] = item.sellingStatus.currentPrice.currencyId;
                            }
                            if (item.galleryPlusPictureURL != null)
                            {
                                dr["image"] = item.galleryPlusPictureURL[0];
                               // Label9.Visible = false;
                            }
                            else
                            {
                                dr["image"] = item.galleryURL;
                            }
                            ds.Tables["Table1"].Rows.Add(dr);
                            break;
                        }
                    }
                    catch
                    {
                        //Label9.Visible = true;
                        //Label9.Text = "ITEM NOT AVAILABLE";
                        //GridView2.Visible = false;
                        //GridView3.Visible = false;
                    }
                    //GridView3.Visible = false;
                    //GridView2.Visible = true;
                    //GridView2.DataSource = ds.Tables["Table1"];
                    //GridView2.DataBind();

                }
            }
        }

        private void amazonsrchbyekeyword(string title)
        {

            string accessKeyId = "AKIAI7X5QEXQNWUELB7A";
            //"AKIAJ7BF65PQMKI4RDQQ";
            string secretKey = "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";
            //"KE72/nPGj1xQhNq7vQvPgg3vLx90UMy5nqoSjRnY";
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.MaxReceivedMessageSize = int.MaxValue;
            AWSECommerceServicePortTypeClient client = new AWSECommerceServicePortTypeClient();
            client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(accessKeyId, secretKey));
            ItemSearchRequest request = new ItemSearchRequest();
            request.SearchIndex = DropDownList1.SelectedItem.Text;
            request.ItemPage = Convert.ToString(page);
            request.Keywords = title;
            request.ResponseGroup = new string[] 
        {
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
         //"Small", "Images", "Medium", "EditorialReview", "BrowseNodes",
        };
            request.RelationshipType = new string[] 
        { 
          //  "Tracks","AuthorityTitle", "DigitalMusicArranger",
          //"DigitalMusicComposer", "DigitalMusicConductor", "DigitalMusicEnsemble", 
          //"DigitalMusicLyricist", "DigitalMusicPerformer", 
          //"DigitalMusicPrimaryArtist", "DigitalMusicProducer", 
          // "DigitalMusicSongWriter", "Episode", "Season" 
        };
            ItemSearch itemSearch = new ItemSearch();
            itemSearch.Request = new ItemSearchRequest[] { request };
            itemSearch.AWSAccessKeyId = accessKeyId;
            request.ItemPage = Convert.ToString(page);
            try
            {
                // ds = new DataSet();
                ds.Tables.Add("Table2");
                ItemSearchResponse response = client.ItemSearch(itemSearch);
                if (response.Items[0].Item != null)
                {
                    ds.Tables["Table2"].Columns.Add("title");
                    ds.Tables["Table2"].Columns.Add("UPC");
                    ds.Tables["Table2"].Columns.Add("Price");
                    ds.Tables["Table2"].Columns.Add("image");
                    ds.Tables["Table2"].Columns.Add("small");
                    ds.Tables["Table2"].Columns.Add("large");                    
                    DataRow r;                    
                    r = ds.Tables["Table2"].NewRow();
                    if (response.Items[0].Item[0].ItemAttributes.UPC != null)
                    {
                        r[1] = response.Items[0].Item[0].ItemAttributes.UPC;
                    }
                    if (response.Items[0].Item[0].OfferSummary.LowestUsedPrice != null)
                    {
                        r[2] = response.Items[0].Item[0].OfferSummary.LowestUsedPrice.FormattedPrice;
                    }
                    else 
                    {
                        if (response.Items[0].Item[0].OfferSummary.LowestNewPrice != null)
                        {
                            r[2] = response.Items[0].Item[0].OfferSummary.LowestNewPrice.FormattedPrice;
                        }
                        else
                        {
                            r[2] = "Price not available...!";
                        }

                        
                    }
                    if (response.Items[0].Item[0].ItemAttributes.Title != null)
                    {
                        r[0] = response.Items[0].Item[0].ItemAttributes.Title;
                    }
                    if (response.Items[0].Item[0].MediumImage != null)
                    {
                        r[3] = response.Items[0].Item[0].MediumImage.URL;                        
                    }
                    else
                    {
                        r[3] = "Not Available";
                    }
                    if (response.Items[0].Item[0].LargeImage != null)
                    {
                        //Image1.Visible = true;
                        //Image1.ImageUrl = response.Items[0].Item[0].LargeImage.URL;
                        r[5] = response.Items[0].Item[0].LargeImage.URL;
                    }
                    else
                    {
                        r[5]= "Not Available";                    
                    }
                    if (response.Items[0].Item[0].SmallImage != null)
                    {
                        //Image2.Visible = true;
                        //Image2.ImageUrl = response.Items[0].Item[0].SmallImage.URL;
                        r[4]=response.Items[0].Item[0].SmallImage.URL;
                    }
                    else
                    {
                       r[4] = "Not Available";
                    }
                    ds.Tables["Table2"].Rows.Add(r);
                                   
                   // GridView4.Visible = true;
                   //// grdamazon.Visible = false;
                   // GridView4.DataSource = ds.Tables["Table2"];
                   // GridView4.DataBind();
                   
                   
                   

                }
                else
                {
                    //Label1.Text = "Item Not Available";
                    //GridView4.Visible = false;
                    //Image1.Visible = false;
                    //Image2.Visible = false;
                }
            }
            catch
            {
                //Label1.Text = "Please Try Again Later";

            }
        }

        protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            DropDownList1.Visible = false;
            Label11.Visible = false;
        }
        protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            DropDownList1.Visible = true;
            Label11.Visible = true;

        }
        protected void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            DropDownList1.Visible = false;
            Label11.Visible = false;
        }
        private void savekeyword(string search)
        {
            try
            {
                DataClasses1DataContext obj = new DataClasses1DataContext();
                next = obj.insertsearch(search);
            }
            catch
            {
                Response.Write("Error in Keyword Save Opetation");
            
            }
        }
        private void saveimage(string small, string medium, string larg,Int32 keyword)
        {
            try
            {
                DataClasses1DataContext obj = new DataClasses1DataContext();
                nextimage = obj.saveimage(small, medium, larg, keyword);
            }
            catch
            {
                Response.Write("Error in image save operation");            
            }
           
        }
        private void saveresult()
        {             
            try
            {
                if (ds.Tables["Table2"].Rows.Count > 0 && ds.Tables["Table1"].Rows.Count > 0 && ds.Tables["Table0"].Rows.Count > 0)
                {
                    DataClasses1DataContext obj = new DataClasses1DataContext();
                    for (int i = 0; i < ds.Tables["Table2"].Rows.Count; i++)
                    {
                        saveimage(ds.Tables["Table2"].Rows[0].ItemArray[4].ToString(), ds.Tables["Table2"].Rows[0].ItemArray[3].ToString(), ds.Tables["Table2"].Rows[0].ItemArray[5].ToString(), next);
                       
                        Image1.ImageUrl = ds.Tables["Table2"].Rows[0].ItemArray[3].ToString();
                        Image2.ImageUrl = ds.Tables["Table2"].Rows[0].ItemArray[4].ToString();
                        Image3.ImageUrl = ds.Tables["Table2"].Rows[0].ItemArray[5].ToString();
                        
                        tbamazon tb = new tbamazon();
                        tb.keywordid = next;
                        if (!string.IsNullOrEmpty(ds.Tables["Table2"].Rows[0].ItemArray[0].ToString()))
                        {
                            tb.title = ds.Tables["Table2"].Rows[0].ItemArray[0].ToString();
                        }
                        else
                        {
                            tb.title = "Not Available";
                        }
                        tb.imageid = nextimage;
                        if (!string.IsNullOrEmpty(ds.Tables["Table2"].Rows[0].ItemArray[2].ToString()))
                        {
                            tb.price = ds.Tables["Table2"].Rows[0].ItemArray[2].ToString();
                        }
                        else
                        {
                            tb.price = "Not Available";
                        }
                        obj.tbamazons.InsertOnSubmit(tb);
                        obj.SubmitChanges();
                    }
                    for (int j = 0; j < ds.Tables["Table0"].Rows.Count; j++)
                    {
                        tbgoogle tb = new tbgoogle();
                        tb.keywordid = next;
                        if (!string.IsNullOrEmpty(ds.Tables["Table0"].Rows[j].ItemArray[0].ToString()))
                        {
                            tb.title = ds.Tables["Table0"].Rows[j].ItemArray[0].ToString();
                        }
                        else
                        {
                            tb.title = "Not Availabel";
                        }
                        if (!string.IsNullOrEmpty(ds.Tables["Table0"].Rows[j].ItemArray[3].ToString()))
                        {
                            tb.marchantname = ds.Tables["Table0"].Rows[j].ItemArray[3].ToString();
                        }
                        else
                        {
                            tb.marchantname = "Not Available";
                        }
                        if (!string.IsNullOrEmpty(ds.Tables["Table0"].Rows[j].ItemArray[2].ToString()))
                        {
                            tb.price = Convert.ToDouble(ds.Tables["Table0"].Rows[j].ItemArray[2]);
                        }
                        else
                        {
                            tb.price = 0;
                        }
                        if (!string.IsNullOrEmpty(ds.Tables["Table0"].Rows[j].ItemArray[4].ToString()))
                        {
                            tb.priceunit = ds.Tables["Table0"].Rows[j].ItemArray[4].ToString();
                        }
                        else
                        {
                            tb.priceunit = "Not Available";
                        }
                        tb.imageid = nextimage;
                        obj.tbgoogles.InsertOnSubmit(tb);
                        obj.SubmitChanges();

                    }
                    for (int k = 0; k < ds.Tables["Table1"].Rows.Count; k++)
                    {
                        tbebay tb = new tbebay();
                        tb.kewordid = next;
                        if (!string.IsNullOrEmpty(ds.Tables["Table1"].Rows[0].ItemArray[0].ToString()))
                        {
                            tb.title = ds.Tables["Table1"].Rows[0].ItemArray[0].ToString();
                        }
                        else
                        {
                            tb.title = "Not Available";
                        }
                        if (!string.IsNullOrEmpty(ds.Tables["Table1"].Rows[0].ItemArray[2].ToString()))
                        {
                            tb.price = Convert.ToDouble(ds.Tables["Table1"].Rows[0].ItemArray[2]);
                        }
                        else
                        {
                            tb.price = 0;
                        }

                        if (!string.IsNullOrEmpty(ds.Tables["Table1"].Rows[0].ItemArray[3].ToString()))
                        {
                            tb.priceUnit = ds.Tables["Table1"].Rows[0].ItemArray[3].ToString();
                        }
                        else
                        {
                            tb.priceUnit = "Not Available";
                        }
                        Panel1.Visible = true;
                        tb.imageid = nextimage;
                        obj.tbebays.InsertOnSubmit(tb);
                        obj.SubmitChanges();
                        
                        GridView4.DataSource = ds.Tables["Table2"];
                        GridView4.DataBind();
                        
                        grdgoogle.DataSource = ds.Tables["Table0"];
                        grdgoogle.DataBind();
                       
                        GridView2.DataSource = ds.Tables["Table1"];
                        GridView2.DataBind();
                        Label14.Visible = false;                   
                    }
                }
                else
                {
                    Label14.Visible = true;
                    Label14.Text = "Sorry product is not available in all location so we do not have any offer for this.Please select other one ...!";
                    Panel1.Visible = false;
                    
                }
            }
            catch(Exception ex)
            {
                Response.Write("Error in Database Operation");            
            }
                next = 0;
                nextimage = 0;
            }

        protected void grdgoogle_Sorting1(object sender, GridViewSortEventArgs e)
        {
            //DataTable dtSortTable =(DataTable) ViewState["vv"] ;
            //    //grdgoogle.DataSource as DataTable;

            //if (dtSortTable != null)
            //{
            //    DataView dvSortedView = new DataView(dtSortTable);
            //    dvSortedView.Sort = e.SortExpression + " " + getSortDirectionString(e.SortDirection);

            //    grdgoogle.DataSource = dvSortedView;
            //    grdgoogle.DataBind();
            //}
        }

        //private string getSortDirectionString(SortDirection sortDireciton)
        //    {
        //       string newSortDirection = String.Empty;
        //    if(sortDireciton== SortDirection.Ascending)
        //    {
        //           newSortDirection = "ASC";
        //    }
        //    else
        //    {
        //           newSortDirection = "DESC";
        //    }

        //    return newSortDirection;

        //}
        }
    }