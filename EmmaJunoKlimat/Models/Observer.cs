using System;
using System.Collections.Generic;
using System.Text;

namespace EmmaJunoKlimat
{

    /// <summary>
    /// This is the name of the observer
    /// </summary>
    public class Observer
    {

        /// <summary>
        /// Primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// First name Observer
        /// </summary>
        public string Firstname { get; set; }
        /// <summary>
        /// Last name Observer
        /// </summary>
        public string Lastname { get; set; }


        public override string ToString()
        {
            return $"{Firstname} {Lastname}";
        }
    }

    
}
