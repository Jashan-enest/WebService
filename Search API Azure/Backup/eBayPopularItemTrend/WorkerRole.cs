using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using System.Xml;

namespace eBayPopularItemTrend
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            XmlDocument doc = null;
            eBayPopularItemTrend objPopularItemTrend = null;
            eBayTrendMasterSetting objMasterSetting = null;
            List<eBayCategories> objLsteBayCategory = null;
            XmlNamespaceManager ns1 = null;
            String eBaySubCategoriesResponse = String.Empty;
            int settingId;
            int count;
            try
            {
                using (eBayPopularItemTrendDBDataContext eBayConn = new eBayPopularItemTrendDBDataContext())
                {
                    var eBaySiteId = (from site in eBayConn.eBayCountrySiteIds
                                      where site.Active.Equals('1')
                                      select site.eBaySiteId).ToList();

                    foreach (var ids in eBaySiteId)
                    {

                        //********************************************************************************//
                        //****** Schedule for the eBay_Category,eBay_SubCategory,eBay_Popular_Items ******//
                        //********************************************************************************//

                        #region Schedule for the eBay_Category,eBay_SubCategory,eBay_Popular_Items

                        // Schedular date for Popular Items
                        String exectionPlan = String.Empty;
                        eBayConn.eBayPopularItemCallSchedular(ids, ref exectionPlan);

                        // Schedular date for eBay Category
                        String executionPlanCategory = String.Empty;
                        eBayConn.eBayCategorySchedule(ids, ref executionPlanCategory);

                        // Schedular date for eBay Category
                        String executionPlanSubCategory = String.Empty;
                        eBayConn.eBaySubCategorySchedule(ids, ref executionPlanSubCategory);

                        #endregion

                        //*****************************************************************************//
                        //****** Insert the Value in 'eBayTrendMasterSetting' table for Category ******//
                        //*****************************************************************************//

                        #region eBayTrendMasterSetting table for Category
                        if (DateTime.Compare(Convert.ToDateTime(DateTime.Now.ToShortDateString()), Convert.ToDateTime(executionPlanCategory)) == 0)
                        {
                            // Insert next Schedule Date for the Category from eBay
                            objMasterSetting = new eBayTrendMasterSetting();
                            objMasterSetting.StartDateTime = DateTime.Now;
                            objMasterSetting.Status = "Start";
                            objMasterSetting.Type = "CAT";
                            objMasterSetting.eBaySiteId = ids;
                            eBayConn.eBayTrendMasterSettings.InsertOnSubmit(objMasterSetting);
                            eBayConn.SubmitChanges();
                            settingId = objMasterSetting.Id;

                            // Request for the Category from eBay
                            eBayHttpRequest.InserteBayCategoryIntoDB(ids);

                            // Update last entry after successfull insertion or updation
                            eBayTrendMasterSetting objeBayTrendSetting = eBayConn.eBayTrendMasterSettings.Single(p => p.Id == settingId);
                            objeBayTrendSetting.NextExecutionDate = DateTime.Now.AddMonths(1);
                            objeBayTrendSetting.CompleteDateTime = DateTime.Now;
                            objeBayTrendSetting.Status = "Completed";
                            eBayConn.SubmitChanges();

                        }
                        #endregion

                        //********************************************************************************//
                        //****** Insert the Value in 'eBayPopularItemTrend' table for SubCategory ******//
                        //********************************************************************************//

                        #region eBayTrendMasterSetting table for SubCategory
                        if (DateTime.Compare(Convert.ToDateTime(DateTime.Now.ToShortDateString()), Convert.ToDateTime(executionPlanSubCategory)) == 0)
                        {
                            // Insert next Schedule Date for the Category from eBay
                            objMasterSetting = new eBayTrendMasterSetting();
                            objMasterSetting.StartDateTime = DateTime.Now;
                            objMasterSetting.Status = "Start";
                            objMasterSetting.Type = "SBCAT";
                            objMasterSetting.eBaySiteId = ids;
                            eBayConn.eBayTrendMasterSettings.InsertOnSubmit(objMasterSetting);
                            eBayConn.SubmitChanges();
                            settingId = objMasterSetting.Id;

                            // Request for the Category from eBay
                            eBayHttpRequest.InserteBaySubCategoriesIntoDB(ids);

                            // Update last entry after successfull insertion or updation
                            eBayTrendMasterSetting objeBayTrendSetting = eBayConn.eBayTrendMasterSettings.Single(p => p.Id == settingId);
                            objeBayTrendSetting.NextExecutionDate = DateTime.Now.AddMonths(1);
                            objeBayTrendSetting.CompleteDateTime = DateTime.Now;
                            objeBayTrendSetting.Status = "Completed";
                            eBayConn.SubmitChanges();
                        }
                        #endregion

                        //********************************************************************************//
                        //****** Insert the Value in 'eBayPopularItemTrend' table for Popular Items ******//
                        //********************************************************************************//

                        #region eBayTrendMasterSetting table for Popular Items

                        if (DateTime.Compare(Convert.ToDateTime(DateTime.Now.ToShortDateString()), Convert.ToDateTime(exectionPlan)) == 0)
                        {
                            objLsteBayCategory = new List<eBayCategories>();

                            var subCategory = (from subCat in eBayConn.eBaySubCategories
                                               where subCat.eBaySiteId.Equals(ids)
                                               select new
                                               {
                                                   subCat.SubCategoryId,
                                                   subCat.SubCategoryName
                                               }).ToList();

                            //******************************************************************************//
                            //******Insert the Value in 'eBayTrendMasterSetting' table for PopularItems*****//
                            //******************************************************************************//
                            objMasterSetting = new eBayTrendMasterSetting();
                            objMasterSetting.StartDateTime = DateTime.Now;
                            objMasterSetting.Status = "Start";
                            objMasterSetting.Type = "ITM";
                            objMasterSetting.eBaySiteId = ids;
                            eBayConn.eBayTrendMasterSettings.InsertOnSubmit(objMasterSetting);
                            eBayConn.SubmitChanges();
                            settingId = objMasterSetting.Id;

                            foreach (var ebay in subCategory)
                            {
                                string eBayXmlResponse = eBayHttpRequest.eBayFindMostPopularItemCall(ebay.SubCategoryId, ebay.SubCategoryName, ids);

                                doc = new XmlDocument();
                                doc.LoadXml(eBayXmlResponse.ToString());

                                ns1 = new XmlNamespaceManager(doc.NameTable);
                                ns1.AddNamespace("def2", "urn:ebay:apis:eBLBaseComponents");
                                XmlNodeList nodeList = doc.SelectNodes("def2:FindPopularItemsResponse", ns1);

                                // Insert the Most Popular Items in DB
                                foreach (XmlNode node in nodeList)
                                {
                                    // Checking the 'Success' Acknowledge from the Request 
                                    String acknowledge = node.SelectSingleNode("def2:Ack", ns1).InnerXml;
                                    if (acknowledge.Equals("Success", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        XmlNodeList xmlNodeList = node.SelectNodes("def2:ItemArray", ns1);
                                        foreach (XmlNode node1 in xmlNodeList)
                                        {
                                            XmlNodeList nodeList2 = node1.SelectNodes("def2:Item", ns1);
                                            count = 0;
                                            foreach (XmlNode node2 in nodeList2)
                                            {
                                                objPopularItemTrend = new eBayPopularItemTrend();
                                                objPopularItemTrend.ExecutionDate = DateTime.Now;
                                                objPopularItemTrend.UnderCategory = ebay.SubCategoryName;
                                                objPopularItemTrend.UnderCategoryId = ebay.SubCategoryId;
                                                objPopularItemTrend.eBaySiteId = ids;
                                                if (node2.SelectNodes("def2:ConvertedCurrentPrice", ns1).Count > 0)
                                                {
                                                    if (Convert.ToDouble(node2.SelectSingleNode("def2:ConvertedCurrentPrice", ns1).InnerXml) > 10 && count < 10)
                                                    {
                                                        //Creating a Data row for the dynamically created table.
                                                        if (node2.SelectNodes("def2:GalleryURL", ns1).Count > 0)
                                                            objPopularItemTrend.GalleryURL = node2.SelectSingleNode("def2:GalleryURL", ns1).InnerXml;
                                                        if (node2.SelectNodes("def2:PrimaryCategoryID", ns1).Count > 0)
                                                            objPopularItemTrend.PrimaryCategoryID = node2.SelectSingleNode("def2:PrimaryCategoryID", ns1).InnerXml;
                                                        if (node2.SelectNodes("def2:Title", ns1).Count > 0)
                                                            objPopularItemTrend.Title = node2.SelectSingleNode("def2:Title", ns1).InnerXml;
                                                        if (node2.SelectNodes("def2:BidCount", ns1).Count > 0)
                                                            objPopularItemTrend.BidCount = Convert.ToInt32(node2.SelectSingleNode("def2:BidCount", ns1).InnerXml);
                                                        if (node2.SelectNodes("def2:ItemID", ns1).Count > 0)
                                                            objPopularItemTrend.ItemID = node2.SelectSingleNode("def2:ItemID", ns1).InnerXml;
                                                        if (node2.SelectNodes("def2:ListingType", ns1).Count > 0)
                                                            objPopularItemTrend.ListingType = node2.SelectSingleNode("def2:ListingType", ns1).InnerXml;
                                                        if (node2.SelectNodes("def2:PrimaryCategoryName", ns1).Count > 0)
                                                            objPopularItemTrend.PrimaryCategoryName = node2.SelectSingleNode("def2:PrimaryCategoryName", ns1).InnerXml;
                                                        if (node2.SelectNodes("def2:ViewItemURLForNaturalSearch", ns1).Count > 0)
                                                            objPopularItemTrend.ViewItemURLForNaturalSearch = node2.SelectSingleNode("def2:ViewItemURLForNaturalSearch", ns1).InnerXml;
                                                        if (node2.SelectNodes("def2:ListingStatus", ns1).Count > 0)
                                                            objPopularItemTrend.ListingStatus = node2.SelectSingleNode("def2:ListingStatus", ns1).InnerXml;
                                                        if (node2.SelectNodes("def2:ConvertedCurrentPrice", ns1).Count > 0)
                                                        {
                                                            objPopularItemTrend.ConvertedCurrentPrice = Convert.ToDecimal(node2.SelectSingleNode("def2:ConvertedCurrentPrice", ns1).InnerXml);
                                                            objPopularItemTrend.currencyID = node2.SelectSingleNode("def2:ConvertedCurrentPrice", ns1).Attributes[0].Value;
                                                        }
                                                        if (node2.SelectNodes("def2:WatchCount", ns1).Count > 0)
                                                            objPopularItemTrend.WatchCount = Convert.ToInt32(node2.SelectSingleNode("def2:WatchCount", ns1).InnerXml);
                                                        if (node2.SelectNodes("def2:EndTime", ns1).Count > 0)
                                                            objPopularItemTrend.EndTime = Convert.ToDateTime(node2.SelectSingleNode("def2:EndTime", ns1).InnerXml);

                                                        //getting shipping data
                                                        XmlNodeList xmlNodeList3 = node2.SelectNodes("def2:ShippingCostSummary", ns1);
                                                        foreach (XmlNode node3 in xmlNodeList3)
                                                        {
                                                            if (node3.SelectNodes("def2:ShippingType", ns1).Count > 0)
                                                                objPopularItemTrend.ShippingType = node3.SelectSingleNode("def2:ShippingType", ns1).InnerXml;
                                                            if (node3.SelectNodes("def2:ShippingServiceCost", ns1).Count > 0)
                                                                objPopularItemTrend.ShippingServiceCost = Convert.ToDecimal(node3.SelectSingleNode("def2:ShippingServiceCost", ns1).InnerXml);
                                                            else
                                                                objPopularItemTrend.ShippingServiceCost = 0;
                                                            if (node3.SelectNodes("def2:ListedShippingServiceCost", ns1).Count > 0)
                                                                objPopularItemTrend.ListedShippingServiceCost = Convert.ToDecimal(node3.SelectSingleNode("def2:ListedShippingServiceCost", ns1).InnerXml);
                                                            else
                                                                objPopularItemTrend.ListedShippingServiceCost = 0;
                                                        }
                                                        eBayConn.eBayPopularItemTrends.InsertOnSubmit(objPopularItemTrend);
                                                        count++;
                                                    }
                                                }
                                            }
                                            eBayConn.SubmitChanges();
                                        }
                                    }
                                }
                            }

                            // After Success Update 'eBayTrendMasterSetting' table
                            eBayTrendMasterSetting objeBayTrendSetting = eBayConn.eBayTrendMasterSettings.Single(p => p.Id == settingId);
                            objeBayTrendSetting.NextExecutionDate = DateTime.Now.AddDays(1);
                            objeBayTrendSetting.CompleteDateTime = DateTime.Now;
                            objeBayTrendSetting.Status = "Completed";
                            eBayConn.SubmitChanges();
                        }

                        #endregion
                    }
                }
            }
            catch (Exception)
            { }
            finally
            {
                doc = null;
                objPopularItemTrend = null;
                objMasterSetting = null;
                objLsteBayCategory = null;
                ns1 = null;
                eBaySubCategoriesResponse = String.Empty;
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
