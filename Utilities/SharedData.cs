using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Utilities
{
    public class SharedData
    {
        private static SharedData instance;

        public static SharedData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SharedData();
                }
                return instance;
            }
        }

        private int currentUserId;

        public int CurrentUserId
        {
            get { return currentUserId; }
            set { currentUserId = value; }
        }
    }
}
