using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoSoftGlobal;

namespace WebRole1
{
    public partial class WeUniteTrendGraph : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FCLiteral.Text = CreateChart();
                using (DataClasses1DataContext dbConn = new DataClasses1DataContext())
                {
                    var countries = (from country in dbConn.eBayCountrySiteIds
                                     where country.Active.Equals("1")
                                     orderby country.eBaySiteId descending
                                     select country).ToList();
                    ddlCountry.DataSource = countries;
                    ddlCountry.DataTextField = "SiteName";
                    ddlCountry.DataValueField = "eBaySiteId";
                    ddlCountry.DataBind();
                }
            }
        }

        public String CreateChart()
        {
            //strXML will be used to store the entire XML document generated
            String strXML = String.Empty;
            System.Collections.Generic.List<SP_CategoriesInTrend_V3Result> categoriesInTrend = null;
            decimal startPrice = 0;
            decimal endPrice = 0;
            String ddlCountryValue = String.Empty;
            try
            {
                #region Comment Code for 3D PieChart
                //using (DataClasses1DataContext objConn = new DataClasses1DataContext())
                //{
                //    var categoriesInTrend = objConn.SP_CategoriesInTrend().ToList();
                //    if (categoriesInTrend.Count > 0)
                //    {
                //        //Generate the graph element
                //        strXML = "<graph caption='WeUnite Trend' subCaption='By Category' decimalPrecision='0' showNames='1' numberSuffix=' Units' pieSliceDepth='30'  formatNumberScale='0' >";
                //        foreach (SP_CategoriesInTrendResult trend in categoriesInTrend)
                //        {
                //            //Generate <set name='..' value='..' link='..' />
                //            strXML = strXML + "<set name='" + trend.Category.ToString() + "' value='" + trend.Watch.ToString() + "' link='" + Server.UrlEncode("IndivisualCategoryTrend.aspx?CategoryName=" + trend.Category.ToString()) + "'/>";
                //        }
                //        //Finally, close <graph> element
                //        strXML = strXML + "</graph>";
                //    }
                //}
                #endregion

                using (DataClasses1DataContext objConn = new DataClasses1DataContext())
                {
                    if (String.IsNullOrEmpty(ddlCountry.SelectedValue.ToString()))
                        ddlCountryValue = "3";
                    else
                        ddlCountryValue = ddlCountry.SelectedValue.ToString();
                    //var categoriesInTrend = objConn.SP_CategoriesInTrend().ToList();
                    categoriesInTrend = new List<SP_CategoriesInTrend_V3Result>();
                    // Conditions for the StartPrice & EndPrice
                    if (txtSPrice.Text == "" || txtSPrice.Text.Equals("start price")) { if (txtEPrice.Text == "" || txtEPrice.Text.Equals("end price")) { startPrice = 0; endPrice = 0; } }
                    else
                    {
                        if (txtEPrice.Text == "" || txtEPrice.Text.Equals("end price")) { startPrice = Convert.ToDecimal(txtSPrice.Text); endPrice = 0; }
                        else { startPrice = Convert.ToDecimal(txtSPrice.Text); endPrice = Convert.ToDecimal(txtEPrice.Text); }
                    }
                    // Conditions for the StartDate & EndDate
                    if (dtStart.Text == "" || dtStart.Text.Equals("start date"))
                    {
                        if (dtEnd.Text == "" || dtEnd.Text.Equals("end date"))
                            //categoriesInTrend = objConn.SP_CategoriesInTrend_V2(null, null, startPrice, endPrice).ToList();
                            categoriesInTrend = objConn.SP_CategoriesInTrend_V3(ddlCountryValue, null, null, startPrice, endPrice).ToList();
                    }
                    else
                    {
                        if (dtEnd.Text == "" || dtStart.Text.Equals("start date"))
                            categoriesInTrend = objConn.SP_CategoriesInTrend_V3(ddlCountryValue, Convert.ToDateTime(dtStart.Text), null, startPrice, endPrice).ToList();
                        else
                            categoriesInTrend = objConn.SP_CategoriesInTrend_V3(ddlCountryValue, Convert.ToDateTime(dtStart.Text), Convert.ToDateTime(dtEnd.Text), startPrice, endPrice).ToList();
                    }

                    //Generate the graph element
                    strXML = "<graph caption='WeUnite Categories Trend Output' xAxisName='eBay Categories' showValues='1' decimalPrecision='0' rotateNames='1' formatNumberScale='0' >";

                    if (categoriesInTrend.Count > 0)
                    {
                        foreach (SP_CategoriesInTrend_V3Result trend in categoriesInTrend)
                        {
                            //Generate <set name='..' value='..' link='..' />
                            //strXML = strXML + "<set name='" + trend.Category.ToString() + "' value='" + trend.Watch.ToString() + "' color='" + RandomHelper.RandomColor().ToArgb() + "' link='" + Server.UrlEncode("WeUniteTrendItems.aspx?Category=" + trend.Category.ToString().Replace('&',';')) + "'/>";
                            strXML = strXML + "<set name='" + trend.Category.ToString() + "' value='" + trend.Watch.ToString() + "' color='" + RandomHelper.RandomColor().ToArgb() + "' link='javascript:CallIframe(" + trend.CategoryId + "," + ddlCountryValue + ")'/>";
                        }
                        //Finally, close <graph> element
                        strXML = strXML + "</graph>";
                    }
                    else
                    {
                        strXML = strXML + "<set name='No Data found' value='0' color='" + RandomHelper.RandomColor().ToArgb() + "'/>";
                        strXML = strXML + "</graph>";
                    }
                }

                //Create the chart - Column3D Chart with data from strXML
                return FusionCharts.RenderChart("FusionCharts/FCF_Column3D.swf", "", strXML, "WeUniteTrend", "675", "675", false, false);
            }
            catch (Exception ex)
            { return ex.Message.ToString(); }
            finally
            { }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            FCLiteral.Text = CreateChart();
        }
    }
}