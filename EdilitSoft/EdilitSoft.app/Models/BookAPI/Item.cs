using static Microsoft.EntityFrameworkCore.SqlServer.Query.Internal.SqlServerOpenJsonExpression;

namespace EdilitSoft.app.Models.BookAPI
{
    public class Item
    {
        public string Kind { get; set; }
        public string Id { get; set; }
        public string Etag { get; set; }
        public string SelfLink { get; set; }
        public VolumeInfo VolumeInfo { get; set; }
        public SaleInfo SaleInfo { get; set; }
        public AccessInfo AccessInfo { get; set; }
        public SearchInfo SearchInfo { get; set; }
    }
}
