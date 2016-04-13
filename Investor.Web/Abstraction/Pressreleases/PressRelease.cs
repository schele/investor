using System;
using System.Collections.Generic;
using Investor.Models.Models.PressReleases;

namespace Investor.Abstraction.Pressreleases
{
    public class PressRelease
    {
        public DateTime Published { get; set; }

        public DateTime Modified { get; set; }

        public string Issuer { get; set; }

        public string Type { get; set; }

        public int Id { get; set; }

        public string Language { get; set; }

        public string Headline { get; set; }

        public string Body { get; set; }

        public List<Attachment> Attachment { get; set; }
    }
}