﻿namespace ProniaWebApp.Models
{
    public class BlogTag : BaseAuditableEntity
    {
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }

    }
}
