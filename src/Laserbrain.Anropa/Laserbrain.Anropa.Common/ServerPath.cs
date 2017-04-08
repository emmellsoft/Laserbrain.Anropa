using System.Text;

namespace Laserbrain.Anropa
{
    public static class ServerPaths // TODO: Clean up!
    {
        public static ServerPath GetServerPath(AsyncServiceMethodInfo asyncServiceMethodInfo)
        {
            const string redundantInterfacePrefix = "I";
            const string redundantSuffix = "Service";

            string serviceName = asyncServiceMethodInfo.ServiceType.Name;
            if (asyncServiceMethodInfo.ServiceType.IsInterface && serviceName.StartsWith(redundantInterfacePrefix))
            {
                serviceName = serviceName.Substring(redundantInterfacePrefix.Length);
            }

            if (serviceName.EndsWith(redundantSuffix))
            {
                serviceName = serviceName.Substring(0, serviceName.Length - redundantSuffix.Length);
            }

            var modulePath = new ModulePath(GetDashName(serviceName));

            return new ServerPath(modulePath, GetDashName(asyncServiceMethodInfo.MethodName));
        }

        private static string GetDashName(string name)
        {
            var text = new StringBuilder();

            bool prevUpper = true;
            foreach (char c in name)
            {
                bool isUpper = char.IsUpper(c);

                if (isUpper && !prevUpper)
                {
                    text.Append("-");
                }

                text.Append(isUpper ? char.ToLower(c) : c);

                prevUpper = isUpper;
            }

            return text.ToString();
        }
    }

    public class ModulePath
    {
        public ModulePath(string path)
        {
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }

            Path = path;
        }

        public string Path { get; }

        public override string ToString() => Path;
    }
    public class ServerPath
    {
        public ServerPath(ModulePath modulePath, string subPath)
        {
            if (!subPath.StartsWith("/"))
            {
                subPath = "/" + subPath;
            }

            ModulePath = modulePath;
            SubPath = subPath;
            FullPath = modulePath.Path + subPath;
        }

        public ModulePath ModulePath { get; }

        public string SubPath { get; }

        public string FullPath { get; }

        public override string ToString() => FullPath;
    }
}