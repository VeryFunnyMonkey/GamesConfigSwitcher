using GCS.Core;

namespace GCS.CLI
{
    public static class ProfileHelper
    {
        public static void HandleProfile(
            List<ConfigFile> configFiles, 
            List<string>? variables
        )
        {

            foreach (var configFile in configFiles)
            {
                FileHelper.profileCopier(configFile.SourceFile, configFile.DestinationFile);

                if (variables != null)
                {
                    var variableDict = new Dictionary<string, string>();

                    foreach (var variableString in variables)
                    {
                        var variableArray = variableString.Split(':', StringSplitOptions.RemoveEmptyEntries);
                        if ((variableArray.Length % 2) != 0)
                        {
                            Console.WriteLine("Invalid number of arguments in variable");
                            return;
                        }

                        var key = variableArray[0];
                        var value = variableArray[1];

                        variableDict.Add(key, value);
                    }

                    VariableHandler.useVariable(configFile.DestinationFile, variableDict);
                }
            }
        }
    }
}
