using System.ComponentModel.DataAnnotations;

namespace CodingChallenge.Data.DataModels
{
 public class UserDataModel : BaseDataModel
 {
 [Required]
 public required string UserName { get; set; }
 }
}
