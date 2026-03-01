namespace Sintef.Apos.Sif.Model
{
    public class InputDeviceRequirements : SISDeviceRequirements
    {
        public Attribute<double?> Accuracy { get; protected set; } //1
        public Attribute<string> AlarmDescription { get; protected set; }
        public Attribute<bool?> AlarmResetAfterShutdownIsRequired { get; protected set; }
        public AttributeList<string> AlarmType { get; protected set; }
        public Attribute<bool?> MeasurementComparisonIsRequired { get; protected set; } //5
        public AttributeList<string> OtherAlarmType { get; protected set; }
        public Attribute<double?> RangeMax { get; protected set; }
        public Attribute<double?> RangeMin { get; protected set; }
        public AttributeList<double?> TripPointValue { get; protected set; }
        public Attribute<string> UnitOfMeasure { get; protected set; } //10

        public InputDeviceRequirements(Group parent, string name) : base(parent, name, 10)
        {
        }
    }
}
