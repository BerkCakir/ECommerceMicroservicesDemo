using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.ProductCatalog.Settings
{
    // This method is called "Options Pattern" - reading configuration file via classes
    public interface IDatabaseSettings
    {
        public string ProductCategoryCollectionName { get; set; }
        public string ProductCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DataBaseName { get; set; }
    }
}
