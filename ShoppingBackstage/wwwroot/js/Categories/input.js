// filePond.js

function initFilePond(layoutFileObject, bannerFileObject) {
    FilePond.registerPlugin(FilePondPluginImagePreview);
    FilePond.registerPlugin(FilePondPluginFileValidateType);
    FilePond.registerPlugin(FilePondPluginImageTransform);
    FilePond.registerPlugin(FilePondPluginImageResize);
    FilePond.registerPlugin(FilePondPluginGetFile);

    const initPond = (element, inputSelector) => {
        const pond = FilePond.create(element, {
            imageResizeTargetWidth: 1920,
            imageResizeTargetHeight: 1070,
            allowImageResize: true,
            allowDownloadByUrl: true,
            allowImageCrop: true,
            imageCropAspectRatio: '1920:1070',
            labelIdle: '上傳檔案', // 輸入文字
            server: {
                process: '/ServerFile/Upload?fn=Categories',
                load: '/ServerFile/Get/',
            },
            files: JSON.parse($(inputSelector).val()),
        });

        pond.on('init', function () {
            // 動態創建樣式，避免css全域汙染
            addImportantStyles();
        });

        function addImportantStyles() {
            var style = document.createElement('style');
            document.head.appendChild(style);
            style.innerHTML = `
                .filepond--credits {
                    display: none !important;
                }
            `;
        }
    };

    initPond(layoutFileObject, "#layoutFilesAttach");
    initPond(bannerFileObject, "#bannerFilesAttach");
}
