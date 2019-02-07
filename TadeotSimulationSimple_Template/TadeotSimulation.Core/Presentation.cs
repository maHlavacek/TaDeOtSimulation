using System;
using System.Collections.Generic;

namespace TadeotSimulation.Core
{
    public class Presentation
    {
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

    }
}
