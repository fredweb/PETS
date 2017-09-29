using FluentNHibernate.Mapping;

namespace XNuvem.Tests.Data.Model
{
    public class Fool
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class FoolMap : ClassMap<Fool>
    {
        public FoolMap() {
            Table("Fools");

            Id(x => x.Id).GeneratedBy.Identity();
            
            Map(x => x.Name);
        }
    }
}
