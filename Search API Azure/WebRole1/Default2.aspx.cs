using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebRole1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        DataClasses1DataContext obj = new DataClasses1DataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            string q = Request.QueryString["ab"].ToString();
            showAmazon(Convert.ToInt32(q));
            showGoogle(Convert.ToInt32(q));
            showEbay(Convert.ToInt32(q));

        }
        private void showGoogle(Int32 id)
        {
            var q = from p in obj.tbgoogles
                    where p.keywordid == id
                    select p;
            GridView1.DataSource = q;
            GridView1.DataBind();
        }
        private void showAmazon(Int32 id)
        {
            var q = from p in obj.tbamazons
                    where p.keywordid == id
                    select p;
            GridView2.DataSource = q;
            GridView2.DataBind();
        }
        private void showEbay(Int32 id)
        {
            var q = from p in obj.tbebays
                    where p.kewordid == id
                    select p;
            GridView3.DataSource = q;
            GridView3.DataBind();
        }       
    }
}