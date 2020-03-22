using SmartSchool.Core.Entities;

namespace SmartSchool.Core.Contracts
{
    public interface IMeasurementRepository
    {
        void AddRange(Measurement[] measurements);

        Measurement[] GetMeasurementsByLocationAndName(string location, string name);
    }
}
