using System.Collections;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Core.Validation
{
    /// <summary>
    /// Valida um objeto dinamicamente inspecionando atributos nas propriedades via Reflection.
    /// Atualmente suporta [RequiredAttribute]. Registre um ILogger&lt;DynamicValidator&gt; no DI para logs.
    /// </summary>
    public class DynamicValidator
    {
        private readonly ILogger<DynamicValidator>? _logger;

        public DynamicValidator(ILogger<DynamicValidator>? logger = null)
        {
            _logger = logger;
        }

        public ValidationResult Validate(object obj)
        {
            var result = new ValidationResult();
            if (obj == null)
            {
                result.Errors.Add(new ValidationError { PropertyName = string.Empty, Message = "Objeto nulo" });
                return result;
            }

            var type = obj.GetType();
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var pi in props)
            {
                try
                {
                    var value = pi.GetValue(obj);

                    // PROCESSA RequiredAttribute (custom)
                    var requiredAttr = pi.GetCustomAttribute<RequiredAttribute>();
                    if (requiredAttr != null)
                    {
                        bool invalid = false;

                        if (value == null)
                        {
                            invalid = true;
                        }
                        else if (pi.PropertyType == typeof(string))
                        {
                            var s = (string?)value;
                            if (string.IsNullOrWhiteSpace(s)) invalid = true;
                        }
                        else if (pi.PropertyType == typeof(Guid))
                        {
                            if (value is Guid g && g == Guid.Empty) invalid = true;
                        }
                        else if (typeof(IEnumerable).IsAssignableFrom(pi.PropertyType) && pi.PropertyType != typeof(string))
                        {
                            // coleções -> verifica se tem elementos
                            if (value is IEnumerable enm)
                            {
                                invalid = !enm.GetEnumerator().MoveNext();
                            }
                        }
                        else if (pi.PropertyType.IsValueType)
                        {
                            // para value types (int, decimal, datetime...) considera inválido se igual ao default(T)
                            var defaultValue = Activator.CreateInstance(pi.PropertyType);
                            if (Equals(value, defaultValue)) invalid = true;
                        }

                        if (invalid)
                        {
                            result.Errors.Add(new ValidationError
                            {
                                PropertyName = pi.Name,
                                Message = requiredAttr.ErrorMessage ?? $"{pi.Name} is required."
                            });
                        }
                    }

                    // Aqui você pode adicionar processamento para outros atributos customizados (Range, Regex, etc.)
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Erro ao validar propriedade {Property} do tipo {Type}", pi.Name, type.FullName);
                    result.Errors.Add(new ValidationError { PropertyName = pi.Name, Message = "Erro ao validar propriedade" });
                }
            }

            return result;
        }

        // Helper estático para uso rápido sem DI
        public static ValidationResult ValidateStatic(object obj, ILogger? logger = null)
        {
            var v = new DynamicValidator(logger as ILogger<DynamicValidator>);
            return v.Validate(obj);
        }
    }
}
