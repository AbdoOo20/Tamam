using Tamam.Application.Common;

namespace Tamam.Application.DTOs.Products;

public class ProductQuery : PagedQuery
{
    public bool? IsActive { get; set; }
}
