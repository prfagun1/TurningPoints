using entities;
using System.Linq.Expressions;
using System.Reflection;

namespace repositorio.Helpers
{
    internal class FilterConfig
    {

        internal static List<Expression<Func<ParameterInternal, bool>>> GetParameterInternalFilter(Dictionary<string, object>? filtroPesquisa)
        {
            var filtro = new List<Expression<Func<ParameterInternal, bool>>>();

            if (filtroPesquisa == null)
                return filtro;

            List<string> camposString = ["Name", "Email", "RecordUser"];
            List<string> camposInteiros = ["Blocked", "Permission"];
            List<string> camposDataMaiorQue = ["BirthDate", "TokenExpiration"];
            List<string> camposDataMenorQue = [];
            List<string> camposGuid = [];

            return RetornaFiltro<ParameterInternal>(filtroPesquisa, filtro, camposString, camposGuid, camposInteiros, camposDataMaiorQue, camposDataMenorQue);
        }

        internal static List<Expression<Func<Chapter, bool>>> GetChapterFilter(Dictionary<string, object>? filtroPesquisa)
        {
            var filtro = new List<Expression<Func<Chapter, bool>>>();

            if (filtroPesquisa == null)
                return filtro;

            List<string> camposString = ["Titulo", "Title"];
            List<string> camposInteiros = [];
            List<string> camposDataMaiorQue = [];
            List<string> camposDataMenorQue = [];
            List<string> camposGuid = [];

            //verificar o filtro para nao pegar somente o id

            var filter = RetornaFiltro<Chapter>(filtroPesquisa, filtro, camposString, camposGuid, camposInteiros, camposDataMaiorQue, camposDataMenorQue);

            Dictionary<string, object> filtros = new Dictionary<string, object>();

            filtroPesquisa.TryGetValue("StoryId", out object storyIdObject);
            if (storyIdObject != null)
            {
                filtro.Add(x => x.StoryId.ToString() == storyIdObject.ToString());
            }

            return filter;
        }
        
        internal static List<Expression<Func<Story, bool>>> GetStoryFilter(Dictionary<string, object>? filtroPesquisa)
        {
            var filtro = new List<Expression<Func<Story, bool>>>();

            if (filtroPesquisa == null)
                return filtro;

            List<string> camposString = ["Title", "Description", "Tags"];
            List<string> camposInteiros = ["RecommendedAge"];
            List<string> camposDataMaiorQue = [];
            List<string> camposDataMenorQue = [];
            List<string> camposGuid = [];

            return RetornaFiltro<Story>(filtroPesquisa, filtro, camposString, camposGuid, camposInteiros, camposDataMaiorQue, camposDataMenorQue);
        }

        internal static List<Expression<Func<Login, bool>>> GetLoginInternalFilter(Dictionary<string, object>? filtroPesquisa)
        {
            var filtro = new List<Expression<Func<Login, bool>>>();

            if (filtroPesquisa == null)
                return filtro;

            List<string> camposString = ["Name", "Value", "RecordUser"];
            List<string> camposInteiros = [];
            List<string> camposDataMaiorQue = [];
            List<string> camposDataMenorQue = [];
            List<string> camposGuid = [];

            return RetornaFiltro<Login>(filtroPesquisa, filtro, camposString, camposGuid, camposInteiros, camposDataMaiorQue, camposDataMenorQue);
        }

        internal static List<Expression<Func<Log, bool>>> GetLogFilter(Dictionary<string, object>? filtroPesquisa)
        {
            var filtro = new List<Expression<Func<Log, bool>>>();

            if (filtroPesquisa == null)
                return filtro;

            List<string> camposString = ["Process", "Type", "Message", "Error", "LoggedUser"];
            List<string> camposInteiros = ["EventoId"];
            List<string> camposDataMaiorQue = ["LoggedDateStart"];
            List<string> camposDataMenorQue = ["LoggedDateEnd"];
            List<string> camposGuid = [];

            return RetornaFiltro<Log>(filtroPesquisa, filtro, camposString, camposGuid, camposInteiros, camposDataMaiorQue, camposDataMenorQue);
        }

        private static List<Expression<Func<T, bool>>> RetornaFiltro<T>(
            Dictionary<string, object> filtroPesquisa,
            List<Expression<Func<T, bool>>> filtro,
            List<string> camposString,
            List<string> camposGuid,
            List<string> camposInteiros,
            List<string> camposDataMaiorQue,
            List<string> camposDataMenorQue)
            where T : class
        {

            foreach (var campo in camposString)
            {
                string? valor = string.Empty;

                if (filtroPesquisa.TryGetValue(campo, out object? valorCampo))
                {
                    if (valorCampo != null)
                        valor = valorCampo.ToString();
                    if (!string.IsNullOrEmpty(valor))
                        filtro.Add(BuildExpression<T, string>(campo, valor, "Contains"));
                }
            }

            foreach (var campo in camposGuid)
            {
                if (filtroPesquisa.TryGetValue(campo, out object? valorCampo) && valorCampo != null && Guid.TryParse(valorCampo.ToString(), out Guid guid))
                {
                    filtro.Add(BuildExpression<T, Guid>(campo, guid, "Equal"));
                }
            }

            foreach (var campo in camposInteiros)
            {
                if (filtroPesquisa.TryGetValue(campo, out object? valorCampo) && valorCampo != null && valorCampo.ToString() != "0")
                {
                    int valor = Convert.ToInt32(valorCampo.ToString());
                    filtro.Add(BuildExpression<T, int>(campo, valor, "Equal"));
                }
            }

            foreach (var campo in camposDataMaiorQue)
            {
                if (filtroPesquisa.TryGetValue(campo, out object? valorCampo) && valorCampo != null && DateTime.TryParse(valorCampo.ToString(), out DateTime data))
                {
                    filtro.Add(BuildExpression<T, DateTime>(campo, data, "GreaterThan"));
                }
            }

            foreach (var campo in camposDataMenorQue)
            {
                if (filtroPesquisa.TryGetValue(campo, out object? valorCampo) && valorCampo != null && DateTime.TryParse(valorCampo.ToString(), out DateTime data))
                {
                    filtro.Add(BuildExpression<T, DateTime>(campo, data, "LessThan"));
                }
            }

            return filtro;
        }


        private static Expression<Func<TEntity, bool>> BuildExpression<TEntity, TProperty>(
            string propertyName,
            TProperty propertyValue,
            string methodName
        ) where TEntity : class
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(parameter, propertyName);
            var propertyValueExpression = Expression.Constant(propertyValue, typeof(TProperty));
            var propertyType = property.Type;

            MethodInfo? method = null;
            Expression propertyValueExpression2;

            if (typeof(TProperty) == typeof(string))
            {
                method = typeof(string).GetMethod(methodName, new[] { typeof(string) });
            }
            if (typeof(TProperty).IsValueType && !typeof(TProperty).IsEnum)
            {
                // Cria uma expressão de igualdade para tipos numéricos
                if (propertyType.IsValueType && Nullable.GetUnderlyingType(propertyType) != null)
                {
                    propertyValueExpression2 = Expression.Convert(propertyValueExpression, propertyType);
                    var equalExpression = Expression.Equal(property, propertyValueExpression2);
                    return Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                }
                else
                {
                    // Se não for nullable, use o valor diretamente
                    propertyValueExpression = Expression.Constant(propertyValue, typeof(TProperty));
                    return Expression.Lambda<Func<TEntity, bool>>(propertyValueExpression, parameter);
                }
            }

            if (method == null)
                ArgumentNullException.ThrowIfNull(method);

            var methodCall = Expression.Call(property, method, propertyValueExpression);
            return Expression.Lambda<Func<TEntity, bool>>(methodCall, parameter);
        }

    }


}
