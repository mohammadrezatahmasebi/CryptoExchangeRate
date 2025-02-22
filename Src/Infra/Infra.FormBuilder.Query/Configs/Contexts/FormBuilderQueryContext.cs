using Microsoft.EntityFrameworkCore;

namespace Si24.Infra.FormBuilder.Query.Configs.Contexts;

public class FormBuilderQueryContext : DbContext
{
    public const string SCHEMA = "FormBuilder";

    public FormBuilderQueryContext(DbContextOptions<FormBuilderQueryContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging(true);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.HasDefaultSchema(SCHEMA);
        
        modelBuilder.Entity<Step>()
            .HasKey(p => p.Id);


        modelBuilder.Entity<Step>().HasMany(p => p.Items).WithOne();


        modelBuilder.Entity<StepItem>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<StepItem>()
            .Property(p => p.IsActive).HasDefaultValue(true);

 
        modelBuilder.Entity<StepItem>().HasMany(p => p.Fields).WithOne();
        
        modelBuilder.Entity<StepField>()
            .HasKey(p => p.Id);
        
        modelBuilder.Entity<StepField>().HasMany(p => p.Options).WithOne();
        
        modelBuilder.Entity<StepField>().HasMany(p => p.TemplateFields).WithOne();
        
        modelBuilder.Entity<StepField>()
            .HasKey(p => p.Id);
      
        modelBuilder.Entity<StepField>()
            .Property(p => p.IsActive).HasDefaultValue(true);


        modelBuilder.Entity<StepField>().HasMany(p => p.Options).WithOne();


        modelBuilder.Entity<StepFieldOption>()
            .HasKey(p => p.Id);
        
        modelBuilder.Entity<StepFieldOption>()
            .Property(p => p.IsActive).HasDefaultValue(true);
        

        modelBuilder.Entity<StepFieldOption>().HasMany(p => p.Fields).WithOne();
   

        base.OnModelCreating(modelBuilder);

    }
}