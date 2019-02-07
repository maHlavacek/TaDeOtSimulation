using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TadeotSimulation.Core
{
    public class Controller
    {
        private const int MIN_PEOPLE_PER_PRESENTATION = 10;
        private const int MAX_PEOPLE_PER_PRESENTATION = 20;
        private const int MAX_WAITING_MINUTES = 40;

        public event EventHandler<string> Log;


        public Controller()
        {
        }

        /// <summary>
        /// Besucher werden aus der csv-Datei in den Arbeitsvorrat eingelesen
        /// </summary>
        public void ReadVisitorsFromCsv()
        {
            string fileName = Utils.MyFile.GetFullNameInApplicationTree("Visitors.csv");
            string[] lines = File.ReadAllLines(fileName, Encoding.Default);



        }

        /// <summary>
        /// Die Simulation startet 60 Minuten, bevor der erste Besucher laut csv-Datei kommt.
        /// Der Beschleunigungsfaktor wird auf 6000 gesetzt.
        /// </summary>
        public void StartSimulation()
        {
        }


    }
}
