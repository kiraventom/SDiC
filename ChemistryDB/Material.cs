using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChemistryDB
{
    public partial class Material
    {
        public Material()
        {
            ParameterValue = new HashSet<ParameterValue>();
        }

        [Key]
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ParameterValue> ParameterValue { get; set; }
    }
}
