namespace Sintef.Apos.Sif.Model
{
    public class InputDeviceComponent : SISComponent
    {
        public Decimal Accuracy { get; protected set; } //1
        public AlarmOrWarning AlarmOrWarning { get; protected set; }
        public String AlarmOrWarningText { get; protected set; }
        public String AlarmPriority { get; protected set; }
        public Boolean AlarmResetAfterShutdown { get; protected set; } //5
        public Boolean MeasurementComparison { get; protected set; }
        public Decimal RangeMax { get; protected set; }
        public Decimal RangeMin { get; protected set; }
        public String TripPointLevel { get; protected set; }
        public Decimal TripPointValue { get; protected set; } //10
        public String UnitOfMeasure { get; protected set; }

        public new const string RefBaseSystemUnitPath = "SIS Unit Classes/InputDeviceComponent";

        public InputDeviceComponent(Group parent, string name) : base(parent, name, 11)
        {
        }
    }
}
