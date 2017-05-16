﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Data.Entity;

namespace BeerReviews.Database.Models
{
    public class Style
    {
        public int StyleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Beer> Beers { get; set; }
    }
}