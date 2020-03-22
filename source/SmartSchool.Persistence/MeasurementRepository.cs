using SmartSchool.Core.Contracts;
using SmartSchool.Core.Entities;
using System.Linq;

namespace SmartSchool.Persistence
{
    public class MeasurementRepository : IMeasurementRepository
    {
        private ApplicationDbContext _dbContext;

        public MeasurementRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public  void AddRange(Measurement[] measurements)
        {
            _dbContext.Measurements.AddRange(measurements);
        }

        public Measurement[] GetMeasurementsByLocationAndName(string location, string name) => 
            _dbContext.Measurements.OrderByDescending(m => m.Sensor.Location == location && m.Sensor.Name == name).ToArray();
    }
}