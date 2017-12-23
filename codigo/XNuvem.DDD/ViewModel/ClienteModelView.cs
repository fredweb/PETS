using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;

namespace XNuvem.DDD.ViewModel
{
    [Validator(typeof(ClienteValidation))]
    public class ClienteModelView
    {
        [Display(Name = "N° do Cliente", Description = "Codigo do Cliente")]
        public long? Id { get; set; }

        [Display(Name = "Nome do Cliente", Description = "Nome do Cliente")]
        public string Nome { get; set; }

        [Display(Name = "Data Nascimento", Description = "Data Nascimento")]
        public string DataNascimento { get; set; }
    }

    public class ClienteValidation : AbstractValidator<ClienteModelView>
    {
        public ClienteValidation()
        {
            RuleFor(w => w.Nome).NotEmpty().WithName("Nome do Cliente é obrigatório.");
            RuleFor(w => w.DataNascimento).NotEmpty().WithMessage("Data de Nascimento é obrigatório.");
            RuleFor(w => w.DataNascimento).Custom((data, contexto) =>
            {
                var auxDate = default(DateTime);
                if (!DateTime.TryParse(data, out auxDate))
                    contexto.AddFailure("Data Invalida");
            });
        }
    }
}