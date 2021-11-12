﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace DataAccess
{
    public class BaseDAO
    {
        public StockDataProvider dataProvider { get; set; } = null;
        public SqlConnection connection = null;
        //----------------------------------------------
        public BaseDAO()
        {
            var connectionString = GetConnectionString();
            dataProvider = new StockDataProvider(connectionString);
        }

        //----------------------------------------------
        public string GetConnectionString()
        {
            string connectionString;
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            connectionString = config["ConnectionString:SalesManagement"];
            return connectionString;
        }
        public void CloseConnection() => dataProvider.CloseConnection(connection);
    }
}
