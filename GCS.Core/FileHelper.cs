namespace GCS.UI
{ 

	public static class FileHelper
	{
		public static bool profileCopier(string sourceFilePath, string destinationFilePath)
		{
            try
            {
                if (File.Exists(sourceFilePath) && Directory.Exists(Path.GetDirectoryName(destinationFilePath)))
                {
                    // Copy the file to the destination
                    File.Copy(sourceFilePath, destinationFilePath, true);

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return false;
        }
    }

}