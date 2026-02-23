namespace Sintef.Apos.Sif.Testing
{
    public class Basic
    {
        [Fact]
        public void VerifyModelVersion()
        {
            Assert.Equal("26", Definition.Version);
        }

        [Fact]
        public void ScientificNotation()
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
        }

        [Fact]
        public void ScientificNotation2()
        {
            var builder = new Builder();

            var sif = builder.SIFs.Append("SIF-00ABC23");

            sif.MaximumAllowableDemandRate.Value = 3.5E-07;

            Assert.True(sif.MaximumAllowableDemandRate.IsValid(out var _));
            Assert.Equal(0.00000035, sif.MaximumAllowableDemandRate.Value);
            Assert.Equal("3.5E-07", sif.MaximumAllowableDemandRate.StringValue);

            sif.MaximumAllowableDemandRate.StringValue = "0.00000035";
            Assert.True(sif.MaximumAllowableDemandRate.IsValid(out var _));
            Assert.Equal(0.00000035, sif.MaximumAllowableDemandRate.Value);
            Assert.Equal("0.00000035", sif.MaximumAllowableDemandRate.StringValue);
        }

        [Fact]
        public void BuildSaveLoadCompare()
        {
            var builder = new Builder();

            var sif = builder.SIFs.Append("SIF-00ABC23");

            var initator = sif.Subsystems.AppendInputDevice();
            initator.VoteBetweenGroups(1, 1);

            var logicSolver = sif.Subsystems.AppendLogicSolver();
            logicSolver.VoteBetweenGroups(1, 1);

            var finalElement = sif.Subsystems.AppendFinalElement();
            finalElement.VoteBetweenGroups(1, 1);

            Assert.Equal(3, sif.Subsystems.Count());

            var initiatorGroup = initator.Groups.Append();
            initiatorGroup.VoteWithinGroup(1, 1);

            var logicSolverGroup = logicSolver.Groups.Append();
            logicSolverGroup.VoteWithinGroup(1, 1);

            var finalElementGroup = finalElement.Groups.Append();
            finalElementGroup.VoteWithinGroup(1, 1);

            initiatorGroup.VoteWithinGroup(2, 2);

            var initiatorComponent1 = initiatorGroup.Components.Append("TT-1001");
            initiatorComponent1.MaximumTestIntervalForSILCompliance.StringValue = "3000";
            initiatorComponent1.TestCoverage.StringValue = "25.7";


            var initiatorComponent2 = initiatorGroup.Components.Append("TT-1002");
            initiatorComponent2.MaximumTestIntervalForSILCompliance.StringValue = "6000";
            initiatorComponent2.TestCoverage.StringValue = "15.8";



            var logicSolverComponent = logicSolverGroup.Components.Append("C01");
            logicSolverComponent.MaximumTestIntervalForSILCompliance.StringValue = "7000";


            var finalElementComponent = finalElementGroup.Components.Append("ESV-3023");
            finalElementComponent.MaximumTestIntervalForSILCompliance.StringValue = "2000";

            Assert.Equal(2, initiatorGroup.MInVotingMooN.Value);
            Assert.Equal(2, initiatorGroup.NumberOfDevicesWithinGroup.Value);
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
            Assert.Equal(3, sif2.Subsystems.Count());


            Assert.Equal("SIF-00ABC23", sif2.SIFID.Value);

            var initiator2 = sif2.InputDevice;
            var logicSolver2 = sif2.LogicSolver;
            var finalElement2 = sif2.FinalElement;

            Assert.Equal("1", initiator2.MInVotingMooN.StringValue);
            Assert.Equal("1", initiator2.NumberOfGroups.StringValue);

            Assert.Equal("1", logicSolver2.MInVotingMooN.StringValue);
            Assert.Equal("1", logicSolver2.NumberOfGroups.StringValue);

            Assert.Equal("1", finalElement2.MInVotingMooN.StringValue);
            Assert.Equal("1", finalElement2.NumberOfGroups.StringValue);

            var initiator2Group = initiator2.Groups.Single();
            var logicSolver2Group = logicSolver2.Groups.Single();
            var finalElement2Group = finalElement2.Groups.Single();

            Assert.Equal(2, initiator2Group.Components.Count());
            Assert.Single(logicSolver2Group.Components);
            Assert.Single(finalElement2Group.Components);

            Assert.Equal("2", initiator2Group.MInVotingMooN.StringValue);
            Assert.Equal("2", initiator2Group.NumberOfDevicesWithinGroup.StringValue);

            var initiator2Component1 = initiator2Group.Components.Single(x => x.TagName.StringValue == "TT-1001");
            Assert.Equal("3000", initiator2Component1.MaximumTestIntervalForSILCompliance.StringValue);
            Assert.Equal("25.7", initiator2Component1.TestCoverage.StringValue);

            var initiator2Component2 = initiator2Group.Components.Single(x => x.TagName.StringValue == "TT-1002");
            Assert.Equal("6000", initiator2Component2.MaximumTestIntervalForSILCompliance.StringValue);
            Assert.Equal("15.8", initiator2Component2.TestCoverage.StringValue);


            var logicSolver2Component = logicSolver2Group.Components.Single(x => x.TagName.StringValue == "C01");
            Assert.Equal("7000", logicSolver2Component.MaximumTestIntervalForSILCompliance.StringValue);

            var finalElement2Component = finalElement2Group.Components.Single(x => x.TagName.StringValue == "ESV-3023");
            Assert.Equal("2000", finalElement2Component.MaximumTestIntervalForSILCompliance.StringValue);

            Assert.Equal(2, initiator2Group.MInVotingMooN.Value);
            Assert.Equal(2, initiator2Group.NumberOfDevicesWithinGroup.Value);
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

            var initator = sif.Subsystems.AppendInputDevice();
            initator.VoteBetweenGroups(1, 1);

            var logicSolver = sif.Subsystems.AppendLogicSolver();
            logicSolver.VoteBetweenGroups(1, 1);

            var finalElement = sif.Subsystems.AppendFinalElement();
            finalElement.VoteBetweenGroups(1, 1);

            Assert.Equal(3, sif.Subsystems.Count());

            var initiatorGroup = initator.Groups.Append();
            initiatorGroup.VoteWithinGroup(1, 1);

            var logicSolverGroup = logicSolver.Groups.Append();
            logicSolverGroup.VoteWithinGroup(1, 1);

            var finalElementGroup = finalElement.Groups.Append();
            finalElementGroup.VoteWithinGroup(1, 1);

            initiatorGroup.VoteWithinGroup(2, 2);

            var initiatorComponent1 = initiatorGroup.Components.Append("TT-1001");
            initiatorComponent1.MaximumTestIntervalForSILCompliance.StringValue = "3000";
            initiatorComponent1.TestCoverage.StringValue = "25.7";


            var initiatorComponent2 = initiatorGroup.Components.Append("TT-1002");
            initiatorComponent2.MaximumTestIntervalForSILCompliance.StringValue = "6000";
            initiatorComponent2.TestCoverage.StringValue = "15.8";



            var logicSolverComponent = logicSolverGroup.Components.Append("C01");
            logicSolverComponent.MaximumTestIntervalForSILCompliance.StringValue = "7000";


            var finalElementComponent = finalElementGroup.Components.Append("ESV-3023");
            finalElementComponent.MaximumTestIntervalForSILCompliance.StringValue = "2000";

            Assert.Equal(2, initiatorGroup.MInVotingMooN.Value);
            Assert.Equal(2, initiatorGroup.NumberOfDevicesWithinGroup.Value);
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
            Assert.Equal(3, sif2.Subsystems.Count());


            Assert.Equal("SIF-00ABC23", sif2.SIFID.Value);

            var initiator2 = sif2.InputDevice;
            var logicSolver2 = sif2.LogicSolver;
            var finalElement2 = sif2.FinalElement;

            Assert.Equal("1", initiator2.MInVotingMooN.StringValue);
            Assert.Equal("1", initiator2.NumberOfGroups.StringValue);

            Assert.Equal("1", logicSolver2.MInVotingMooN.StringValue);
            Assert.Equal("1", logicSolver2.NumberOfGroups.StringValue);

            Assert.Equal("1", finalElement2.MInVotingMooN.StringValue);
            Assert.Equal("1", finalElement2.NumberOfGroups.StringValue);

            var initiator2Group = initiator2.Groups.Single();
            var logicSolver2Group = logicSolver2.Groups.Single();
            var finalElement2Group = finalElement2.Groups.Single();

            Assert.Equal(2, initiator2Group.Components.Count());
            Assert.Single(logicSolver2Group.Components);
            Assert.Single(finalElement2Group.Components);

            Assert.Equal("2", initiator2Group.MInVotingMooN.StringValue);
            Assert.Equal("2", initiator2Group.NumberOfDevicesWithinGroup.StringValue);

            var initiator2Component1 = initiator2Group.Components.Single(x => x.TagName.StringValue == "TT-1001");
            Assert.Equal("3000", initiator2Component1.MaximumTestIntervalForSILCompliance.StringValue);
            Assert.Equal("25.7", initiator2Component1.TestCoverage.StringValue);

            var initiator2Component2 = initiator2Group.Components.Single(x => x.TagName.StringValue == "TT-1002");
            Assert.Equal("6000", initiator2Component2.MaximumTestIntervalForSILCompliance.StringValue);
            Assert.Equal("15.8", initiator2Component2.TestCoverage.StringValue);


            var logicSolver2Component = logicSolver2Group.Components.Single(x => x.TagName.StringValue == "C01");
            Assert.Equal("7000", logicSolver2Component.MaximumTestIntervalForSILCompliance.StringValue);

            var finalElement2Component = finalElement2Group.Components.Single(x => x.TagName.StringValue == "ESV-3023");
            Assert.Equal("2000", finalElement2Component.MaximumTestIntervalForSILCompliance.StringValue);

            Assert.Equal(2, initiator2Group.MInVotingMooN.Value);
            Assert.Equal(2, initiator2Group.NumberOfDevicesWithinGroup.Value);
            Assert.Equal(3000.0, initiator2Component1.MaximumTestIntervalForSILCompliance.Value);
            Assert.Equal(25.7, initiator2Component1.TestCoverage.Value);
            Assert.Equal(6000.0, initiator2Component2.MaximumTestIntervalForSILCompliance.Value);
            Assert.Equal(15.8, initiator2Component2.TestCoverage.Value);
            Assert.Equal(7000.0, logicSolver2Component.MaximumTestIntervalForSILCompliance.Value);
            Assert.Equal(2000.0, finalElement2Component.MaximumTestIntervalForSILCompliance.Value);

            Assert.True(sif.IsSameAs(sif2));
        }
    }
}