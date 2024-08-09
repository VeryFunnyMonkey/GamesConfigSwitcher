using GCS.Core;

namespace GCS.CLI
{
    public static class ProfileHelper
    {
        public static void HandleProfile(
            string profilePath, 
            string configPath, 
            List<string>? variables
        )
        {
            FileHelper.profileCopier(profilePath, configPath);

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

                VariableHandler.useVariable(configPath, variableDict);
            }
        }
    }
}
