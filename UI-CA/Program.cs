using BusinessLayer;
using DataAccessLayer.EF;
using Microsoft.EntityFrameworkCore;
using UI_CA;

var optionsBuilder = new DbContextOptionsBuilder<F1CarDbContext>()
    .UseSqlite("Data Source=f1cars.db");

using (var context = new F1CarDbContext(optionsBuilder.Options))
{
    
    bool databaseCreated = context.CreateDatabase(deleteIfExists: true); 
    
    
    if (databaseCreated)
    {
        Console.WriteLine("Seeding the database...");
        DataSeeder.Seed(context);  
        
        var cars = context.F1Cars.ToList();
        var laps = context.FastestLaps.ToList();
    }
    
    var repository = new Repository(context);
    var manager = new Manager(repository);

    var ui = new ConsoleUi(manager);
    ui.Run();
}