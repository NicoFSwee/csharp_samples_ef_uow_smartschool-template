using SmartSchool.Core.Contracts;
using SmartSchool.Core.Entities;

namespace SmartSchool.Core.Contracts
{
    public interface ISensorRepository
    {
        Measurement[] GetMeasurementsFromSensorWithLocationAndName(string location, string name);

        Sensor[] GetAll();
    }
}
