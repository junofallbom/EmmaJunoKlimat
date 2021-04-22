using System;
using System.Collections.Generic;
using System.Text;

namespace EmmaJunoKlimat
{
    public class Category
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? BaseCategoryID { get; set; }
        public int? UnitId { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
