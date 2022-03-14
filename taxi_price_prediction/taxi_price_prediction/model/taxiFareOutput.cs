using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace taxi_price_prediction.model
{
    //çıktı alınacakken kullanılacak model
    class taxiFareOutput
    {
        [ColumnName("Score")]
        public float FareAmount { get; set; }
    }
}
