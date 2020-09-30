using System.Collections.Generic;

namespace BooksApi.Models
{
    public class PngstoreDatabaseSettings : IPngstoreDatabaseSettings
    {
        public List<string> PngCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IPngstoreDatabaseSettings
    {
        List<string> PngCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}