using System.ComponentModel.DataAnnotations;

namespace LoanTracker.Shared.Models;

public class LoanTypeRequest
{
    public int LoanTypeId { get; set; }

    [Required]
    [Display(Name = "Product Name")]
    public string LoanTypeName { get; set; } = null!;

    [Display(Name = "Product Description")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [Required]
    [Display(Name = "Burmese Product Name")]
    public string BurmeseLoanTypeName { get; set; } = null!;

    [Display(Name = "Burmese Product Description")]
    [DataType(DataType.MultilineText)]
    public string? BurmeseDescription { get; set; }
}

public class LoanTypeResponse
{
    public int LoanTypeId { get; set; }
    public string LoanTypeName { get; set; } = null!;
    public string? Description { get; set; }
    public string BurmeseLoanTypeName { get; set; } = null!;
    public string? BurmeseDescription { get; set; }
    public DateTime? CreatedDate { get; set; }
}
