﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakeYouPro.Bource.CRM.Dal.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class LeadEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Role { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DateCreate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? MiddleName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Surname { get; set; }

        [Required]
        [Phone]
        [Column(TypeName = "nvarchar(30)")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(3)")]
        [MinLength(2)]
        public string Citizenship { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string PassportNumber { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(300)")]
        public string Registration { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string? Comment { get; set; }

        public virtual List<AccountEntity> Accounts { get; set; } = new List<AccountEntity> ();
    }
}