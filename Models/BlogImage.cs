﻿namespace ProniaWebApp.Models
{
    public class BlogImage
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public Blog Blog { get; set; }
    }
}
