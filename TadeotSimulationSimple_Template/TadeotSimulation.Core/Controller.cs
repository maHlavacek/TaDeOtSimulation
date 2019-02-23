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

        private int _countVisitors;
        private int _countPeople;
        private int _countForPresentation;
        private List<Visitor> _waitingPeople;
        private DateTime _lastPresentationFinished;

        public event EventHandler<string> Log;

        private List<Visitor> _listOfVisitors;
        private bool _presentationIsFinished = true;
        #endregion

        #region Constructor
        public Controller()
        {
            _listOfVisitors = new List<Visitor>();
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
                _listOfVisitors.Add(visitor);
            }
            Log?.Invoke(this, $"Read {_listOfVisitors.Count} visitors with {_listOfVisitors.Sum(s => s.Adults)} adults from csv-file");
        }

        /// <summary>
        /// Die Simulation startet 60 Minuten, bevor der erste Besucher laut csv-Datei kommt.
        /// Der Beschleunigungsfaktor wird auf 6000 gesetzt.
        /// </summary>
        public void StartSimulation()
        {
            FastClock.Instance.Factor = 6000;
            DateTime timeToStart = _listOfVisitors.Select(s => s.EntryTime).Min().AddMinutes(-60);
            FastClock.Instance.Time = timeToStart;
            FastClock.Instance.OneMinuteIsOver += Instance_OneMinuteIsOver;
            Presentation.Instance.PresentationFinished += Instance_PresentationFinished;
            FastClock.Instance.IsRunning = true;
        }

        /// <summary>
        /// Crate the message for the console
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="presentationIsFinished"></param>
        private void Instance_PresentationFinished(object sender, bool presentationIsFinished)
        {
            _presentationIsFinished = presentationIsFinished;
            StringBuilder stringBuilder = new StringBuilder();

            if (presentationIsFinished)
            {
                _lastPresentationFinished = FastClock.Instance.Time;
                stringBuilder.Append($"Presentation finished :{FastClock.Instance.Time.TimeOfDay}, ");

                if (_listOfVisitors.Count > 0)
                {
                    stringBuilder.Append($"Visitors {_countForPresentation}, ");
                    stringBuilder.Append($"waiting: { _waitingPeople.Count + _waitingPeople.Sum(s => s.Adults)} ");

                }
                else
                {
                    stringBuilder.Append($"Visitors {Presentation.Instance.SumOfVisitors}, ");
                    stringBuilder.Append($"waiting: { _waitingPeople.Count + _waitingPeople.Sum(s => s.Adults)} ");
                    stringBuilder.Append($"\n!!! SIMULATION BEENDET, beenden mit Eingabetaste...");
                    FastClock.Instance.OneMinuteIsOver -= Instance_OneMinuteIsOver;
                    FastClock.Instance.IsRunning = false;
                }

            }
            else
            {
                stringBuilder.Append($"Presentation started :{FastClock.Instance.Time.TimeOfDay}, ");
                stringBuilder.Append($"Visitors: {_countVisitors}, ");
                stringBuilder.Append($"People: {_countPeople}, ");
                stringBuilder.Append($"waiting: {_waitingPeople.Count + _waitingPeople.Sum(s => s.Adults)} ");
                _countForPresentation = _countPeople;
            }
            Log?.Invoke(this, stringBuilder.ToString());
        }

        /// <summary>
        /// One minute is over
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fastClockTime"></param>
        private void Instance_OneMinuteIsOver(object sender, DateTime fastClockTime)
        {
            OnOneMinuteIsOver(fastClockTime);
        }


        /// <summary>
        /// Checks the sum of visitors and when the minimum of people per presentation is reached,
        /// the presentation will start
        /// </summary>
        /// <param name="time"></param>
        public void OnOneMinuteIsOver(DateTime time)
        {         
            _waitingPeople = _listOfVisitors.Where(w => w.EntryTime <= time).ToList();
            _countVisitors = _waitingPeople.Count;
            _countPeople = _waitingPeople.Count + _waitingPeople.Sum(s => s.Adults);

            if (
                _waitingPeople.Count + _waitingPeople.Sum(s => s.Adults) >= MIN_PEOPLE_PER_PRESENTATION
                && _presentationIsFinished
                || _lastPresentationFinished.AddMinutes(MAX_WAITING_MINUTES) == time
                && _waitingPeople.Count > 0
                )
            {
                foreach (Visitor visitor in _waitingPeople)
                {
                    _listOfVisitors.Remove(visitor);
                }
                Presentation.Instance.StartPresentation(_waitingPeople);
            }
            
           
        }
        #endregion
    }
}
