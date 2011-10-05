using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoSoftGlobal;

namespace WebRole1
{
    public partial class WeUniteTrendItems : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FCLiteral.Text = ChartOfItem();
        }

        public String ChartOfItem()
        {
            //strXML will be used to store the entire XML document generated
            String strXML = String.Empty;
            decimal startPrice = 0;
            decimal endPrice = 0;
            List<SP_ItemInTrend_V3Result> items = null;
            try
            {
                using (DataClasses1DataContext objConn = new DataClasses1DataContext())
                {
                    //StartDate,EndDate,StartPrice,EndPrice
                    items = new List<SP_ItemInTrend_V3Result>();

                    // Conditions for the StartPrice & EndPrice
                    if (Request.QueryString["StartPrice"] == "") { if (Request.QueryString["EndPrice"] == "") { startPrice = 0; endPrice = 0; } }
                    else
                    {
                        if (Request.QueryString["EndPrice"] == "") { startPrice = Convert.ToDecimal(Request.QueryString["StartPrice"]); endPrice = 0; }
                        else { startPrice = Convert.ToDecimal(Request.QueryString["StartPrice"]); endPrice = Convert.ToDecimal(Request.QueryString["EndPrice"]); }
                    }

                    // Conditions for the StartDate & EndDate
                    if (Request.QueryString["StartDate"] == "")
                    {
                        if (Request.QueryString["EndDate"] == "")
                        {
                            //items = objConn.SP_ItemInTrend_V2(Request.QueryString["Category"].ToString(), null, null, startPrice, endPrice).ToList();
                            items = objConn.SP_ItemInTrend_V3(Request.QueryString["eBaySiteId"].ToString(), Request.QueryString["Category"].ToString(), null, null, startPrice, endPrice).ToList();
                        }
                    }
                    else
                    {
                        if (Request.QueryString["EndDate"] == "")
                            //items = objConn.SP_ItemInTrend_V2(Request.QueryString["Category"].ToString(), Convert.ToDateTime(Request.QueryString["StartDate"].ToString()), null, startPrice, endPrice).ToList();
                            items = objConn.SP_ItemInTrend_V3(Request.QueryString["eBaySiteId"].ToString(), Request.QueryString["Category"].ToString(), Convert.ToDateTime(Request.QueryString["StartDate"].ToString()), null, startPrice, endPrice).ToList();
                        else
                            items = objConn.SP_ItemInTrend_V3(Request.QueryString["eBaySiteId"].ToString(), Request.QueryString["Category"].ToString(), Convert.ToDateTime(Request.QueryString["EndDate"].ToString()), null, startPrice, endPrice).ToList();
                    }

                    //Generate the graph element
                    strXML = "<graph caption='WeUnite Items Trend' xAxisName='eBay Items' decimalPrecision='0' showNames='1' numberSuffix='' rotateNames='1' formatNumberScale='0' >";
                    if (items.Count > 0)
                    {
                        foreach (SP_ItemInTrend_V3Result item in items)
                        {
                            //Generate <set name='..' value='..' link='..' />
                            if (item.Title.Length < 50)
                                strXML = strXML + "<set name='" + item.Title.ToString().Replace("'", "") + "' value='" + item.Watch.ToString() + "' link='javascript:eBayItemWebService(" + item.Ids.ToString().Split(',').FirstOrDefault().ToString() + "," + Request.QueryString["eBaySiteId"].ToString() + ")'/>";
                            else
                                strXML = strXML + "<set name='" + item.Title.ToString().Replace("'", "").Substring(0, 49) + "' value='" + item.Watch.ToString() + "' link='javascript:eBayItemWebService(" + item.Ids.ToString().Split(',').FirstOrDefault().ToString() + "," + Request.QueryString["eBaySiteId"].ToString() + ")'/>";
                        }
                        //Finally, close <graph> element
                        strXML = strXML + "</graph>";
                    }
                    else
                    {
                        strXML = strXML + "<set name='No data found' value='0' />";
                        strXML = strXML + "</graph>";
                    }
                }

                //Create the chart - Column3D Chart with data from strXML
                return FusionCharts.RenderChart("FusionCharts/FCF_Column3D.swf", "", strXML, "WeUniteTrend", "675", "650", false, false);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                strXML = string.Empty;
            }
        }
    }
}