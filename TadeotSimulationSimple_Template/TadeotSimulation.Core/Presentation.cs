using System;
using System.Collections.Generic;
using System.Linq;

namespace TadeotSimulation.Core
{
    public class Presentation
    {
        #region Fields
        private const int PRESENTATION_MINUTES = 10;

        private DateTime _startTime;
        private List<Visitor> _listOfVisitors;
        private List<Visitor> _waiters;
        private static Presentation _instance;
        private EventHandler<string> _logFromController;
        #endregion


        #region Properties
        public bool IsRunning { get; private set; }

        public static Presentation Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Presentation();
                }
                return _instance;
            }
        }
        #endregion

        #region Constructor
        private Presentation()
        {
            FastClock.Instance.OneMinuteIsOver += Instance_OneMinuteIsOver;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts the presentation and notify the console
        /// </summary>
        /// <param name="visitors"></param>
        /// <param name="LogFromController"></param>
        public void StartPresentation(List<Visitor> visitors, EventHandler<string> LogFromController,List<Visitor> waiters)
        {
            _listOfVisitors = new List<Visitor>();
            _listOfVisitors = visitors;
            _waiters = waiters;
            _startTime = FastClock.Instance.Time;
            if (_logFromController == null)
            {
                _logFromController = LogFromController;
            }
            IsRunning = true;
            _logFromController?.Invoke(this, $"{_startTime.TimeOfDay}, Presentation started, Visitors: {_listOfVisitors.Count}, People: {_listOfVisitors.Count + _listOfVisitors.Sum(s => s.Adults)}, waiting {_waiters.Count + _waiters.Sum(s => s.Adults)}");
        }

        /// <summary>
        /// Check if the presentation is finished and notify the console
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fastClockTime"></param>
        private void Instance_OneMinuteIsOver(object sender, DateTime fastClockTime)
        {
            if (_startTime.AddMinutes(PRESENTATION_MINUTES) == fastClockTime)
            {
                IsRunning = false;
                _logFromController?.Invoke(this, $"{fastClockTime.TimeOfDay}, Presentation finished, Visitors: {_listOfVisitors.Count}, People: {_listOfVisitors.Count + _listOfVisitors.Sum(s => s.Adults)}, waiting {_waiters.Count + _waiters.Sum(s => s.Adults)}");
            }
        }
        #endregion
    }
}
