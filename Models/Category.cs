﻿namespace ProniaWebApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Product { get; set; }
        public List<Blog> Blog { get; set; }

    }
}