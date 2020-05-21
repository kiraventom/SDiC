using App.Common.Abstraction;

namespace App.DbEdit.Abstraction
{
    public abstract class DbEditModel : Model
    {
        protected abstract dynamic Context { get; set; }
        public abstract dynamic GetAllTables();
        public void Save() => Context.SaveChanges();
        ~DbEditModel() => Context.Dispose();
    }
}
