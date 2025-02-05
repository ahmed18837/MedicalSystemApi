using MedicalSystemApi.Services.Interfaces;

namespace MedicalSystemApi.Services.Implements
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _baseUploadPath;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _baseUploadPath = Path.Combine(_environment.WebRootPath, "Images");

            if (!Directory.Exists(_baseUploadPath))
            {
                Directory.CreateDirectory(_baseUploadPath);
            }
        }

        public string? SaveFile(IFormFile? file, string category)
        {
            if (file == null || file.Length == 0)
            {
                return null; // No file uploaded
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Invalid file type. Only JPG, JPEG, and PNG are allowed");
            }

            const long maxFileSize = 2 * 1024 * 1024; //2MB
            if (file.Length > maxFileSize)
            {
                throw new InvalidOperationException("File size exceeds the maximum limit of 2 MB");
            }

            var categoryPath = Path.Combine(_baseUploadPath, category);
            if (!Directory.Exists(categoryPath))
            {
                Directory.CreateDirectory(categoryPath);
            }

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(categoryPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Path.Combine("Images", category, fileName).Replace("\\", "/");
        }

        public async Task DeleteFileAsync(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentException("File path cannot be null or empty");
            }

            var fullPath = Path.Combine(_environment.WebRootPath, relativePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                try
                {
                    await Task.Run(() => File.Delete(fullPath));
                }
                catch (Exception)
                {
                    throw new IOException($"Error occurred while deleting the file: {fullPath}");
                }
            }
            else
            {
                throw new FileNotFoundException($"File not found: {fullPath}");
            }
        }
    }
}
