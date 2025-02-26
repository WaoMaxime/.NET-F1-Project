using System.ComponentModel.DataAnnotations;
namespace UI.DTO;

public class UpdateF1CarHpDto
{
    [Range(500, 1500)]
    public int F1CarHp { get; set; }
}