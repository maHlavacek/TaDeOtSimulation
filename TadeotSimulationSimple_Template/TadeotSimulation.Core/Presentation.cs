using System;
using System.Collections.Generic;
using System.Linq;

namespace TadeotSimulation.Core
{
    public class Presentation
    {
        private DateTime _startTime;
        private List<Visitor> _listOfVisitors;
        private static Presentation _instance;
        public static Presentation Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Presentation();
                }
                return _instance;
            }
        }
        private const int PRESENTATION_MINUTES = 10;

        private Presentation()
        {
        }

        public void StartPresentation(List<Visitor> visitors,EventHandler<string> LogFromController)
        {
            _listOfVisitors = new List<Visitor>();
            _listOfVisitors = visitors;
            _startTime = FastClock.Instance.Time;
            FastClock.Instance.OneMinuteIsOver += Instance_OneMinuteIsOver;
            LogFromController?.Invoke(this, $"{_startTime}, Presentation started, Visitors: {_listOfVisitors.Count}, People: {_listOfVisitors.Count + _listOfVisitors.Sum(s => s.Adults)}");
        }

        private void Instance_OneMinuteIsOver(object sender, DateTime fastClockTime)
        {
            if(_startTime.AddMinutes(PRESENTATION_MINUTES) == fastClockTime)
            {
                
            }
        }
    }
}
