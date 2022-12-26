﻿using System.ComponentModel.DataAnnotations;

namespace Signals.App.Database.Entities
{
    public class UserEntity
    {
        [Key]
        public Guid Id { get; set; }

        ///TODO: Check all restrictions for all entities, for example this one can have min length restriction
        [MaxLength(50)]
        public string Username { get; set; }

        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDisabled { get; set; }
    }
}
