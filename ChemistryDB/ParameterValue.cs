using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChemistryDB
{
    public partial class ParameterValue
    {
        [Key]
        public long Id { get; set; }
        public long MaterialId { get; set; }
        public long ParameterId { get; set; }
        public double Value { get; set; }

        public virtual Material Material { get; set; }
        public virtual Parameter Parameter { get; set; }
    }
}
