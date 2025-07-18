﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain;

public enum F1Team
{
    Mercedes,
    RedBull,
    Ferrari,
    Mclaren,
    AstonMartin
}

public class F1Car : IValidatableObject
{
    [Key]
    public int Id { get; set; } 

    [Required]
    public F1Team Team { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Chasis { get; set; }

    [Range(1, 10)]
    public int ConstructorsPosition { get; set; }

    [Range(0, 50)]
    public double DriversPositions { get; set; }

    public DateTime ManufactureDate { get; set; }

    [Required]
    public TyreType Tyres { get; set; }

    [Range(500, 1500)]
    public double? EnginePower { get; set; }
    
    public string? UserId { get; set; }
    public IdentityUser? User { get; set; }
    public ICollection<FastestLap> FastestLaps { get; set; } = new List<FastestLap>();
    public ICollection<CarTyre> CarTyres { get; set; } = new List<CarTyre>();
    
    public F1Car(
        F1Team team,
        string chasis,
        int constructorsPosition,
        double driversPositions,
        DateTime manufactureDate,
        TyreType tyres, IdentityUser user, string userId,
        double? enginePower = null)
    {
        Team = team;
        Chasis = chasis;
        ConstructorsPosition = constructorsPosition;
        DriversPositions = driversPositions;
        ManufactureDate = manufactureDate;
        Tyres = tyres;
        EnginePower = enginePower;
        User = user;
        UserId = userId;
    }
    
    public F1Car(
        F1Team team,
        string chasis,
        int constructorsPosition,
        double driversPositions,
        DateTime manufactureDate,
        TyreType tyres, IdentityUser user,
        double? enginePower = null)
    {
        Team = team;
        Chasis = chasis;
        ConstructorsPosition = constructorsPosition;
        DriversPositions = driversPositions;
        ManufactureDate = manufactureDate;
        Tyres = tyres;
        EnginePower = enginePower;
        User = user;
    }
    
    public F1Car(
        F1Team team,
        string chasis,
        int constructorsPosition,
        double driversPositions,
        DateTime manufactureDate,
        TyreType tyres,
        double? enginePower = null)
    {
        Team = team;
        Chasis = chasis;
        ConstructorsPosition = constructorsPosition;
        DriversPositions = driversPositions;
        ManufactureDate = manufactureDate;
        Tyres = tyres;
        EnginePower = enginePower;
    }
    
    public F1Car()
    {
        Team = F1Team.Mercedes;
        Chasis = "Default Chassis";
        ConstructorsPosition = 1;
        DriversPositions = 1.0;
        ManufactureDate = DateTime.Now.AddYears(-1);
        Tyres = TyreType.Medium;
        EnginePower = 1000;
    }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EnginePower <= 0)
        {
            yield return new ValidationResult(
                "Engine Power must be a positive value.",
                new[] { nameof(EnginePower) });
        }

        if (ManufactureDate > DateTime.Now)
        {
            yield return new ValidationResult(
                "Manufacture date cannot be in the future.",
                new[] { nameof(ManufactureDate) });
        }
    }
}
