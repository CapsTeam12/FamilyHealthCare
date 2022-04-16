namespace Contract.Constants
{
    public static class NotificationContentTemplate
    {
        public const string CancelledAppointment = 
            "Your appointment with the doctor is canceled due to {0}";
        public const string RescheduledAppointment = 
            "Your appointment will be rescheduled to the time {0} due to reason {1}";
        public const string NewAppointment = 
            "Patient {0} has booked an appointment at {1}";
    }
}
