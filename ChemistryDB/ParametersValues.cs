using System;
using System.Collections.Generic;

namespace ChemistryDB
{
    public partial class ParametersValues
    {
        public ParametersValues()
        {
            Id = -1;
        }

        public ParametersValues(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
        public long MaterialId { get; set; }
        public long ParameterId { get; set; }
        public double Value { get; set; }

        public virtual Materials Material { get; set; }
        public virtual Parameters Parameter { get; set; }
    }
}
