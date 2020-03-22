using System;
using System.Collections.Generic;
using System.IO;
using SmartSchool.Core.Entities;
using Utils;

namespace SmartSchool.TestConsole
{
    public class ImportController
    {
        const string Filename = "measurements.csv";

        /// <summary>
        /// Liefert die Messwerte mit den dazugehörigen Sensoren
        /// </summary>
        public static IEnumerable<Measurement> ReadFromCsv()
        {
            List<Measurement> measurements = new List<Measurement>();
            List<Sensor> sensors = new List<Sensor>();

            string measurementsPath = MyFile.GetFullNameInApplicationTree(Filename);
            string[] lines = File.ReadAllLines(measurementsPath);

            foreach (string line in lines)
            {
                string[] data = line.Split(";");

                if(data[0] != "Date")
                {
                    string[] locationAndName = data[2].Split("_");
                    DateTime time = new DateTime();
                    DateTime.TryParse(data[0] + " " + data[1], out time);
                    Sensor tmp = new Sensor
                    {
                        Name = locationAndName[1],
                        Location = locationAndName[0]
                    };
                    
                    if(!sensors.Contains(tmp))
                    {
                        sensors.Add(tmp);
                    }

                    measurements.Add(new Measurement
                    {
                        Sensor = sensors.Find(s => s.Location == tmp.Location && s.Name == tmp.Name),
                        Time = time,
                        Value = Convert.ToDouble(data[3])

                    });
                }
            }

            return measurements;
        }
    }
}
