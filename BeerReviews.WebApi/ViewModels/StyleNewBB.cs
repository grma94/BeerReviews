using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerReviews.WebApi.ViewModels
{
    public class StyleNewBB
    {
        public int StyleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }
        public virtual ICollection<BeerWithAvg> Beers { get; set; }
    }
}