using System;
using System.Collections.Generic;
using System.Text;

namespace EmmaJunoKlimat
{
    public class Area
    {
        //Primary Key
        public int? Id { get; set; }
        public string Name { get; set; }
        //Foreign Key references to Primary Key in Country
        public int? CountryId { get; set; }

        public override string ToString()
        {
           return $"{Name}";
        }        
    }
}
