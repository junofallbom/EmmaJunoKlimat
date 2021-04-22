using System;
using System.Collections.Generic;
using System.Text;

namespace EmmaJunoKlimat
{
   public class Country
    {
        //Primary Key
        public int? Id { get; set; }
        public string CountryName { get; set; }

        public override string ToString()
        {
            return $"{CountryName}";
        }
    }
}
