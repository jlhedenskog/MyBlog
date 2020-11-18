using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Utilities
{
    public static class DataHelper
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            //The default connection string will come from appSettings like usual
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            //It will be automatically overwritten if we are running on Heroku
            var databseURL = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databseURL) ? connectionString : BuildConnectionString(databseURL);
        }
    }
}
