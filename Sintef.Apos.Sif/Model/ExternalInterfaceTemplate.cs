namespace Sintef.Apos.Sif.Model
{
    public class ExternalInterfaceTemplate
    {
        public string Name { get; }
        public string RefBaseClassPath { get; }

        public string RefBaseRoleClassPath { get; }
        public string RoleRequirements { get; }

        public ExternalInterfaceTemplate(string name, string refBaseClassPath, string refBaseRoleClassPath)
        {
            Name = name;
            RefBaseClassPath = refBaseClassPath;
            RefBaseRoleClassPath = refBaseRoleClassPath;

            var split = refBaseRoleClassPath.Split('/');

            RoleRequirements = split[split.Length - 1];
        }
    }
}
