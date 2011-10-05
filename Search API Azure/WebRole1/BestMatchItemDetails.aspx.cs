using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebRole1.BestMatchItemDetailsService;

namespace WebRole1
{
    public partial class BestMatchItemDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //getBestMatchItemDetailsRequest1 objBestMatch = new getBestMatchItemDetailsRequest1();
                //String[] itemsArray = new String[0];
                //itemsArray[0] = Request.QueryString["ItemsId"].ToString();
                //objBestMatch.getBestMatchItemDetailsRequest.itemId = itemsArray;
            }
        }


    }
}