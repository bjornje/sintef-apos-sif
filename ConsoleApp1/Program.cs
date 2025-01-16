using Sintef.Apos.Sif;

Console.WriteLine("Hello, lets build a simple SIF and save to aml-file");
var builder = new Builder();

var sif = builder.SIFs.Append("MyTestSif");
var subsystem = sif.Subsystems.Append();

var inputDevice = subsystem.CreateInputDevice();
var inputDeviceGroup = inputDevice.Groups.Append();
var initiatorComponent1 = inputDeviceGroup.Components.Append("TT-1001");
var initiatorComponent2 = inputDeviceGroup.Components.Append("TT-1002");

var logicSolver = subsystem.CreateLogicSolver();
var logicSolverGroup = logicSolver.Groups.Append();
var solverComponent1 = logicSolverGroup.Components.Append("M01");

Console.WriteLine($"{initiatorComponent1.Path}");
Console.WriteLine($"{initiatorComponent2.Path}");
Console.WriteLine($"{solverComponent1.Path}");

var isValid = builder.Validate();
Console.WriteLine($"SIF is valid? {isValid}.");

if (isValid)
{
    var outputFilename = sif.SIFID.Value + ".aml";
    builder.SaveToFile(outputFilename);
    Console.WriteLine($"Result written to file {outputFilename}");
}
else
{
    foreach(var error in builder.Errors) Console.WriteLine($"[{error.Node.Path}] {error.Message}");
}

