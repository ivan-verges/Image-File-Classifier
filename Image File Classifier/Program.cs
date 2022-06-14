scanBasePath(basePath: @"C:\Users\verge\Downloads\Test");

void scanBasePath(string basePath, string searchPattern = "*.*")
{
    if (string.IsNullOrEmpty(basePath))
    {
        return;
    }

    string[] allfiles = Directory.GetFiles(basePath, searchPattern, SearchOption.AllDirectories);
    foreach (string file in allfiles)
    {
        createFolderFromDate(file, File.GetCreationTime(file));
    }
}


void createFolderFromDate(string filePath, DateTime creationDate)
{
    if (string.IsNullOrEmpty(filePath))
    {
        return;
    }

    string[] weekDays = { "DOMINGO", "LUNES", "MARTES", "MIERCOLES", "JUEVES", "VIERNES", "SABADO" };
    string[] months = { "ENE", "FEB", "MAR", "ABR", "MAY", "JUN", "JUL", "AGO", "SEP", "OCT", "NOV", "DIC" };
    
    string folderName = "{0} {1} {2} {3}"
        .Replace("{0}", weekDays[(int)creationDate.DayOfWeek])
        .Replace("{1}", creationDate.ToString("dd"))
        .Replace("{2}", months[creationDate.Month - 1])
        .Replace("{3}", creationDate.ToString("yyyy"));

    string fileName = Path.GetFileName(filePath);
    string pathName = Path.GetFullPath(filePath).Replace(fileName, "");
    string newPath = Path.Combine(pathName, folderName);

    if (!Directory.Exists(newPath))
    {
        Directory.CreateDirectory(newPath);
    }

    newPath = Path.Combine(newPath, fileName);

    moveFileToFolder(filePath, newPath);
}

void moveFileToFolder(string sourceFile, string destinationFile)
{
    File.Move(sourceFile, destinationFile);
}