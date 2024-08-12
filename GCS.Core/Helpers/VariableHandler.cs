using System.Text;

namespace GCS.Core
{
    public static class VariableHandler
    {
        public static void useVariable(string profile, Dictionary<string, string> Variables)
        {
            var fileContents = File.ReadAllText(profile);

            foreach (var kvp in Variables)
            {
                var formattedVar = $"${{{kvp.Key}}}";

                if (fileContents.Contains(formattedVar))
                {
                    fileContents = fileContents.Replace(formattedVar, kvp.Value);
                }
            }

            File.WriteAllText(profile, fileContents);
        }
    }
}
