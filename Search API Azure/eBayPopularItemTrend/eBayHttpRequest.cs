using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.IO;

namespace eBayPopularItemTrend
{

    public class eBayCategories
    {
        private Int32 _eBayCategoriesId;
        public Int32 eBayCategoriesId
        {
            get { return _eBayCategoriesId; }
            set { _eBayCategoriesId = value; }
        }

        private String _eBayCategoriesName;
        public String eBayCategoriesName
        {
            get { return _eBayCategoriesName; }
            set { _eBayCategoriesName = value; }
        }
    }

    public class eBaySubCategories
    {
        private Int32 _eBayCategoriesId;
        public Int32 eBayCategoriesId
        {
            get { return _eBayCategoriesId; }
            set { _eBayCategoriesId = value; }
        }

        private Int32 _eBaySubCategoriesId;
        public Int32 eBaySubCategoriesId
        {
            get { return _eBayCategoriesId; }
            set { _eBaySubCategoriesId = value; }
        }

        private String _eBaySubCategoriesName;
        public String eBaySubCategoriesName
        {
            get { return _eBaySubCategoriesName; }
            set { _eBaySubCategoriesName = value; }
        }
    }

    public class eBayHttpRequest
    {
        public static String eBayFindMostPopularItemCall(String categoryId, String keyword, String eBaySiteId)
        {
            HttpWebRequest ebayRequest = null;
            HttpWebResponse ebayResponse = null;
            string ebayResponseXML = string.Empty;
            StringBuilder strUrl = null;
            try
            {
                strUrl = new StringBuilder();

                strUrl.Append("http://open.api.ebay.com/shopping?callname=FindPopularItems&responseencoding=xml&appid=Marketin-0d9e-456e-b9ec-27a374a14023");
                strUrl.Append("&CategoryID=");
                strUrl.Append(categoryId);
                strUrl.Append("&siteid=");
                strUrl.Append(eBaySiteId);
                strUrl.Append("&QueryKeywords=");
                strUrl.Append(keyword);
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

                return ebayResponseXML.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                ebayRequest = null;
                ebayResponse = null;
                ebayResponseXML = string.Empty;
                strUrl = null;
            }
        }

        public static void InserteBayCategoryIntoDB(String eBaySiteId)
        {
            HttpWebRequest ebayRequest = null;
            HttpWebResponse ebayResponse = null;
            StringBuilder strUrl = null;
            string ebayResponseXML = string.Empty;
            XmlDocument doc = null;
            Int32 count = 0;
            eBayCategory objeBayCategory = null;
            try
            {
                using (eBayPopularItemTrendDBDataContext eBayConn = new eBayPopularItemTrendDBDataContext())
                {
                    List<String> eBayCategoryDBValues = (from data in eBayConn.eBayCategories
                                                         select data.CategoryId).ToList();

                    //Creating a request for Ebay
                    //Currently the request if for the Root(-1) category.
                    strUrl = new System.Text.StringBuilder();
                    strUrl.Append("http://open.api.ebay.com/Shopping?callname=GetCategoryInfo&appid=Marketin-0d9e-456e-b9ec-27a374a14023&");
                    strUrl.Append("siteid=");
                    strUrl.Append(eBaySiteId);
                    strUrl.Append("&CategoryID=-1&IncludeSelector=ChildCategories&version=713");

                    ebayRequest = (HttpWebRequest)WebRequest.Create(strUrl.ToString());
                    ebayRequest.Method = "GET";

                    //Getting the Ebay Response in XML format.
                    ebayResponse = (HttpWebResponse)ebayRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(ebayResponse.GetResponseStream()))
                    {
                        ebayResponseXML = sr.ReadToEnd().ToString();
                    }

                    doc = new XmlDocument();
                    doc.LoadXml(ebayResponseXML.ToString());

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
                                //To skip the first XML node from the eBay
                                if (count != 0 && !eBayCategoryDBValues.Contains(nodeSecond.SelectSingleNode("def:CategoryID", ns).InnerText))
                                {
                                    objeBayCategory = new eBayCategory();
                                    objeBayCategory.CategoryId = nodeSecond.SelectSingleNode("def:CategoryID", ns).InnerText;
                                    objeBayCategory.CategoryName = nodeSecond.SelectSingleNode("def:CategoryName", ns).InnerText;
                                    objeBayCategory.CreatedOn = DateTime.UtcNow;
                                    objeBayCategory.ModifiedOn = DateTime.UtcNow;
                                    objeBayCategory.eBaySiteId = eBaySiteId;
                                    eBayConn.eBayCategories.InsertOnSubmit(objeBayCategory);
                                }
                                count++;
                            }
                        }
                        eBayConn.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                ebayRequest = null;
                ebayResponse = null;
                strUrl = null;
                ebayResponseXML = string.Empty;
                doc = null;
            }
        }

        public static void InserteBaySubCategoriesIntoDB(String eBaySiteId)
        {
            HttpWebRequest ebayRequest = null;
            HttpWebResponse ebayResponse = null;
            String ebayResponseXML = string.Empty;
            StringBuilder strUrl = null;
            XmlDocument xmleBaySubCategory = null;
            Int32 count = 0;
            eBaySubCategory objeBaySubCategory = null;
            try
            {
                using (eBayPopularItemTrendDBDataContext eBayConn = new eBayPopularItemTrendDBDataContext())
                {
                    //fetch all the Categories Id from the 'eBayCategory' table
                    List<String> eBayCategoriesID = (from ids in eBayConn.eBayCategories
                                                     where ids.eBaySiteId.Equals(eBaySiteId)
                                                     select ids.CategoryId).ToList();

                    //fetch all the Categories Id from the 'eBaySubCategory' table
                    List<String> eBaySubCategoriesID = (from ids in eBayConn.eBaySubCategories
                                                        where ids.eBaySiteId.Equals(eBaySiteId)
                                                        select ids.SubCategoryId).ToList();

                    foreach (String eBayCategoryID in eBayCategoriesID)
                    {
                        //start the count again to skip the first element node of eBay response
                        count = 0;
                        //creating a request for Ebay accordingly to the selected CategoryId
                        strUrl = new System.Text.StringBuilder();
                        strUrl.Append("http://open.api.ebay.com/Shopping?callname=GetCategoryInfo&appid=Marketin-0d9e-456e-b9ec-27a374a14023&");
                        strUrl.Append("siteid=");
                        strUrl.Append(eBaySiteId);
                        strUrl.Append("&CategoryID=");
                        strUrl.Append(eBayCategoryID);
                        strUrl.Append("&IncludeSelector=ChildCategories&version=713");

                        //creating a request for Ebay.
                        ebayRequest = (HttpWebRequest)WebRequest.Create(strUrl.ToString());
                        ebayRequest.Method = "GET";

                        //getting the Ebay Response in XML format.
                        ebayResponse = (HttpWebResponse)ebayRequest.GetResponse();
                        using (StreamReader sr = new StreamReader(ebayResponse.GetResponseStream()))
                        {
                            ebayResponseXML = sr.ReadToEnd().ToString();
                        }

                        xmleBaySubCategory = new XmlDocument();
                        xmleBaySubCategory.LoadXml(ebayResponseXML.ToString());

                        XmlNamespaceManager ns = new XmlNamespaceManager(xmleBaySubCategory.NameTable);
                        ns.AddNamespace("def", "urn:ebay:apis:eBLBaseComponents");
                        XmlNodeList nodeRootList = xmleBaySubCategory.SelectNodes("def:GetCategoryInfoResponse", ns);
                        foreach (XmlNode nodeRoot in nodeRootList)
                        {
                            XmlNodeList nodeFirstList = nodeRoot.SelectNodes("def:CategoryArray", ns);
                            foreach (XmlNode nodeFirst in nodeFirstList)
                            {
                                XmlNodeList nodeSecondList = nodeFirst.SelectNodes("def:Category", ns);
                                foreach (XmlNode nodeSecond in nodeSecondList)
                                {
                                    //To skip the first XML node from the eBay
                                    if (count != 0 && !eBaySubCategoriesID.Contains(nodeSecond.SelectSingleNode("def:CategoryID", ns).InnerText))
                                    {
                                        objeBaySubCategory = new eBaySubCategory();
                                        objeBaySubCategory.CategoryId = nodeSecond.SelectSingleNode("def:CategoryParentID", ns).InnerText;
                                        objeBaySubCategory.SubCategoryId = nodeSecond.SelectSingleNode("def:CategoryID", ns).InnerText;
                                        objeBaySubCategory.SubCategoryName = nodeSecond.SelectSingleNode("def:CategoryName", ns).InnerText;
                                        objeBaySubCategory.CreatedOn = DateTime.UtcNow;
                                        objeBaySubCategory.ModifiedOn = DateTime.UtcNow;
                                        objeBaySubCategory.eBaySiteId = eBaySiteId;
                                        eBayConn.eBaySubCategories.InsertOnSubmit(objeBaySubCategory);
                                    }
                                    count++;
                                }
                            }
                            eBayConn.SubmitChanges();
                        }
                    }
                }
            }
            catch (Exception ex) { }
            finally
            {
                ebayRequest = null;
                ebayResponse = null;
                ebayResponseXML = string.Empty;
                strUrl = null;
                xmleBaySubCategory = null;
                objeBaySubCategory = null;
            }
        }
    }

}
