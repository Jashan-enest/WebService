using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Web;
using System.Net;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace WebRole1
{
    public partial class eBay_Xml_Parser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            HttpWebRequest ebayRequest = null;
            HttpWebResponse ebayResponse = null;
            string ebayResponseXML = string.Empty;
            StringBuilder strUrl = null;
            try
            {
                strUrl = new StringBuilder();

                strUrl.Append("http://open.api.ebay.com/shopping?callname=FindPopularItems&responseencoding=xml&appid=Marketin-0d9e-456e-b9ec-27a374a14023");
                strUrl.Append("&siteid=");
                strUrl.Append(ddlCountries.SelectedValue.ToString());
                strUrl.Append("&QueryKeywords=");
                if (rdoCategory.Checked)
                    strUrl.Append(ddlCategories.SelectedValue);
                else
                    strUrl.Append(txtKeyword.Text.Trim());
                strUrl.Append("&version=713");

                //Creating a request for Ebay.
                ebayRequest = (HttpWebRequest)WebRequest.Create(strUrl.ToString());
                ebayRequest.Method = "GET";

                //Getting the Ebay Response in XML format.
                ebayResponse = (HttpWebResponse)ebayRequest.GetResponse();
                using (StreamReader sr = new StreamReader(ebayResponse.GetResponseStream()))
                {
                    ebayResponseXML = sr.ReadToEnd().ToString();
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(ebayResponseXML.ToString());

                XmlNamespaceManager ns1 = new XmlNamespaceManager(doc.NameTable);
                ns1.AddNamespace("def2", "urn:ebay:apis:eBLBaseComponents");
                ns1.AddNamespace("def3", "urn:ebay:apis:eBLBaseComponents");
                XmlNodeList nodeList = doc.SelectNodes("def2:FindPopularItemsResponse", ns1);

                //Creating a Table for the data.
                DataTable tblEbaySearch = new DataTable();
                tblEbaySearch.Columns.Add("GalleryURL");
                tblEbaySearch.Columns.Add("Title");
                tblEbaySearch.Columns.Add("BidCount");
                tblEbaySearch.Columns.Add("ItemID");
                tblEbaySearch.Columns.Add("ListingType");
                tblEbaySearch.Columns.Add("PrimaryCategoryName");
                tblEbaySearch.Columns.Add("ViewItemURLForNaturalSearch");
                tblEbaySearch.Columns.Add("ListingStatus");
                tblEbaySearch.Columns.Add("ConvertedCurrentPrice");
                tblEbaySearch.Columns.Add("WatchCount");
                tblEbaySearch.Columns.Add("EndTime");
                tblEbaySearch.Columns.Add("ShippingType");
                tblEbaySearch.Columns.Add("ShippingServiceCost");
                tblEbaySearch.Columns.Add("ListedShippingServiceCost");
                tblEbaySearch.Columns.Add("CurrencyID");
                tblEbaySearch.Columns.Add("ListShippingCurrencyID");
                tblEbaySearch.Columns.Add("ConvertedCurrencyID");

                foreach (XmlNode node in nodeList)
                {
                    XmlNodeList xmlNodeList = node.SelectNodes("def2:ItemArray", ns1);
                    foreach (XmlNode node1 in xmlNodeList)
                    {
                        XmlNodeList nodeList2 = node1.SelectNodes("def2:Item", ns1);
                        foreach (XmlNode node2 in nodeList2)
                        {
                            if (node2.SelectNodes("def2:ConvertedCurrentPrice", ns1).Count > 0)
                            {
                                if (Convert.ToDouble(node2.SelectSingleNode("def2:ConvertedCurrentPrice", ns1).InnerXml) > 10)
                                {
                                    //Creating a Data row for the dynamically created table.
                                    DataRow row = tblEbaySearch.NewRow();
                                    if (node2.SelectNodes("def2:GalleryURL", ns1).Count > 0)
                                        row["GalleryURL"] = node2.SelectSingleNode("def2:GalleryURL", ns1).InnerXml;
                                    if (node2.SelectNodes("def2:Title", ns1).Count > 0)
                                        row["Title"] = node2.SelectSingleNode("def2:Title", ns1).InnerXml;
                                    if (node2.SelectNodes("def2:BidCount", ns1).Count > 0)
                                        row["BidCount"] = node2.SelectSingleNode("def2:BidCount", ns1).InnerXml;
                                    if (node2.SelectNodes("def2:ItemID", ns1).Count > 0)
                                        row["ItemID"] = node2.SelectSingleNode("def2:ItemID", ns1).InnerXml;
                                    if (node2.SelectNodes("def2:ListingType", ns1).Count > 0)
                                        row["ListingType"] = node2.SelectSingleNode("def2:ListingType", ns1).InnerXml;
                                    if (node2.SelectNodes("def2:PrimaryCategoryName", ns1).Count > 0)
                                        row["PrimaryCategoryName"] = node2.SelectSingleNode("def2:PrimaryCategoryName", ns1).InnerXml;
                                    if (node2.SelectNodes("def2:ViewItemURLForNaturalSearch", ns1).Count > 0)
                                        row["ViewItemURLForNaturalSearch"] = node2.SelectSingleNode("def2:ViewItemURLForNaturalSearch", ns1).InnerXml;
                                    if (node2.SelectNodes("def2:ListingStatus", ns1).Count > 0)
                                        row["ListingStatus"] = node2.SelectSingleNode("def2:ListingStatus", ns1).InnerXml;
                                    if (node2.SelectNodes("def2:ConvertedCurrentPrice", ns1).Count > 0)
                                    {
                                        row["ConvertedCurrentPrice"] = node2.SelectSingleNode("def2:ConvertedCurrentPrice", ns1).InnerXml;
                                        row["ConvertedCurrencyID"] = node2.SelectSingleNode("def2:ConvertedCurrentPrice", ns1).Attributes[0].Value;
                                    }
                                    if (node2.SelectNodes("def2:WatchCount", ns1).Count > 0)
                                        row["WatchCount"] = node2.SelectSingleNode("def2:WatchCount", ns1).InnerXml;
                                    if (node2.SelectNodes("def2:EndTime", ns1).Count > 0)
                                        row["EndTime"] = Convert.ToDateTime(node2.SelectSingleNode("def2:EndTime", ns1).InnerXml);

                                    //getting shipping data
                                    XmlNodeList xmlNodeList3 = node2.SelectNodes("def2:ShippingCostSummary", ns1);
                                    foreach (XmlNode node3 in xmlNodeList3)
                                    {
                                        if (node3.SelectNodes("def2:ShippingType", ns1).Count > 0)
                                            row["ShippingType"] = node3.SelectSingleNode("def2:ShippingType", ns1).InnerXml;
                                        if (node3.SelectNodes("def2:ShippingServiceCost", ns1).Count > 0)
                                        {
                                            row["ShippingServiceCost"] = node3.SelectSingleNode("def2:ShippingServiceCost", ns1).InnerXml;
                                            row["CurrencyID"] = node3.SelectSingleNode("def2:ShippingServiceCost", ns1).Attributes[0].Value;
                                        }
                                        if (node3.SelectNodes("def2:ListedShippingServiceCost", ns1).Count > 0)
                                        {
                                            row["ListedShippingServiceCost"] = node3.SelectSingleNode("def2:ListedShippingServiceCost", ns1).InnerXml;
                                            row["ListShippingCurrencyID"] = node3.SelectSingleNode("def2:ListedShippingServiceCost", ns1).Attributes[0].Value;
                                        }
                                    }
                                    tblEbaySearch.Rows.Add(row);
                                }
                            }
                        }
                        tblEbaySearch.AcceptChanges();
                    }
                }
                grdEbayData.Visible = true;
                //dt.DefaultView.Sort = "ID ASC";
                grdEbayData.DataSource = tblEbaySearch;
                grdEbayData.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = ex.Message.ToString();
            }
            finally
            {
                ebayRequest = null;
                ebayResponse = null;
                ebayResponseXML = string.Empty;
                strUrl = null;
            }
        }

        protected void ddlCountries_SelectedIndexChanged(object sender, EventArgs e)
        {
            HttpWebRequest ebayRequest = null;
            string keyword = string.Empty;
            HttpWebResponse ebayResponse = null;
            string ebayResponseXML = string.Empty;
            string url = string.Empty;
            XmlDocument doc = null;
            try
            {
                ddlCategories.Items.Clear();
                if (ddlCountries.SelectedValue.Equals("-1"))
                {
                    ddlCategories.Visible = false;
                    return;
                }
                //Creating a request for Ebay.
                url = "http://open.api.ebay.com/Shopping?callname=GetCategoryInfo&appid=Marketin-0d9e-456e-b9ec-27a374a14023&";
                url += "siteid=" + ddlCountries.SelectedValue.ToString();
                url += "&CategoryID=-1&IncludeSelector=ChildCategories&version=713";

                ebayRequest = (HttpWebRequest)WebRequest.Create(url);
                ebayRequest.Method = "GET";

                //Getting the Ebay Response in XML format.
                ebayResponse = (HttpWebResponse)ebayRequest.GetResponse();
                using (StreamReader sr = new StreamReader(ebayResponse.GetResponseStream()))
                {
                    ebayResponseXML = sr.ReadToEnd().ToString();
                }

                doc = new XmlDocument();
                doc.LoadXml(ebayResponseXML.ToString());
                DataTable tblEbayCategories = new DataTable();
                tblEbayCategories.Columns.Add("CategoryName");

                XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
                ns.AddNamespace("def", "urn:ebay:apis:eBLBaseComponents");
                XmlNodeList nodeRootList = doc.SelectNodes("def:GetCategoryInfoResponse", ns);
                foreach (XmlNode nodeRoot in nodeRootList)
                {
                    XmlNodeList nodeFirstList = nodeRoot.SelectNodes("def:CategoryArray", ns);
                    foreach (XmlNode nodeFirst in nodeFirstList)
                    {
                        XmlNodeList nodeSecondList = nodeFirst.SelectNodes("def:Category", ns);
                        foreach (XmlNode nodeSecond in nodeSecondList)
                        {
                            DataRow dr = tblEbayCategories.NewRow();
                            dr["CategoryName"] = nodeSecond.SelectSingleNode("def:CategoryName", ns).InnerText;
                            tblEbayCategories.Rows.Add(dr);
                        }
                    }
                }
                ddlCategories.DataSource = tblEbayCategories;
                ddlCategories.DataTextField = "CategoryName";
                ddlCategories.DataValueField = "CategoryName";
                ddlCategories.DataBind();
                ddlCategories.Visible = true;
            }
            catch (Exception)
            { }
            finally
            {
                ebayRequest = null;
                keyword = string.Empty;
                ebayResponse = null;
                ebayResponseXML = string.Empty;
                url = string.Empty;
                doc = null;
            }
        }
    }
}