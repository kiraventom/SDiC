using System;
using System.Collections.Generic;

namespace ChemistryDB
{
    public partial class ParametersTypes
    {
        public ParametersTypes()
        {
            Parameters = new HashSet<Parameters>();
            Id = -1;
            Name = string.Empty;
        }

        public ParametersTypes(long id)
        {
            Parameters = new HashSet<Parameters>();
            Id = id;
            Name = string.Empty;
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Parameters> Parameters { get; set; }
    }
}
