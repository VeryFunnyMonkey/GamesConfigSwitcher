namespace GCS.Core
{
    public static class VariableHandler
    {
        public static bool useVariable(string profile, Dictionary<string, string> Variables)
        {
            foreach (var kvp in Variables)
            {
                var formattedVar = $"${{{kvp.Key}}}";

                var fileContents = File.ReadAllText(profile);

                Console.WriteLine(formattedVar);

                if(fileContents.Contains(formattedVar))
                {
                    fileContents = fileContents.Replace(formattedVar, kvp.Value);
                    File.WriteAllText(profile, fileContents);
                    return true;
                }

            }
            return false;
        }
    } 
}