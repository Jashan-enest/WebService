using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel.Web;
using System.IO;
using System.Configuration;
using Microsoft.Http;
using System.Net;
using System.Collections.Specialized;
using Microsoft.AppFabricCAT.Samples.Azure.TransientFaultHandling;
using Microsoft.AppFabricCAT.Samples.Azure.TransientFaultHandling.SqlAzure;
using Newtonsoft.Json;



namespace WebRole1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AcsService" in code, svc and config file together.
    public class AcsService : IAcsService
    {
        /// <summary>
        /// Call the main procedure of DB and give the corresponding result in xml format
        /// </summary>
        /// <returns>stream of bytes</returns>

        public System.IO.Stream XMLHeaderRequest()
        {
            RetryPolicy retry = new RetryPolicy<RetryStrategy>(5, new TimeSpan(0, 0, 5));
            string inputXml = string.Empty;
            string result = string.Empty;
            string responseType = string.Empty;
            try
            {
                if (WebOperationContext.Current.IncomingRequest.Headers["ResponseType"] != null)
                { 
                    responseType=WebOperationContext.Current.IncomingRequest.Headers["ResponseType"].ToString();
                    if (responseType.Equals("XML", StringComparison.CurrentCultureIgnoreCase)) 
                    {
                        WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
                    }
                    else if(responseType.Equals("json", StringComparison.CurrentCultureIgnoreCase))
                    
                    {
                        WebOperationContext.Current.OutgoingResponse.ContentType = "application/json;charset=utf-8";
                    
                    }
                    else
                    {
                        WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
                    }
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
                }
                
                
           
          //  WebOperationContext.Current.OutgoingResponse.ContentType = "application/json";
           
                //if (ValidateToken() || BasicHTTPAuthentication())
                //{
                
                inputXml = WebOperationContext.Current.IncomingRequest.Headers["InputXml"].ToString();
                int index = inputXml.IndexOf("<");
                if (index == -1)
                {
                    XmlDocument xml = (XmlDocument)JsonConvert.DeserializeXmlNode(inputXml);
                    inputXml = xml.InnerXml.ToString();
                }
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WE_UNITEConnectionString"].ConnectionString))
                {
                    string sprocName = "Proc_Parse_XML_Request";
                    string xmlResult = String.Empty;
                    using (SqlCommand cmd = new SqlCommand(sprocName, con))
                    {                     
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                       
                        // -1 to match the varchar(max) in DB
                        cmd.Parameters.Add("@strInput", SqlDbType.VarChar, -1).Value = inputXml;
                        SqlParameter paramXml = new SqlParameter("@result", SqlDbType.Xml);
                        paramXml.Direction = ParameterDirection.Output;                       
                        cmd.Parameters.Add(paramXml);

                        // If connection is closed then open the connection with DataBase.
                        if (con.State == ConnectionState.Closed)
                        {
                            con.OpenWithRetry(retry);
                        }
                       
                    
                        cmd.ExecuteNonQueryWithRetry(retry);

                        // Close the connection with Database after successful execution.
                        con.Close();

                        if (String.IsNullOrEmpty(paramXml.Value.ToString()))
                            xmlResult = "<error>No data found.</error>";
                        else
                            xmlResult = paramXml.Value.ToString();

                        if(responseType.Equals("json",StringComparison.CurrentCultureIgnoreCase))
                        {
                            XmlDocument xml = new XmlDocument();
                            xml.LoadXml(xmlResult);
                            result = JsonConvert.SerializeXmlNode(xml);
                        
                        }
                        else
                        {
                        result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + xmlResult;
                        }
                        //if (index == -1)
                        //{
                        //    XmlDocument xml = new XmlDocument();
                        //    xml.LoadXml(@result);
                        //    result = JsonConvert.SerializeXmlNode(xml);
                        //}

                     
                        return new MemoryStream(Encoding.UTF8.GetBytes(result));
                        
                        
                        
                        
                    }
                }
                //}
                //else
                //{
                //    result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + ReturnUnauthorized();
                //    return new MemoryStream(Encoding.UTF8.GetBytes(result));
                //}
               
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.StackTrace.ToString());
                return new MemoryStream(Encoding.UTF8.GetBytes("<?xml version=\"1.0\" encoding=\"utf-8\"?><error>" + ex.Message.ToString() + "</error>"));
            
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.StackTrace.ToString());
                return new MemoryStream(Encoding.UTF8.GetBytes("<?xml version=\"1.0\" encoding=\"utf-8\"?><error>" + ex.Message.ToString() + "</error>"));
               
            }
        }

        /// <summary>
        /// Validate the token generated by the ACS, to access the service.
        /// </summary>
        /// <returns>true or false</returns>

        public bool ValidateToken()
        {
            string acsHostName = string.Empty;
            string serviceNamespace = string.Empty;
            string trustedAudience = string.Empty;
            string trustedTokenPolicyKey = string.Empty;
            string requiredClaimType = string.Empty;
            string requiredClaimValue = string.Empty;
            TokenValidator validator = null;
            try
            {
                // Get the Token header
                string headerValue = WebOperationContext.Current.IncomingRequest.Headers["Token"].ToString();

                // Get the Application Setting Values from the Web.Config

                acsHostName = ConfigurationManager.AppSettings["ACSHostName"].ToString();
                serviceNamespace = ConfigurationManager.AppSettings["ServiceNamespace"].ToString();
                trustedAudience = ConfigurationManager.AppSettings["trustedAudience"].ToString();
                trustedTokenPolicyKey = ConfigurationManager.AppSettings["TrustedTokenPolicyKey"].ToString();
                requiredClaimType = ConfigurationManager.AppSettings["RequiredClaimType"].ToString();
                requiredClaimValue = ConfigurationManager.AppSettings["RequiredClaimValue"].ToString();

                // check that a value is there
                if (string.IsNullOrEmpty(headerValue))
                {
                    return false;
                }

                // check that it starts with 'WRAP'
                if (!headerValue.StartsWith("WRAP "))
                {
                    return false;
                }

                string[] nameValuePair = headerValue.Substring("WRAP ".Length).Split(new char[] { '=' }, 2);

                if (nameValuePair.Length != 2 ||
                    nameValuePair[0] != "access_token" ||
                    !nameValuePair[1].StartsWith("\"") ||
                    !nameValuePair[1].EndsWith("\""))
                {
                    return false;
                }

                // trim off the leading and trailing double-quotes
                string token = nameValuePair[1].Substring(1, nameValuePair[1].Length - 2);

                // create a token validator
                validator = new TokenValidator(
                   acsHostName,
                   serviceNamespace,
                   trustedAudience,
                   trustedTokenPolicyKey);

                // validate the token
                if (!validator.Validate(token))
                {
                    return false;
                }

                //// check for an action claim
                //Dictionary<string, string> claims = validator.GetNameValues(token);

                //string actionClaimValue;
                //if (!claims.TryGetValue(requiredClaimType, out actionClaimValue))
                //{
                //    return false;
                //}

                //// check for the correct action claim value
                //if (!actionClaimValue.Equals(requiredClaimValue))
                //{
                //    return false;
                //}

                // Sending True after validating the Access Control (ACS) token.
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                acsHostName = string.Empty;
                serviceNamespace = string.Empty;
                trustedAudience = string.Empty;
                trustedTokenPolicyKey = string.Empty;
                requiredClaimType = string.Empty;
                requiredClaimValue = string.Empty;
                validator = null;
            }
        }

        /// <summary>
        /// Authenticate the request coming from the APIGEE
        /// </summary>
        /// <returns>true or false</returns>

        public bool BasicHTTPAuthentication()
        {
            string base64UserPasswordAuthentication = string.Empty;
            string encodedUserNamePassword = string.Empty;
            try
            {
                base64UserPasswordAuthentication = WebOperationContext.Current.IncomingRequest.Headers["Authorization"].ToString();
                base64UserPasswordAuthentication = base64UserPasswordAuthentication.Replace("Basic ", string.Empty);
                if (!string.IsNullOrEmpty(base64UserPasswordAuthentication))
                {
                    byte[] byteUserNamePassword = Convert.FromBase64String(base64UserPasswordAuthentication);
                    encodedUserNamePassword = System.Text.Encoding.UTF8.GetString(byteUserNamePassword);
                    string[] credentials = encodedUserNamePassword.Split(':');
                    return AutheticateUserNamePassword(credentials[0].ToString(), credentials[1].ToString());
                }
                else
                { return false; }
            }
            catch (Exception)
            { return false; }
            finally
            {
                base64UserPasswordAuthentication = string.Empty;
                encodedUserNamePassword = string.Empty;
            }
        }

        /// <summary>
        /// Validate the user name and password from the ACS.
        /// </summary>
        /// <param name="userName">User Name from basic http authentication</param>
        /// <param name="password">Password from basic http authentication</param>
        /// <returns>true or false</returns>

        public bool AutheticateUserNamePassword(string userName, string password)
        {
            string tokenResponse = string.Empty;
            string response = string.Empty;
            WebClient client = null;
            NameValueCollection values = null;
            try
            {
                // Authenticating the username and password using ACS service.
                // If Username and password will able to fetch the token from the ACS then login credentials are correct otherwise unauthorized user is trying to access our service.
                client = new WebClient();
                client.BaseAddress = string.Format("https://{0}.{1}", ConfigurationManager.AppSettings["ServiceNamespace"].ToString(), ConfigurationManager.AppSettings["AcsHostName"].ToString());

                values = new NameValueCollection();
                values.Add("wrap_name", userName);
                values.Add("wrap_password", password);
                values.Add("wrap_scope", ConfigurationManager.AppSettings["trustedAudience"].ToString());

                byte[] responseBytes = client.UploadValues("WRAPv0.9/", "POST", values);

                response = Encoding.UTF8.GetString(responseBytes);

                tokenResponse = response
                   .Split('&')
                   .Single(value => value.StartsWith("wrap_access_token=", StringComparison.OrdinalIgnoreCase))
                   .Split('=')[1];

                // If token created successfully from the request to ACS then code will return True.
                if (!string.IsNullOrEmpty(tokenResponse))
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                tokenResponse = string.Empty;
                response = string.Empty;
                client = null;
                values = null;
            }
        }

        /// <summary>
        /// Return Error Xml for unauthorized user
        /// </summary>
        /// <returns>string</returns>

        private string ReturnUnauthorized()
        {
            StringBuilder strError = null;
            try
            {
                strError = new StringBuilder();
                strError.Append("<error><errorcode>401</errorcode><message>Unauthorized Access</message></error>");
                return strError.ToString();
            }
            catch (Exception ex)
            {
                strError = new StringBuilder();
                strError.Append("<error><message>");
                strError.Append(ex.Message);
                strError.Append("</message></error>");
                return strError.ToString();
            }
            finally
            {
                strError = null;
            }
        }

    }
}
