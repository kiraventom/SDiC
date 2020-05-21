namespace ChemistryDB
{
    public partial class Materials
    {
        public Materials()
        {
            Id = -1;
            Name = string.Empty;
        }

        public Materials(long id)
        {
            Id = id;
            Name = string.Empty;
        }

        public long Id { get; set; }
        public string Name { get; set; }
    }
}
