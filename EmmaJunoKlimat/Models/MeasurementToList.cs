using System;
using System.Collections.Generic;
using System.Text;

namespace EmmaJunoKlimat
{
    public class MeasurementToList
    {

        public int? Id { get; set; }
        public double? Value { get; set; }

        public string Category { get; set;}

        public string Suit { get; set; }

        public string Unit { get; set; }


        public override string ToString()
        {
            return $"{Value} {Unit} {Category} {Suit}";
        }
    }
}
