﻿using System.ComponentModel.DataAnnotations;

namespace Domain;

public class CarTyre
{
    [Required]
    public int CarId { get; set; }

    public F1Car Car { get; set; }

    [Required]
    public TyreType Tyre { get; set; }

    [Range(10, 50)]
    public int TyrePressure { get; set; }

    [Range(50, 150)]
    public int OperationalTemperature { get; set; }

    public int? RaceId { get; set;} 

    public Race Race { get; set; }
}


