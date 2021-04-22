using System;
using System.Collections.Generic;
using System.Text;

namespace EmmaJunoKlimat
{
    public class Observation
    {
        //DateTime currentdate = new DateTime();
        //Primary Key
        public int Id { get; set; }
        public DateTime? CurrentDate { get; set; }
        //Foreign Key som refererar till primärnyckeln i Observer
        public int? ObserverId { get; set; }
        //Foreign Key som refererar till primärnyckeln i Geolocation
        public int? GeolocationId { get; set; }

        public override string ToString()
        {
            return $"{CurrentDate}";

        }
    }
}
