using Microsoft.SqlServer.Management.Common;
using System;


namespace AM_SqlServer
{
    /// <summary>
    /// Wrapper around SQL Sever Agent / SSMS / SSRS / SSIS jobs
    /// </summary>
    public class SqlServerManager
    {
        public bool ConnectToRemoteDatabase(string server, string instance, string username, string password)
        {
            ServerConnection srvConn = new ServerConnection(server);
            

            return true;
        }

        public bool ConnectToLocalDatabase(string instance, string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
