using System.ComponentModel;

namespace EdilitSoft.app.Models.BookAPI
{
    public class ImageLinks
    {
        public string SmallThumbnail { get; set; }

        [DisplayName("Illustration")]
        public string Thumbnail { get; set; }
    }
}
