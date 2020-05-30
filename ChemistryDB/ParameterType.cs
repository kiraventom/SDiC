using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChemistryDB
{
    public partial class ParameterType
    {
        public ParameterType()
        {
            Parameter = new HashSet<Parameter>();
        }

        [Key]
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Parameter> Parameter { get; set; }
    }
}
