using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Xml;
using System.Web.Script.Serialization;
using Newtonsoft;

namespace WebRole1
{
    public partial class CustomSearchAPIJSON : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebRequest webRequest = null;
            HttpWebResponse response = null;
            String jsonResponse = String.Empty;
            XmlDocument xmlResponse = null;
            XmlNamespaceManager ns = null;
            XmlNamespaceManager nsfors = null;
            String MerchantName = String.Empty, MerchantAccountID = String.Empty, Country = String.Empty, Langauge = String.Empty, Title = String.Empty, Description = String.Empty,
                ProductLink = String.Empty, Brand = String.Empty, ProductCondition = String.Empty, CurrencyId = String.Empty, ProductImage = String.Empty;
            Decimal Price;
            try
            {
                webRequest = WebRequest.Create("https://www.googleapis.com/shopping/search/v1/public/products?key=AIzaSyC4YdFPuZc36HvaAQs-eEsmp12qv_C8sd0&country=US&q=digital+camera&alt=atom&rankBy=price%3Adescending&startIndex=1&maxResults=50") as HttpWebRequest;
                response = (HttpWebResponse)webRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    jsonResponse = sr.ReadToEnd().ToString();
                }
                xmlResponse = new XmlDocument();
                xmlResponse.LoadXml(jsonResponse);
                ns = new XmlNamespaceManager(xmlResponse.NameTable);
                nsfors = new XmlNamespaceManager(xmlResponse.NameTable);
                ns.AddNamespace("ns", "http://www.w3.org/2005/Atom");
                nsfors.AddNamespace("s", "http://www.google.com/shopping/api/schemas/2010");

                XmlNodeList xmlEntryLst = xmlResponse.SelectNodes("/ns:feed/ns:entry", ns);
                foreach (XmlNode xmlEntry in xmlEntryLst)
                {
                    String title = xmlEntry.SelectSingleNode("ns:title", ns).InnerText;
                    XmlNodeList xmlnotes = xmlEntry.SelectNodes("s:product", nsfors);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                response = null;
                webRequest = null;
                jsonResponse = String.Empty;
            }
        }
    }
}