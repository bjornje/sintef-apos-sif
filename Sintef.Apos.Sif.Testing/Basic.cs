using Sintef.Apos.Sif.Model;

namespace Sintef.Apos.Sif.Testing
{
    public class Basic
    {
        [Fact]
        public void VerifyModelVersion()
        {
            Assert.Equal(AttributeValues.Version, Definition.Version);
        }

        [Fact]
        public void ScientificNotification()
        {
            var builder = new Builder();

            var sif = builder.SIFs.Append("SIF-00ABC23");

            sif.MaximumAllowableDemandRate.StringValue = "3.5E-07";

            Assert.True(sif.MaximumAllowableDemandRate.IsValid(out var _));
            Assert.Equal(0.00000035, sif.MaximumAllowableDemandRate.Value);
            Assert.Equal("3.5E-07", sif.MaximumAllowableDemandRate.StringValue);

            sif.MaximumAllowableDemandRate.StringValue = "0.00000035";

            Assert.True(sif.MaximumAllowableDemandRate.IsValid(out var _));
            Assert.Equal(0.00000035, sif.MaximumAllowableDemandRate.Value);
            Assert.Equal("0.00000035", sif.MaximumAllowableDemandRate.StringValue);

            sif.MaximumAllowableDemandRate.Value = 3.5E-07;

            Assert.True(sif.MaximumAllowableDemandRate.IsValid(out var _));
            Assert.Equal(0.00000035, sif.MaximumAllowableDemandRate.Value);
            Assert.Equal("3.5E-07", sif.MaximumAllowableDemandRate.StringValue);

            sif.MaximumAllowableDemandRate.Value = 0.00000035;
            Assert.True(sif.MaximumAllowableDemandRate.IsValid(out var _));
            Assert.Equal(0.00000035, sif.MaximumAllowableDemandRate.Value);
            Assert.Equal("3.5E-07", sif.MaximumAllowableDemandRate.StringValue);
        }

        [Fact]
        public void ScientificNotification2()
        {
            var builder = new Builder();

            var sif = builder.SIFs.Append("SIF-00ABC23");

            sif.MaximumAllowableDemandRate.StringValue = "3E-07";

            Assert.True(sif.MaximumAllowableDemandRate.IsValid(out var _));
            Assert.Equal(0.0000003, sif.MaximumAllowableDemandRate.Value);
            Assert.Equal("3E-07", sif.MaximumAllowableDemandRate.StringValue);

            sif.MaximumAllowableDemandRate.StringValue = "0.0000003";

            Assert.True(sif.MaximumAllowableDemandRate.IsValid(out var _));
            Assert.Equal(0.0000003, sif.MaximumAllowableDemandRate.Value);
            Assert.Equal("0.0000003", sif.MaximumAllowableDemandRate.StringValue);

            sif.MaximumAllowableDemandRate.Value = 3E-07;

            Assert.True(sif.MaximumAllowableDemandRate.IsValid(out var _));
            Assert.Equal(0.0000003, sif.MaximumAllowableDemandRate.Value);
            Assert.Equal("3E-07", sif.MaximumAllowableDemandRate.StringValue);

            sif.MaximumAllowableDemandRate.Value = 0.0000003;
            Assert.True(sif.MaximumAllowableDemandRate.IsValid(out var _));
            Assert.Equal(0.0000003, sif.MaximumAllowableDemandRate.Value);
            Assert.Equal("3E-07", sif.MaximumAllowableDemandRate.StringValue);
        }


        [Fact]
        public void OrderedList()
        {
            var builder = new Builder();

            var sif = builder.SIFs.Append("SIF-00ABC23");

            Assert.True(sif.QuantificationMethodOrTool.IsOrderedList);

            sif.QuantificationMethodOrTool.Values = ["a", "b", "c"];

            Assert.Equal(3, sif.QuantificationMethodOrTool.Items.Count);
            Assert.Equal("a", sif.QuantificationMethodOrTool.Items[0].ObjectValue);
            Assert.Equal("b", sif.QuantificationMethodOrTool.Items[1].ObjectValue);
            Assert.Equal("c", sif.QuantificationMethodOrTool.Items[2].ObjectValue);

            var array = sif.QuantificationMethodOrTool.Values.ToArray();

            Assert.Equal("a", array[0]);
            Assert.Equal("b", array[1]);
            Assert.Equal("c", array[2]);

            var group = sif.Groups.AppendInputDevice();
            var component = group.Components.Append("TT-1001") as InputDeviceRequirements;

            component.TripPointValue.Values = [3.75, 4.789, 8.79, 11.436];

            Assert.Equal(4, component.TripPointValue.Items.Count);
            Assert.Equal(3.75, component.TripPointValue.Items[0].ObjectValue);
            Assert.Equal(4.789, component.TripPointValue.Items[1].ObjectValue);
            Assert.Equal(8.79, component.TripPointValue.Items[2].ObjectValue);
            Assert.Equal(11.436, component.TripPointValue.Items[3].ObjectValue);

            var array2 = component.TripPointValue.Values.ToArray();

            Assert.Equal(3.75, array2[0]);
            Assert.Equal(4.789, array2[1]);
            Assert.Equal(8.79, array2[2]);
            Assert.Equal(11.436, array2[3]);

            var list = new List<double?> { 0.89, 0.42 };

            component.TripPointValue.Values = list;
            Assert.Equal(2, component.TripPointValue.Items.Count);
            Assert.Equal(0.89, component.TripPointValue.Items[0].ObjectValue);
            Assert.Equal(0.42, component.TripPointValue.Items[1].ObjectValue);

            var list2 = component.TripPointValue.Values.ToList();

            Assert.Equivalent(list, list2);

        }

        [Fact]
        public void BuildSaveLoadCompare()
        {
            var builder = new Builder();

            var sif = builder.SIFs.Append("SIF-00ABC23");
            sif.SafetyIntegrityLevelRequirement.Value = v_SILLevel.SIL_1;

            var initiator = sif.Groups.AppendInputDevice();
            initiator.VoteWithinGroup(2, 2);

            var logicSolver = sif.Groups.AppendLogicSolver();
            logicSolver.VoteWithinGroup(1, 1);

            var finalElement = sif.Groups.AppendFinalElement();
            finalElement.VoteWithinGroup(1, 1);

            Assert.Equal(3, sif.Groups.Count());

            var initiatorComponent1 = initiator.Components.Append("TT-1001");
            initiatorComponent1.MaximumTestIntervalForSILCompliance.StringValue = "3000";
            initiatorComponent1.TestCoverage.StringValue = "25.7";


            var initiatorComponent2 = initiator.Components.Append("TT-1002");
            initiatorComponent2.MaximumTestIntervalForSILCompliance.StringValue = "6000";
            initiatorComponent2.TestCoverage.StringValue = "15.8";



            var logicSolverComponent = logicSolver.Components.Append("C01");
            logicSolverComponent.MaximumTestIntervalForSILCompliance.StringValue = "7000";


            var finalElementComponent = finalElement.Components.Append("ESV-3023");
            finalElementComponent.MaximumTestIntervalForSILCompliance.StringValue = "2000";

            Assert.Equal(2, initiator.MInVotingMooN.Value);
            Assert.Equal(2, initiator.NInVotingMooN.Value);
            Assert.Equal(3000.0, initiatorComponent1.MaximumTestIntervalForSILCompliance.Value);
            Assert.Equal(25.7, initiatorComponent1.TestCoverage.Value);
            Assert.Equal(6000.0, initiatorComponent2.MaximumTestIntervalForSILCompliance.Value);
            Assert.Equal(15.8, initiatorComponent2.TestCoverage.Value);
            Assert.Equal(7000.0, logicSolverComponent.MaximumTestIntervalForSILCompliance.Value);
            Assert.Equal(2000.0, finalElementComponent.MaximumTestIntervalForSILCompliance.Value);


            var sifIsValid = builder.Validate();
            Assert.NotEmpty(builder.Errors);
            Assert.False(sifIsValid);
            Assert.Equal(builder.Errors.Count(), builder.Errors.Count(x => x.Attribute != null && x.Attribute.IsMandatory));


            const string filename = "SIF-00ABC23.aml";

            builder.SaveToFile(filename);

            var builder2 = new Builder();
            builder2.LoadFromFile(filename);

            var sif2IsValid = builder2.Validate();
            Assert.NotEmpty(builder2.Errors);
            Assert.False(sif2IsValid);

            Assert.Equal(builder2.Errors.Count(), builder2.Errors.Count(x => x.Attribute != null && x.Attribute.IsMandatory));

            Assert.Equal(builder.Errors.Count(), builder2.Errors.Count());

            var sif2 = builder2.SIFs.Single();
            Assert.Equal(3, sif2.Groups.Count());


            Assert.Equal("SIF-00ABC23", sif2.SIFID.Value);
            Assert.Equal(v_SILLevel.SIL_1, sif2.SafetyIntegrityLevelRequirement.Value);

            var initiator2 = sif2.InputDevice;
            var logicSolver2 = sif2.LogicSolver;
            var finalElement2 = sif2.FinalElement;

            Assert.Equal(2, initiator2.Components.Count());
            Assert.Single(logicSolver2.Components);
            Assert.Single(finalElement2.Components);

            Assert.Equal("2", initiator2.MInVotingMooN.StringValue);
            Assert.Equal("2", initiator2.NInVotingMooN.StringValue);

            var initiator2Component1 = initiator2.Components.Single(x => x.TagName.StringValue == "TT-1001");
            Assert.Equal("3000", initiator2Component1.MaximumTestIntervalForSILCompliance.StringValue);
            Assert.Equal("25.7", initiator2Component1.TestCoverage.StringValue);

            var initiator2Component2 = initiator2.Components.Single(x => x.TagName.StringValue == "TT-1002");
            Assert.Equal("6000", initiator2Component2.MaximumTestIntervalForSILCompliance.StringValue);
            Assert.Equal("15.8", initiator2Component2.TestCoverage.StringValue);


            var logicSolver2Component = logicSolver2.Components.Single(x => x.TagName.StringValue == "C01");
            Assert.Equal("7000", logicSolver2Component.MaximumTestIntervalForSILCompliance.StringValue);

            var finalElement2Component = finalElement2.Components.Single(x => x.TagName.StringValue == "ESV-3023");
            Assert.Equal("2000", finalElement2Component.MaximumTestIntervalForSILCompliance.StringValue);

            Assert.Equal(2, initiator2.MInVotingMooN.Value);
            Assert.Equal(2, initiator2.NInVotingMooN.Value);
            Assert.Equal(3000.0, initiator2Component1.MaximumTestIntervalForSILCompliance.Value);
            Assert.Equal(25.7, initiator2Component1.TestCoverage.Value);
            Assert.Equal(6000.0, initiator2Component2.MaximumTestIntervalForSILCompliance.Value);
            Assert.Equal(15.8, initiator2Component2.TestCoverage.Value);
            Assert.Equal(7000.0, logicSolver2Component.MaximumTestIntervalForSILCompliance.Value);
            Assert.Equal(2000.0, finalElement2Component.MaximumTestIntervalForSILCompliance.Value);

            Assert.True(sif.IsSameAs(sif2));
        }
        [Fact]
        public void BuildSaveToLoadFromStreamCompare()
        {
            var builder = new Builder();

            var sif = builder.SIFs.Append("SIF-00ABC23");
            sif.SafetyIntegrityLevelRequirement.Value = v_SILLevel.SIL_1;

            var initiator = sif.Groups.AppendInputDevice();
            initiator.VoteWithinGroup(2, 2);

            var logicSolver = sif.Groups.AppendLogicSolver();
            logicSolver.VoteWithinGroup(1, 1);

            var finalElement = sif.Groups.AppendFinalElement();
            finalElement.VoteWithinGroup(1, 1);

            Assert.Equal(3, sif.Groups.Count());

            var initiatorComponent1 = initiator.Components.Append("TT-1001");
            initiatorComponent1.MaximumTestIntervalForSILCompliance.StringValue = "3000";
            initiatorComponent1.TestCoverage.StringValue = "25.7";


            var initiatorComponent2 = initiator.Components.Append("TT-1002");
            initiatorComponent2.MaximumTestIntervalForSILCompliance.StringValue = "6000";
            initiatorComponent2.TestCoverage.StringValue = "15.8";



            var logicSolverComponent = logicSolver.Components.Append("C01");
            logicSolverComponent.MaximumTestIntervalForSILCompliance.StringValue = "7000";


            var finalElementComponent = finalElement.Components.Append("ESV-3023");
            finalElementComponent.MaximumTestIntervalForSILCompliance.StringValue = "2000";

            Assert.Equal(2, initiator.MInVotingMooN.Value);
            Assert.Equal(2, initiator.NInVotingMooN.Value);
            Assert.Equal(3000.0, initiatorComponent1.MaximumTestIntervalForSILCompliance.Value);
            Assert.Equal(25.7, initiatorComponent1.TestCoverage.Value);
            Assert.Equal(6000.0, initiatorComponent2.MaximumTestIntervalForSILCompliance.Value);
            Assert.Equal(15.8, initiatorComponent2.TestCoverage.Value);
            Assert.Equal(7000.0, logicSolverComponent.MaximumTestIntervalForSILCompliance.Value);
            Assert.Equal(2000.0, finalElementComponent.MaximumTestIntervalForSILCompliance.Value);


            var sifIsValid = builder.Validate();
            Assert.NotEmpty(builder.Errors);
            Assert.False(sifIsValid);
            Assert.Equal(builder.Errors.Count(), builder.Errors.Count(x => x.Attribute != null && x.Attribute.IsMandatory));


            using var outputStream = new MemoryStream();
            builder.SaveToStream(outputStream);

            var builder2 = new Builder();
            builder2.LoadFromStream(outputStream);

            var sif2IsValid = builder2.Validate();
            Assert.NotEmpty(builder2.Errors);
            Assert.False(sif2IsValid);

            Assert.Equal(builder2.Errors.Count(), builder2.Errors.Count(x => x.Attribute != null && x.Attribute.IsMandatory));

            Assert.Equal(builder.Errors.Count(), builder2.Errors.Count());

            var sif2 = builder2.SIFs.Single();
            Assert.Equal(3, sif2.Groups.Count());


            Assert.Equal("SIF-00ABC23", sif2.SIFID.Value);
            Assert.Equal(v_SILLevel.SIL_1, sif2.SafetyIntegrityLevelRequirement.Value);

            var initiator2 = sif2.InputDevice;
            var logicSolver2 = sif2.LogicSolver;
            var finalElement2 = sif2.FinalElement;

            Assert.Equal(2, initiator2.Components.Count());
            Assert.Single(logicSolver2.Components);
            Assert.Single(finalElement2.Components);

            Assert.Equal("2", initiator2.MInVotingMooN.StringValue);
            Assert.Equal("2", initiator2.NInVotingMooN.StringValue);

            var initiator2Component1 = initiator2.Components.Single(x => x.TagName.StringValue == "TT-1001");
            Assert.Equal("3000", initiator2Component1.MaximumTestIntervalForSILCompliance.StringValue);
            Assert.Equal("25.7", initiator2Component1.TestCoverage.StringValue);

            var initiator2Component2 = initiator2.Components.Single(x => x.TagName.StringValue == "TT-1002");
            Assert.Equal("6000", initiator2Component2.MaximumTestIntervalForSILCompliance.StringValue);
            Assert.Equal("15.8", initiator2Component2.TestCoverage.StringValue);


            var logicSolver2Component = logicSolver2.Components.Single(x => x.TagName.StringValue == "C01");
            Assert.Equal("7000", logicSolver2Component.MaximumTestIntervalForSILCompliance.StringValue);

            var finalElement2Component = finalElement2.Components.Single(x => x.TagName.StringValue == "ESV-3023");
            Assert.Equal("2000", finalElement2Component.MaximumTestIntervalForSILCompliance.StringValue);

            Assert.Equal(2, initiator2.MInVotingMooN.Value);
            Assert.Equal(2, initiator2.NInVotingMooN.Value);
            Assert.Equal(3000.0, initiator2Component1.MaximumTestIntervalForSILCompliance.Value);
            Assert.Equal(25.7, initiator2Component1.TestCoverage.Value);
            Assert.Equal(6000.0, initiator2Component2.MaximumTestIntervalForSILCompliance.Value);
            Assert.Equal(15.8, initiator2Component2.TestCoverage.Value);
            Assert.Equal(7000.0, logicSolver2Component.MaximumTestIntervalForSILCompliance.Value);
            Assert.Equal(2000.0, finalElement2Component.MaximumTestIntervalForSILCompliance.Value);

            Assert.True(sif.IsSameAs(sif2));
        }

        [Fact]
        public void AddAttribute()
        {
            const string attributeName = "MyNewAttribute";

            var types = Definition.GetTypes();

            var refAttributeType = types.FirstOrDefault(x => x.EndsWith("/string"));
            Assert.NotNull(refAttributeType);

            var isOrderedList = false;
            var targetNode = typeof(SafetyInstrumentedFunction).Name;

            Assert.True(Definition.TryAddAttribute(attributeName, refAttributeType, isOrderedList, targetNode));

            var builder = new Builder();

            var sif = builder.SIFs.Append("SIF-00ABC23");

            builder.PushAttributes();

            var attribute = sif.Attributes.FirstOrDefault(x => x.Name == attributeName);

            Assert.NotNull(attribute);
            Assert.IsType<Attribute<string>>(attribute);

            var myNewAttribute = attribute as Attribute<string>;

            Assert.NotNull(myNewAttribute);

            myNewAttribute.Value = "Hello";

            Assert.Equal("Hello", myNewAttribute.ObjectValue);
            Assert.Equal("Hello", myNewAttribute.StringValue);
        }

        [Fact]
        public void AddOrderedListAttribute()
        {
            const string attributeName = "MyNewAttributeList";

            var types = Definition.GetTypes();

            var refAttributeType = types.FirstOrDefault(x => x.EndsWith("/integer"));
            Assert.NotNull(refAttributeType);

            var isOrderedList = true;
            var targetNode = typeof(SafetyInstrumentedFunction).Name;

            Assert.True(Definition.TryAddAttribute(attributeName, refAttributeType, isOrderedList, targetNode));

            var builder = new Builder();

            var sif = builder.SIFs.Append("SIF-00ABC23");

            builder.PushAttributes();

            var attribute = sif.Attributes.FirstOrDefault(x => x.Name == attributeName);

            Assert.NotNull(attribute);
            Assert.IsType<AttributeList<int?>>(attribute);

            var myNewAttribute = attribute as AttributeList<int?>;

            Assert.NotNull(myNewAttribute);

            myNewAttribute.Values = [81, 82, 83];

            Assert.Equal(81, myNewAttribute.Items[0].ObjectValue);
            Assert.Equal(82, myNewAttribute.Items[1].ObjectValue);
            Assert.Equal(83, myNewAttribute.Items[2].ObjectValue);

            Assert.Equal("81", myNewAttribute.Items[0].StringValue);
            Assert.Equal("82", myNewAttribute.Items[1].StringValue);
            Assert.Equal("83", myNewAttribute.Items[2].StringValue);
        }


    }
}