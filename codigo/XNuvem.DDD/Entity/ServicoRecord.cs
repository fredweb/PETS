using XNuvem.Data;

namespace XNuvem.DDD.Entity
{
    public class ServicoRecord
    {
        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Sigla { get; set; }
        public virtual decimal Valor { get; set; }
    }

    public class ServicoMap : EntityMap<ServicoRecord>
    {
        public ServicoMap()
        {
            Table("Servico");
            Id(w => w.Id).GeneratedBy.Identity();
            Map(w => w.Nome).Length(255).Not.Nullable();
            Map(w => w.Sigla).Length(3).Not.Nullable();
            Map(w => w.Valor).Precision(2).Length(7).Default("0.00").Not.Nullable();
        }
    }
}