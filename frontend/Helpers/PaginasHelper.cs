using entities;
using entities.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace frontend.Helpers
{
    public class PaginasHelper
    {
        public static void ConfiguraEntidadeOrdenada(string ordenacao, SearchFilter filtro)
        {
            if (filtro.SortField == ordenacao)
            {
                filtro.SortType = filtro.SortType == Enumerators.SortOrder.Ascending ? Enumerators.SortOrder.Descending : Enumerators.SortOrder.Ascending;
            }
            else
            {
                filtro.SortField = ordenacao;
                filtro.SortType = Enumerators.SortOrder.Ascending;
            }
            filtro.PageIndex = 1;
        }

        public static async Task AdicionaCor(int id, IJSRuntime JSRuntime, Dictionary<int, ElementReference> LinhasTabela)
        {
            await JSRuntime.InvokeVoidAsync("TabelaAlteraCorLinhaAdicionar", LinhasTabela.GetValueOrDefault(id));
        }

        public static async Task RemoveCor(int id, IJSRuntime JSRuntime, Dictionary<int, ElementReference> LinhasTabela)
        {
            await JSRuntime.InvokeVoidAsync("TabelaAlteraCorLinhaRemover", LinhasTabela.GetValueOrDefault(id));
        }

        public static string GetMensagemErro()
        {
            return $"Ocorreu um erro, caso o problema persista favor contactar a TI informando a tela e o horário do problema: {DateTime.Now}";
        }


    }

}