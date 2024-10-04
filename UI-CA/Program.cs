using BusinessLayer;
using DataAccessLayer;
using UI_CA;


var repository = new InMemoryRepository();
var manager = new Manager(repository);
InMemoryRepository.Seed();
var ui = new ConsoleUi(manager);
ui.Run();
