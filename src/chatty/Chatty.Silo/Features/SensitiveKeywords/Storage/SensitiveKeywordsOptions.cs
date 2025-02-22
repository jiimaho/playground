using System.ComponentModel.DataAnnotations;

namespace Chatty.Silo.Features.SensitiveKeywords.Storage;

public class SensitiveKeywordsOptions
{
    public const string SectionName = "SensitiveKeywords"; 
    
    [Required]
    public IEnumerable<string> Keywords { get; set; } = [];
}