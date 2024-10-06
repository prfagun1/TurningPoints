using Microsoft.Extensions.Configuration;
using System;

namespace frontend.Services
{
    public class VariaveisAmbiente
    {
        private readonly IConfiguration _configuration;
        public string? UrlAPI;
        public string? Token { get; set; }
        public string? TokenTempoExpiracaoMinutos { get; set; }
        public string? Usuario { get; set; }
        public DateTime? DataExpiracao { get; set; }

        public VariaveisAmbiente(IConfiguration configuration)
        {
            _configuration = configuration;
            UrlAPI = _configuration["ParametrosAmbiente:UrlApi"] ?? string.Empty;
            Usuario = "Não autenticado";

            if (string.IsNullOrEmpty(UrlAPI))
                throw new ArgumentException("Não foi localizada a UrlAPI no arquivo de configuração");
        }
    }
}
