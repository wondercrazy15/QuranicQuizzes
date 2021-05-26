using System;
using System.Collections.Generic;

namespace QuranicQuizzes.Models
{
    public class Categories
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public List<SubCategories> SubCategories { get; set; }
    }

    public class SubCategories
    {
        public int ID { get; set; }
        public string SubCategoryName { get; set; }
        public string ImageURL { get; set; }
    }
}
