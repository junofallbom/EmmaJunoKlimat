using System;
using System.Collections.Generic;
using System.Text;

namespace EmmaJunoKlimat
{
    public class Geolocation
    {
        //Primary Key
        public int? Id { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        //Foreign Key references to Primary Key in Area
        public int? AreaId { get; set; }

        public override string ToString()
        {
            return $"{Latitude} {Longitude}";
        }
    }
}
