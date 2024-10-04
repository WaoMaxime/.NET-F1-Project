using System.ComponentModel.DataAnnotations;
using DataAccessLayer;
using Domain;

namespace BusinessLayer
{
    public class Manager : IManager
    {
        private readonly IRepository _repository;

        public Manager(IRepository repository)
        {
            _repository = repository;
        }

        public FastestLap GetFastestLap(int id) => _repository.ReadFastestLap(id);

        public IEnumerable<FastestLap> GetAllFastestLaps() => _repository.ReadAllFastestLaps();

        public IEnumerable<FastestLap> GetFastestLapsByCircuit(string circuit) => _repository.ReadFastestLapsByCircuit(circuit);

        public FastestLap AddFastestLap(string circuit, int airTemperature, int trackTemperature, TimeSpan lapTime, DateTime dateOfRecord, F1Car car)
        {
            var newLap = new FastestLap(circuit, airTemperature, trackTemperature, lapTime, dateOfRecord, car);
            ValidateModel(newLap);
            _repository.CreateFastestLap(newLap);
            return newLap;
        }

        public F1Car GetF1Car(int id) => _repository.ReadF1Car(id);

        public IEnumerable<F1Car> GetAllF1Cars() => _repository.ReadAllF1Cars();

        public IEnumerable<F1Car> GetF1CarsByTeam(F1Team team) => _repository.ReadF1CarsByTeam(team);

        public F1Car AddF1Car(F1Team team, string chasis, int constructorsPosition, double driversPositions, DateTime manufactureDate, TyreType tyres, double? enginePower = null)
        {
            var newCar = new F1Car(team, chasis, constructorsPosition, driversPositions, manufactureDate, tyres, enginePower);
            ValidateModel(newCar);
            _repository.CreateF1Car(newCar);
            return newCar;
        }

        private void ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model);
            if (!Validator.TryValidateObject(model, context, validationResults, true))
            {
                throw new ValidationException("Validation failed for the following properties: " +
                                              string.Join(", ", validationResults));
            }
        }
    }
}
