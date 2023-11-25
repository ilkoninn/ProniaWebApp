﻿namespace ProniaWebApp.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product>? Product { get; set; }
        public ICollection<Blog>? Blog { get; set; }    
    }
}
