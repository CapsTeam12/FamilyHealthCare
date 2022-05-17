namespace Contract.Constants
{
    public static class NotificationContentTemplate
    {
        public const string CancelledAppointment = 
            "Appointment is cancelled";
        public const string RescheduledAppointment = 
            "Your appointment will be rescheduled to the time {0} due to reason {1}";
        public const string NewAppointment = 
            "Patient {0} has booked an appointment at {1}";
    }
}
