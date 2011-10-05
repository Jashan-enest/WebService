using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AppFabricCAT.Samples.Azure.TransientFaultHandling;
using Microsoft.AppFabricCAT.Samples.Azure.TransientFaultHandling.SqlAzure;
using System.Data.SqlClient;

namespace WebRole1
{
    public class RetryStrategy:ITransientErrorDetectionStrategy
    {
        public bool IsTransient(Exception ex)
        {
            if (ex != null && ex is SqlException)
            {
                foreach (SqlError error in (ex as SqlException).Errors)
                {
                    switch (error.Number)
                    {
                        case 20:
                            System.Diagnostics.Debug.WriteLine("SQL Error: The instance of SQL Server does not support encryption. Retrying...");
                            return true;
                        case 64:
                            System.Diagnostics.Debug.WriteLine("SQL Error: An error occurred during login. Retrying...");
                            return true;
                        case 233:
                            System.Diagnostics.Debug.WriteLine("SQL Error: Connection initialization error. Retrying...");
                            return true;
                        case 10053:
                            System.Diagnostics.Debug.WriteLine("SQL Error: A transport-level error occurred when receiving results from the server. Retrying...");
                            return true;
                        case 10054:
                            System.Diagnostics.Debug.WriteLine("SQL Error: A transport-level error occurred when sending the request to the server. Retrying...");
                            return true;
                        case 10060:
                            System.Diagnostics.Debug.WriteLine("SQL Error:Network or instance-specific error. Retrying...");
                            return true;
                        case 40143:
                            System.Diagnostics.Debug.WriteLine("SQL Error:Connection could not be initialized. Retrying...");
                            return true;
                        case 40197:
                            System.Diagnostics.Debug.WriteLine("SQL Error:The service encountered an error processing your request. Retrying...");
                            return true;
                        case 40501:
                            System.Diagnostics.Debug.WriteLine("SQL Error:The server is busy. Retrying...");
                            return true;
                        case 40613:
                            System.Diagnostics.Debug.WriteLine("SQL Error:The database is currently unavailable. Retrying...");
                            return true;
                            
                       

                    }
                }
            }

            // For all others, do not retry.
            return false;
        }


    }
}