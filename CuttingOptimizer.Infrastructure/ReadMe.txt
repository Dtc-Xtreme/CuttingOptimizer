Add-Migration init
Update-Database

## Nieuwe reeks nummer per jaar. ##
File: CuttingOptimizerDbContext
modelBuilder.Entity<Quotation>().Property(c => c.ID).HasDefaultValueSql("NEXT VALUE FOR OPNR2023");
// Pas OPNR2023 aan naar OPNR2024