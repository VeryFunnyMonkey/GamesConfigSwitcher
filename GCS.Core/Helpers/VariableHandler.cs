using System.Text;

namespace GCS.Core
{
    public static class VariableHandler
    {
        // List of encodings to attempt
        private static readonly Encoding[] Encodings = new Encoding[]
        {
            Encoding.UTF8,
            Encoding.Unicode,  // UTF-16 LE
            Encoding.BigEndianUnicode,  // UTF-16 BE
            Encoding.UTF32,
            Encoding.ASCII
        };

        public static void useVariable(string profile, Dictionary<string, string> Variables)
        {
            string fileContents = null;

            // Attempt to read the file with different encodings - not a massive fan of this, works for now but will come up with better solution in future.
            foreach (var encoding in Encodings)
            {
                try
                {
                    fileContents = File.ReadAllText(profile, encoding);
                    //_logger.LogDebug($"attempting to read file with encoding: {encoding.EncodingName}");

                    // Check if the file contains any of the variables
                    if (ContainsVariables(fileContents, Variables))
                    {
                        //_logger.LogDebug($"found variable with encoding: {encoding.EncodingName}");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    Console.WriteLine($"Failed to read the file with {encoding.EncodingName}: {ex.Message}");
                }
            }

            if (fileContents == null)
            {
                throw new InvalidOperationException("Failed to read the file with any of the specified encodings.");
            }

            // Replace variables
            foreach (var kvp in Variables)
            {
                var formattedVar = $"${{{kvp.Key}}}";
                fileContents = fileContents.Replace(formattedVar, kvp.Value);
            }

            // Write back the modified content using the original encoding
            File.WriteAllText(profile, fileContents);
        }

        private static bool ContainsVariables(string fileContents, Dictionary<string, string> Variables)
        {
            foreach (var kvp in Variables)
            {
                var formattedVar = $"${{{kvp.Key}}}";
                if (fileContents.Contains(formattedVar))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
