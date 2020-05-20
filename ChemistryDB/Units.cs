using System;
using System.Collections.Generic;

namespace ChemistryDB
{
    public partial class Units
    {
        public Units()
        {
            Parameters = new HashSet<Parameters>();
            Id = -1;
            Unit = string.Empty;
        }

        public Units(long id)
        {
            Parameters = new HashSet<Parameters>();
            Id = id;
            Unit = string.Empty;
        }

        public long Id { get; set; }
        public string Unit { get; set; }

        public virtual ICollection<Parameters> Parameters { get; set; }
    }
}
