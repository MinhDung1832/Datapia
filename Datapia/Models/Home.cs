using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datapia.Models
{
    public class overview_area
    {
        public string area { get; set; }
        public string wh_code { get; set; }
        public string total_rack { get; set; }
        public string total_shelf { get; set; }
        public string total_shelf_floor { get; set; }
        public string sum_stored_volume { get; set; }
        public string sum_volume { get; set; }
        public string total_stored_bin { get; set; }
        public string volume_percent { get; set; }
        public string bin_percent { get; set; }
    }
}
