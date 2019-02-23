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
        private static Presentation _instance;
        public event EventHandler<bool> PresentationFinished;
        #endregion


        #region Properties

        public int SumOfVisitors { get; private set; }

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
            _listOfVisitors = new List<Visitor>();

        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts the presentation and notify the console
        /// </summary>
        /// <param name="visitors"></param>
        /// <param name="LogFromController"></param>
        public void StartPresentation(List<Visitor> visitors)
        {
            _listOfVisitors = visitors;
            SumOfVisitors += visitors.Count;
            _startTime = FastClock.Instance.Time;
            PresentationFinished?.Invoke(this, false);
        }

        /// <summary>
        /// Check if the presentation is finished and notify the controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fastClockTime"></param>
        private void Instance_OneMinuteIsOver(object sender, DateTime fastClockTime)
        {
            if (_startTime.AddMinutes(PRESENTATION_MINUTES) == fastClockTime)
            {
                PresentationFinished?.Invoke(this, true);
            }
        }
        #endregion
    }
}
