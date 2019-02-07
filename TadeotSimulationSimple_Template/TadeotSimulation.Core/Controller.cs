using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TadeotSimulation.Core
{
    public class Controller
    {
        #region Fields
        private const int MIN_PEOPLE_PER_PRESENTATION = 10;
        private const int MAX_PEOPLE_PER_PRESENTATION = 20;
        private const int MAX_WAITING_MINUTES = 40;

        public event EventHandler<string> Log;

        private List<Visitor> _listOdVisitors;
        #endregion

        #region Constructor
        public Controller()
        {
            _listOdVisitors = new List<Visitor>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Besucher werden aus der csv-Datei in den Arbeitsvorrat eingelesen
        /// </summary>
        public void ReadVisitorsFromCsv()
        {
            string fileName = Utils.MyFile.GetFullNameInApplicationTree("Visitors.csv");
            string[] lines = File.ReadAllLines(fileName, Encoding.Default);
            string[] columns;
            for (int i = 1; i < lines.Length; i++)
            {
                columns = lines[i].Split(';');
                int id = int.Parse(columns[0]);
                int adults = int.Parse(columns[3]);
                DateTime entryDateAndTime = DateTime.Parse(columns[1] + " " + columns[2]);
                Visitor visitor = new Visitor(id, adults, entryDateAndTime);
                _listOdVisitors.Add(visitor);
            }
            Log?.Invoke(this, $"Read {_listOdVisitors.Count} visitors with {_listOdVisitors.Sum(s => s.Adults)} adults from csv-file");
        }

        /// <summary>
        /// Die Simulation startet 60 Minuten, bevor der erste Besucher laut csv-Datei kommt.
        /// Der Beschleunigungsfaktor wird auf 6000 gesetzt.
        /// </summary>
        public void StartSimulation()
        {
            FastClock.Instance.Factor = 6000;
            DateTime timeToStart = _listOdVisitors.Select(s => s.EntryTime).Min().AddMinutes(-60);
            FastClock.Instance.Time = timeToStart;
            FastClock.Instance.OneMinuteIsOver += Instance_OneMinuteIsOver;
            FastClock.Instance.IsRunning = true;
        }
        /// <summary>
        /// Checks the sum of visitors and when the minimum of people per presentation is reached,
        /// the presentation will start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fastClockTime"></param>
        private void Instance_OneMinuteIsOver(object sender, DateTime fastClockTime)
        {
            List<Visitor> waitingPeople = new List<Visitor>();
            waitingPeople = _listOdVisitors.Where(w => w.EntryTime <= fastClockTime).ToList();
            if (waitingPeople.Count + waitingPeople.Sum(s => s.Adults) >= MIN_PEOPLE_PER_PRESENTATION
                && !Presentation.Instance.IsRunning)
            {
                Presentation.Instance.StartPresentation(waitingPeople, Log,_listOdVisitors.Where(w => w.EntryTime <= fastClockTime).ToList());
            }
            if (Presentation.Instance.IsRunning)
            {
                foreach (Visitor visitor in waitingPeople)
                {
                    _listOdVisitors.Remove(visitor);
                }
            }
        }
        #endregion
    }
}
