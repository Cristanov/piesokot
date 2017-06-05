namespace NaSpacerDo.Logic
{
    public interface IFileService
    {
        /// <summary>
        /// Zapisuje plik w systemie plików i zwraca do niego ścieżkę.
        /// </summary>
        /// <param name="fileName">Nazwa pliku wraz z rozszerzeniem</param>
        /// <param name="companyName">Nazwa obiektu</param>
        /// <param name="content">Zawartość pliku</param>
        /// <returns></returns>
        string Save(string fileName, string companyName, byte[] content);

        /// <summary>
        /// Usuwa folder jeśli istnieje. Wraz z podfolderami i plikami
        /// </summary>
        /// <param name="path"></param>
        void Delete(string path);

        string CreateCompanyDirectory(string name);

        void DeleteFile(string path);

        /// <summary>
        /// Zwraca rozmiar pliku w bajtach
        /// </summary>
        /// <param name="id">Ścieżka do pliku</param>
        /// <returns>Rozmiar w bajtach</returns>
        long GetSize(string path);

        /// <summary>
        /// Zwraca fizyczną ścieżkę na podstawie relatywnej
        /// </summary>
        /// <param name="relativefilePath">Relatywna ścieżka</param>
        /// <returns>Fizyczna ścieżka</returns>
        string GetPhysicalPath(string relativefilePath);
    }
}
