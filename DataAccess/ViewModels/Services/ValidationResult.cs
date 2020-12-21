using System.Collections.Generic;

namespace DataAccess.ViewModels.Services
{
    public class ValidationResult<TOut>
    {
        public Dictionary<TOut, List<string>> Results { get; set; } =
            new Dictionary<TOut, List<string>>();
    }
}
