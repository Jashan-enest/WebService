using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Web.Script.Services;

namespace WebRole1.eBayItemService
{
    /// <summary>
    /// Summary description for eBayAjaxCallService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class eBayAjaxCallService : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public String eBayItem(String ItemId, String eBaySiteId)
        {
            StringBuilder strResult = null;
            try
            {
                using (DataClasses1DataContext objConn = new DataClasses1DataContext())
                {
                    var items = objConn.SP_eBayItemDetail(ItemId, eBaySiteId).ToList();
                    if (items.Count > 0)
                    {
                        strResult = new StringBuilder();
                        foreach (var item in items)
                        {
                            strResult.Append(item.GalleryURL);
                            strResult.Append("@");
                            strResult.Append(item.Title);
                            strResult.Append("@");
                            strResult.Append(item.ViewItemURLForNaturalSearch);
                            strResult.Append("@");
                            strResult.Append(item.Category);
                            strResult.Append("@");
                            strResult.Append(item.price);
                        }
                    }
                }
                return strResult.ToString();
            }
            catch (Exception)
            { return null; }
            finally
            {
                strResult = null;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public eBayItemDetail eBayItemCustom(String ItemId, String eBaySiteId)
        {
            eBayItemDetail objDetails = null;
            try
            {
                using (DataClasses1DataContext objConn = new DataClasses1DataContext())
                {
                    var items = objConn.SP_eBayItemDetail(ItemId, eBaySiteId).ToList();
                    if (items.Count > 0)
                    {
                        objDetails = new eBayItemDetail();
                        foreach (var item in items)
                        {
                            objDetails.ImageUrl = item.GalleryURL;
                            objDetails.Title = item.Title;
                            objDetails.VieweBayUrl = item.ViewItemURLForNaturalSearch;
                        }
                    }
                }
                return objDetails;
            }
            catch (Exception)
            { return null; }
            finally
            {
                objDetails = null;
            }
        }
    }

    public class eBayItemDetail
    {
        private string _imageUrl;
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _vieweBayUrl;
        public string VieweBayUrl
        {
            get { return _vieweBayUrl; }
            set { _vieweBayUrl = value; }
        }
    }

}
