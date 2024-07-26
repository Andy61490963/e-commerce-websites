using ShoppingBackstage.Service.Interface;
using Shopping.lib.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Net;
using Shopping.lib.Enums;
using Shopping.lib.Extensions;

namespace ShoppingBackstage.Controllers;

public class ServerFileController : BaseController
{
    private readonly IConfiguration _configuration;
    private readonly IServerFileService _service;
    private readonly IWebHostEnvironment _env;

    public ServerFileController(IConfiguration configuration, IServerFileService service, IWebHostEnvironment env)
    {
        _configuration = configuration;
        _service = service;
        _env = env;
        _physicalFolderPath = _configuration.GetValue<string>("VirtualFolderPath");
    }

    /// <summary>
    /// 實體檔案根路徑
    /// </summary>
    private const string VirtualFolderRootPath = "~/Shopping.uploads/";

    /// <summary>
    /// 
    /// </summary>
    private readonly string? _physicalFolderPath;

    public async Task<IActionResult?> Upload(string fn)
    {
        var file = Request.Form.Files[0];

        var folderName = fn.ToString();
        var virtualFolderPath = !string.IsNullOrWhiteSpace(folderName) ? VirtualFolderRootPath + folderName + "/" : VirtualFolderRootPath;
        var physicalFolderPath = $"{virtualFolderPath.Replace(VirtualFolderRootPath, _physicalFolderPath)}";

        var fileName = Guid.NewGuid().ToString();
        var extension = Path.GetExtension(file.FileName);
        var newFileName = $"{fileName}{extension}";
        var virtualFilePath = $"{virtualFolderPath}{newFileName}";
        var physicalFilePath = $"{virtualFilePath.Replace(VirtualFolderRootPath, _physicalFolderPath)}";

        if (string.IsNullOrWhiteSpace(extension))
        {
            return null;
        }

        // 檢查資料夾是否存在
        if (!Directory.Exists(physicalFolderPath))
        {
            Directory.CreateDirectory(physicalFolderPath);
        }

        while (System.IO.File.Exists(_physicalFolderPath + fileName + extension))
        {
            fileName = Guid.NewGuid().ToString();
            newFileName = $"{fileName}{extension}";
            virtualFilePath = $"{virtualFolderPath}{newFileName}";
            physicalFilePath = $"{virtualFilePath.Replace(VirtualFolderRootPath, _physicalFolderPath)}";
        }

        var model = new ServerFileViewModel
        {
            id_ = Guid.Parse(fileName),
            file_name_ = newFileName,
            display_name_ = file.FileName.Split('\\').LastOrDefault(),
            file_path_ = virtualFilePath,
            extension_ = extension,
            create_time_ = DateTime.Now
        };

        // 存檔
        await using (Stream fileStream = new FileStream(physicalFilePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        model.file_size_ = new FileInfo(physicalFilePath)?.Length.ToFileSizeString();

        _service.Save(model);
        //_service.OperationLogger( ActionType.Add, "上傳檔案", model.display_name_ ?? newFileName, model.id_, null, CurrentUser.Id );

        return Json(model);
    }

    public IActionResult GetFile(Guid id)
    {
        var file = _service.Get(id);

        if (file == null)
        {
            return new StatusCodeResult(HttpStatusCode.NotFound.ToInt());
        }

        var defaultContentType = "application/octet-stream";

        var provider = new FileExtensionContentTypeProvider();

        if (!provider.TryGetContentType(file.file_name_, out var contentType))
        {
            contentType = defaultContentType;
        }
        var path = PhysicalFile($"{file.file_path_.Replace(VirtualFolderRootPath, _physicalFolderPath)}", contentType, file.display_name_);

        return path;
    }

    public IActionResult Get(Guid id)
    {
        return GetFile(id);
    }
}
