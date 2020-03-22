using SmartSchool.Core.Contracts;
using SmartSchool.Core.Entities;
using System.Linq;

namespace SmartSchool.Persistence
{
    public class SensorRepository : ISensorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SensorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Sensor[] GetAll() => _dbContext.Sensors.OrderBy(s => s.Location)
                                                        .ThenBy(s => s.Name)
                                                        .ToArray();

        public Measurement[] GetMeasurementsFromSensorWithLocationAndName(string location, string name) =>
            _dbContext.Sensors.FirstOrDefault(s => s.Location == location && s.Name == name)
            .Measurements.ToArray();

    }
}