using System;
using System.Collections.Generic;

namespace BaltaDataAccess.Models
{
    public class Career
    {
        public Career (Guid id, string title)
        {
            Items = new List<CareerItem>();
            Title = title;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public IList<CareerItem> Items { get; set; }
    }

}