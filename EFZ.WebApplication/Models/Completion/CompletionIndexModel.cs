using System.Collections.Generic;

namespace EFZ.WebApplication.Models.Completion
{
    public class CompletionIndexModel : BaseIndexModel
    {

        public IList<CompletionVm> Completions { get; set; } = new List<CompletionVm>();
    }
}