using System;
using System.Collections.Generic;
using System.Text;

namespace EmmaJunoKlimat
{
    public class Measurement
    {
        public int? Id { get; set; }
        public double? Value { get; set; }
        public int? ObservationId { get; set; }
        public int? CategoryID { get; set; }

        public override string ToString()
        {
            return $"{Value} {CategoryID}";
        }

    }
}
