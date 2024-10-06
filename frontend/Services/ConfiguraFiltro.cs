using System.Reflection;

namespace frontend.Services
{
    public class ConfiguraFiltro
    {
        public static Dictionary<string, object> GetFiltro<T>(T entidade) where T : class
        {
            var filtros = new Dictionary<string, object>();

            if (entidade == null)
                return filtros;

            PropertyInfo[] propriedades = typeof(T).GetProperties();

            foreach (var propriedade in propriedades.Where(x => !x.Name.ToUpper().Contains("ID")))
            {
                var valor = propriedade.GetValue(entidade);

                if (valor != null)
                {
                    if (valor is string strValue && !string.IsNullOrEmpty(strValue))
                    {
                        filtros.Add(propriedade.Name, valor);
                    }
                    else if (!(valor is string) && !valor.Equals(GetDefault(propriedade.PropertyType)))
                    {
                        filtros.Add(propriedade.Name, valor);
                    }
                }
            }

            return filtros;
        }

        private static object? GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }

}
