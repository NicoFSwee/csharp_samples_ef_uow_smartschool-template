using System;
using System.Linq;
using SmartSchool.Core.Entities;
using SmartSchool.Persistence;

namespace SmartSchool.TestConsole
{
	class Program
	{
		static void Main()
		{
			Console.WriteLine("Import der Measurements und Sensors in die Datenbank");
			using (UnitOfWork unitOfWorkImport = new UnitOfWork())
			{
				Console.WriteLine("Datenbank löschen");
				unitOfWorkImport.DeleteDatabase();
				Console.WriteLine("Datenbank migrieren");
				unitOfWorkImport.MigrateDatabase();
				Console.WriteLine("Messwerte werden von measurements.csv eingelesen");
				var measurements = ImportController.ReadFromCsv().ToArray();
				if (measurements.Length == 0)
				{
					Console.WriteLine("!!! Es wurden keine Messwerte eingelesen");
					return;
				}

				Console.WriteLine(
						$"  Es wurden {measurements.Count()} Messwerte eingelesen, werden in Datenbank gespeichert ...");
				unitOfWorkImport.MeasurementRepository.AddRange(measurements);
				int countSensors = measurements.GroupBy(m => m.Sensor).Count();
				int savedRows = unitOfWorkImport.SaveChanges();
				Console.WriteLine(
						$"{countSensors} Sensoren und {savedRows - countSensors} Messwerte wurden in Datenbank gespeichert!");
				Console.WriteLine();
			}

			using (UnitOfWork unitOfWork = new UnitOfWork())
			{
				Console.WriteLine("Import beendet, Test der gespeicherten Daten");
				Console.WriteLine("--------------------------------------------");
				Console.WriteLine();

				var count = unitOfWork.MeasurementRepository.GetMeasurementsByLocationAndName("livingroom", "temperature").Count();
				Console.WriteLine($"Anzahl Messwerte für Sensor temperature in location livingroom: {count}");
				Console.WriteLine();

				var greatestmeasurements = unitOfWork.SensorRepository.GetMeasurementsFromSensorWithLocationAndName("livingroom", "temperature")
																			.OrderByDescending(_ => _.Value)
																			.ThenByDescending(_ => _.Time)
																			.Take(3)
																			.ToArray();

				Console.WriteLine("Letzte 3 höchste Temperaturmesswerte im Wohnzimmer");
				WriteMeasurements(greatestmeasurements);
				Console.WriteLine();

				var average = unitOfWork.SensorRepository.GetMeasurementsFromSensorWithLocationAndName("office", "co2")
															.Where(_ => _.Value > 300 && _.Value < 5000)
															.Average(_ => _.Value);

				Console.WriteLine($"Durchschnitt der gültigen Co2-Werte (>300, <5000) im office: {average}");
				Console.WriteLine();
				Console.WriteLine("Alle Sensoren mit dem Durchschnitt der Messwerte");
				WriteAllSensorsWithAverageValues(unitOfWork.SensorRepository.GetAll());

				unitOfWork.SaveChanges();
			}

			Console.Write("Beenden mit Eingabetaste ...");
			Console.ReadLine();
		}

		private static void WriteAllSensorsWithAverageValues(Sensor[] sensors)
		{
			Console.WriteLine($"{"Location", -20}{"Name", -20}{ "Value", 0}");
			foreach (var sensor in sensors)
			{
				Console.WriteLine($"{sensor.Location, -20}{sensor.Name, -20}{sensor.Measurements.Average(_ => _.Value),0:f2}");
			}
		}

		private static void WriteMeasurements(Measurement[] measurements)
		{
			Console.WriteLine("Date       Time     Value");
			for (int i = 0; i < measurements.Length; i++)
			{
				Console.WriteLine($"{measurements[i].Time} {measurements[i].Value}°");
			}
		}
	}
}
