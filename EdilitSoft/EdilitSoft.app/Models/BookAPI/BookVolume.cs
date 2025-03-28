namespace EdilitSoft.app.Models.BookAPI
{
    public class BookVolume
    {
        public string Kind { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}
