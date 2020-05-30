using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChemistryDB
{
    public partial class Parameter
    {
        public Parameter()
        {
            ParameterValue = new HashSet<ParameterValue>();
        }

        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public long TypeId { get; set; }
        public long UnitId { get; set; }

        public virtual ParameterType Type { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ICollection<ParameterValue> ParameterValue { get; set; }
    }
}
