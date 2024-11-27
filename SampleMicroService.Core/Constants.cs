using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMicroService.Core;

public static class Constants
{
    public static class EnvironmentVariables
    {
    public static string MongoDbConnectionString = "mongodb://host.docker.internal:27017";
    public static string MongoDbDatabaseName = "Product";
    }
  
}
