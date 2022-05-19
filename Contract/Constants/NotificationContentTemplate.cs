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
        public const string DeactivateUser =
           "Your account has been locked by the administrator!";
        public const string ActivateUser =
           "Your account has been unlocked by the administrator!";
    }
}
