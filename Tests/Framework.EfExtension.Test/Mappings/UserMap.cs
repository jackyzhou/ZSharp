﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.EfExtension.Test
{
    public partial class UserMap
        : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // table
            ToTable("User", "dbo");

            // keys
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Id)
                .HasColumnName("Id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();
            Property(t => t.EmailAddress)
                .HasColumnName("EmailAddress")
                .HasMaxLength(250)
                .IsRequired();
            Property(t => t.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(200)
                .IsOptional();
            Property(t => t.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(200)
                .IsOptional();
            Property(t => t.Avatar)
                .HasColumnName("Avatar")
                .IsOptional();
            Property(t => t.CreatedDate)
                .HasColumnName("CreatedDate")
                .IsRequired();
            Property(t => t.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .IsRequired();
            Property(t => t.RowVersion)
                .HasColumnName("RowVersion")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .HasMaxLength(8)
                .IsRowVersion()
                .IsRequired();
            Property(t => t.PasswordHash)
                .HasColumnName("PasswordHash")
                .HasMaxLength(86)
                .IsRequired();
            Property(t => t.PasswordSalt)
                .HasColumnName("PasswordSalt")
                .HasMaxLength(5)
                .IsRequired();
            Property(t => t.Comment)
                .HasColumnName("Comment")
                .IsOptional();
            Property(t => t.IsApproved)
                .HasColumnName("IsApproved")
                .IsRequired();
            Property(t => t.LastLoginDate)
                .HasColumnName("LastLoginDate")
                .IsOptional();
            Property(t => t.LastActivityDate)
                .HasColumnName("LastActivityDate")
                .IsRequired();
            Property(t => t.LastPasswordChangeDate)
                .HasColumnName("LastPasswordChangeDate")
                .IsOptional();
            Property(t => t.AvatarType)
                .HasColumnName("AvatarType")
                .HasMaxLength(150)
                .IsOptional();

            // Relationships

        }
    }
}
