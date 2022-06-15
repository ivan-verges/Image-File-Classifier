ScanPathFiles(basePath: "BASE_PATH", searchPattern: "*.*", searchOption: SearchOption.AllDirectories);

void ScanPathFiles(string basePath, string searchPattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
{
    if (string.IsNullOrEmpty(basePath) || !Directory.Exists(basePath))
    {
        return;
    }

    string[] matchingFilesPaths = Directory.GetFiles(basePath, searchPattern, searchOption);
    foreach (string filePath in matchingFilesPaths)
    {
        CreateDirectoryFromFile(filePath, true);
    }
}


void CreateDirectoryFromFile(string filePath, bool mergeHours)
{
    if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
    {
        return;
    }

    string[] weekDays = { "DOMINGO", "LUNES", "MARTES", "MIERCOLES", "JUEVES", "VIERNES", "SABADO" };
    string[] months = { "ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE" };

    DateTime creationDate = File.GetCreationTime(filePath);
    if (creationDate.Hour >= 0 && creationDate.Hour <= 7 && mergeHours)
    {
        creationDate = creationDate.AddDays(-1);
    }

    string folderName = "{0} {1} {2} {3}"
        .Replace("{0}", weekDays[(int)creationDate.DayOfWeek])
        .Replace("{1}", creationDate.ToString("dd"))
        .Replace("{2}", months[creationDate.Month - 1])
        .Replace("{3}", creationDate.ToString("yyyy"));

    string fileName = Path.GetFileName(filePath);
    string pathName = Path.GetFullPath(filePath).Replace(fileName, string.Empty);
    string newPath = (!pathName.Contains(folderName)) ? Path.Combine(pathName, folderName) : pathName;

    if (!Directory.Exists(newPath))
    {
        Directory.CreateDirectory(newPath);
    }

    newPath = Path.Combine(newPath, fileName);

    MoveFile(filePath, newPath);
}

void MoveFile(string sourceFile, string destinationFile)
{
    if (!sourceFile.Equals(destinationFile))
    {
        File.Move(sourceFile, destinationFile);
    }
}