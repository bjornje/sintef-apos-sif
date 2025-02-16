namespace Sintef.Apos.Sif.Model
{
    public class InputDeviceComponent : SISComponent
    {
        public AlarmOrWarning AlarmOrWarning { get; protected set; }
        public String AlarmOrWarningText { get; protected set; }
        public String AlarmPriority { get; protected set; }
        public InputDeviceTrigger TripPointLevel { get; protected set; }
        public String UnitOfMeasure { get; protected set; }

        public new const string RefBaseSystemUnitPath = "SIS Unit Classes/InputDeviceComponent";

        public InputDeviceComponent(Group parent, string name) : base(parent, name, 5)
        {
        }
    }
}
