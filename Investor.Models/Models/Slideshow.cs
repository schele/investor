namespace Investor.Models.Models
{
    public class Slideshow
    {
        public string Src { get; set; }

        public FocalPointObject FocalPoint { get; set; }
    }

    public class FocalPointObject
    {
        public decimal Left { get; set; }

        public decimal Top { get; set; }
    }
}