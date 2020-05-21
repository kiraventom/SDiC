namespace ChemistryDB
{
    public partial class Parameters
    {
        public Parameters()
        {
            Id = -1;
            Name = string.Empty;
        }

        public Parameters(long id)
        {
            Id = id;
            Name = string.Empty;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public long TypeId { get; set; }
        public long? UnitId { get; set; }

        public virtual ParametersTypes Type { get; set; }
        public virtual Units Unit { get; set; }
    }
}
