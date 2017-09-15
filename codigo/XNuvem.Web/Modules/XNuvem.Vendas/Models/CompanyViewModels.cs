/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XNuvem.Vendas.Models
{
    public class CompanyViewModel
    {
        [Display(Name="Nome da empresa")]
        [Required(ErrorMessage = "É necessário informar o nome da empresa.")]
        public string Name { get; set; }

        [Display(Name="Servidor de licença do SAP")]
        [Required(ErrorMessage="É necessário informar o nome do servidor de licenças.")]
        public string ServerName { get; set; }

        [Display(Name="Banco de dados da empresa")]
        [Required(ErrorMessage="É necessário informar o nome do banco de dados.")]
        public string CompanyDB { get; set; }

        [Display(Name="Usuário do SAP")]
        [Required(ErrorMessage="É necessário informar o nome do usuário.")]
        public string UserName { get; set; }

        [Display(Name="Senha do SAP")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Tamanho mínino do pool")]
        [Required]
        [Range(1, 10)]
        public int MinPoolSize { get; set; }

        [Display(Name = "Tamanho Máximo do pool")]
        [Required]
        [Range(1, 50)]
        public int MaxPoolSize { get; set; }

        [Required]
        [Display(Name = "Connection string", 
                Description = "String de conexão com o banco de dados para consulta")
        ]
        public string ConnectionString { get; set; }

        [Required]
        [Display(Name="Banco de dados da aplicação")]
        public string ApplicationDB { get; set; }
    }

    public class UsageViewModel
    {
        public int? Id { get; set; }
        
        [Required(ErrorMessage="É necessário informar o nome")]
        [Display(Name="Nome para exibição")]
        public string Name { get; set; }
        
        [Required(ErrorMessage="É necessário informar a utilização")]
        [Display(Name="Utilização SAP")]
        public int Usage { get; set; }

        public string UsageText { get; set; }
    }
}