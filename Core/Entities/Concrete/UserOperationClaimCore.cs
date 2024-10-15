using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Concrete;

public class UserOperationClaimCore<TOperationClaim, TUser> : IEntity
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }
    public required TUser User { get; set; }

    [ForeignKey("OperationClaim")]
    public int OperationClaimId { get; set; }
    public required TOperationClaim OperationClaim { get; set; }
}
