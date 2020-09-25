using System;

namespace WebApi_test.Clases
{
    public class NoticiaLista
    {
        public NoticiaLista()
        {
            promo_items  = new Promo_items();
            headlines    = new Headlines();
            subheadlines = new subheadlines();
        }

        public string[] _website_urls { get; set; }

        public DateTime display_date { get; set; }

        public Headlines headlines { get; set; }

        public Promo_items promo_items { get; set; }

        public DateTime publish_date { get; set; }

        public subheadlines subheadlines { get; set; }

        public string ImagenBase64 { get; set; }

        public string ContentType { get; set; }
    }
}
