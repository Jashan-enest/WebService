using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;


public class ProductRepository
{
    //public static IList<Product> GetProducts(string criteria)
    //{
        //string key = criteria[0].ToString(); 
            //

        //IList<Product> list = CacheRepository.GetObjects<Product>(key);

        //if (list == null || list.Count == 0)
        //{
        //    list = DataAccess.GetProducts(criteria);
        //    CacheRepository.SaveObject(key, list);
        //}

        // return the list based on the criteria 

       // List<Product> productList = list as List<Product>;        

        //list = productList.FindAll(delegate(Product product)
        //{
        //    return product.ProductName.ToLower().StartsWith(criteria.ToLower());
        //});

        //list = productList;
        //return productList;
       
    }

